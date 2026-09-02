using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Models;
using Oqtane.Repository;
using Oqtane.Shared;
using OpenEug.TenTrees.Module.Assessment.Repository;
using OpenEug.TenTrees.Models;
using OpenEug.TenTrees.Module.Grower.Repository;
using OpenEug.TenTrees.Shared;

namespace OpenEug.TenTrees.Module.Assessment.Services
{
    public class ServerAssessmentService : IAssessmentService
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IAssessmentNoteRepository _assessmentNoteRepository;
        private readonly IAssessmentPhotoRepository _assessmentPhotoRepository;
        private readonly IGrowerRepository _growerRepository;
        private readonly IFolderRepository _folderRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogManager _logger;

        public ServerAssessmentService(
            IAssessmentRepository assessmentRepository,
            IAssessmentNoteRepository assessmentNoteRepository,
            IAssessmentPhotoRepository assessmentPhotoRepository,
            IGrowerRepository growerRepository,
            IFolderRepository folderRepository,
            IFileRepository fileRepository,
            IUserRepository userRepository,
            ILogManager logger)
        {
            _assessmentRepository = assessmentRepository;
            _assessmentNoteRepository = assessmentNoteRepository;
            _assessmentPhotoRepository = assessmentPhotoRepository;
            _growerRepository = growerRepository;
            _folderRepository = folderRepository;
            _fileRepository = fileRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public Task<Models.Assessment> GetAssessmentAsync(int assessmentId, string mentorUsername = null)
        {
            var assessment = _assessmentRepository.GetAssessment(assessmentId);
            if (assessment != null && mentorUsername != null)
            {
                var grower = _growerRepository.GetGrower(assessment.GrowerId);
                if (grower == null || grower.MentorUsername != mentorUsername)
                    return Task.FromResult<Models.Assessment>(null);
            }
            return Task.FromResult(assessment);
        }

        public Task<List<Models.Assessment>> GetAssessmentsAsync(string mentorUsername = null)
        {
            var list = _assessmentRepository.GetAssessments().ToList();
            if (mentorUsername != null)
            {
                var mentorGrowerIds = _growerRepository.GetGrowersByMentor(mentorUsername)
                    .Select(g => g.GrowerId).ToHashSet();
                list = list.Where(a => mentorGrowerIds.Contains(a.GrowerId)).ToList();
            }
            return Task.FromResult(list);
        }

        public Task<List<Models.Assessment>> GetAssessmentsByGrowerAsync(int growerId, string mentorUsername = null)
        {
            if (mentorUsername != null)
            {
                var grower = _growerRepository.GetGrower(growerId);
                if (grower == null || grower.MentorUsername != mentorUsername)
                    return Task.FromResult(new List<Models.Assessment>());
            }
            return Task.FromResult(_assessmentRepository.GetAssessmentsByGrower(growerId).ToList());
        }

        public Task<Models.Assessment> AddAssessmentAsync(Models.Assessment assessment)
        {
            var grower = _growerRepository.GetGrower(assessment.GrowerId);
            if (grower == null)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Create, "Grower not found {GrowerId}", assessment.GrowerId);
                return Task.FromResult<Models.Assessment>(null);
            }

            assessment.PermaculturePrinciplesCount = CalculatePermaculturePrinciplesCount(assessment);
            assessment = _assessmentRepository.AddAssessment(assessment);
            _logger.Log(LogLevel.Information, this, LogFunction.Create, "Assessment Added {Assessment}", assessment);
            return Task.FromResult(assessment);
        }

        private int CalculatePermaculturePrinciplesCount(Models.Assessment assessment)
        {
            var count = 0;
            if (assessment.TreesLookHealthy) count++;
            if (!assessment.HasChemicalFertilizers) count++;
            if (!assessment.HasPesticides) count++;
            if (assessment.IsMulched) count++;
            if (assessment.IsMakingCompost) count++;
            if (assessment.IsCollectingWater) count++;
            if (!assessment.HasLeakyTaps) count++;
            if (assessment.IsGardenDesignedToCaptureWater) count++;
            if (assessment.IsUsingGreywater) count++;
            return count;
        }

        public Task<Models.Assessment> UpdateAssessmentAsync(Models.Assessment assessment)
        {
            assessment.PermaculturePrinciplesCount = CalculatePermaculturePrinciplesCount(assessment);
            assessment = _assessmentRepository.UpdateAssessment(assessment);
            _logger.Log(LogLevel.Information, this, LogFunction.Update, "Assessment Updated {Assessment}", assessment);
            return Task.FromResult(assessment);
        }

        public Task DeleteAssessmentAsync(int assessmentId)
        {
            foreach (var photo in _assessmentPhotoRepository.GetPhotosByAssessment(assessmentId))
            {
                DeleteOqtaneFile(photo.FileId);
            }
            _assessmentPhotoRepository.DeletePhotosByAssessment(assessmentId);
            _assessmentRepository.DeleteAssessment(assessmentId);
            _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Assessment Deleted {AssessmentId}", assessmentId);
            return Task.CompletedTask;
        }

        public Task<bool> CanSubmitAssessmentAsync(int growerId)
        {
            var grower = _growerRepository.GetGrower(growerId);
            return Task.FromResult(grower != null && grower.Status == GrowerStatus.Active);
        }

        public Task<List<AssessmentListDto>> GetAssessmentListAsync(int? villageId = null, int? cohortId = null, string mentorUsername = null, int? growerId = null)
        {
            return Task.FromResult(_assessmentRepository.GetAssessmentList(villageId, cohortId, mentorUsername, growerId).ToList());
        }

        public Task<List<Models.AssessmentNote>> GetNotesByAssessmentAsync(int assessmentId)
        {
            return Task.FromResult(_assessmentNoteRepository.GetNotesByAssessment(assessmentId).ToList());
        }

        public Task<List<Models.AssessmentNote>> GetNotesByGrowerAsync(int growerId)
        {
            return Task.FromResult(_assessmentNoteRepository.GetNotesByGrower(growerId).ToList());
        }

        public Task<Models.AssessmentNote> AddNoteAsync(Models.AssessmentNote note)
        {
            if (note == null)
            {
                return Task.FromResult<Models.AssessmentNote>(null);
            }

            if (note.NoteType != AssessmentNoteTypes.HomeVisit)
            {
                note.NoteType = AssessmentNoteTypes.General;
            }

            var assessment = _assessmentRepository.GetAssessment(note.AssessmentId, tracking: false);
            if (assessment == null)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Create, "AssessmentNote Add Failed — Assessment not found {AssessmentId}", note.AssessmentId);
                return Task.FromResult<Models.AssessmentNote>(null);
            }

            var created = _assessmentNoteRepository.AddNote(note);
            _logger.Log(LogLevel.Information, this, LogFunction.Create, "AssessmentNote Added {AssessmentNote}", created);
            return Task.FromResult(created);
        }

        public Task<List<Models.AssessmentProblem>> GetProblemsByAssessmentAsync(int assessmentId)
        {
            return Task.FromResult(_assessmentRepository.GetProblemsByAssessment(assessmentId).ToList());
        }

        public Task ReplaceProblemsAsync(int assessmentId, List<Models.AssessmentProblem> problems)
        {
            foreach (var p in problems)
                p.AssessmentId = assessmentId;
            _assessmentRepository.ReplaceProblems(assessmentId, problems);
            return Task.CompletedTask;
        }

        public Task<int?> GetPhotoFolderIdAsync(int assessmentId, string mentorUsername = null)
        {
            var folder = GetAssessmentPhotoFolder(assessmentId, mentorUsername);
            return Task.FromResult(folder == null ? (int?)null : folder.FolderId);
        }

        public Task<List<AssessmentPhotoDto>> GetPhotosByAssessmentAsync(int assessmentId, string mentorUsername = null)
        {
            if (!CanAccessAssessment(assessmentId, mentorUsername))
            {
                return Task.FromResult(new List<AssessmentPhotoDto>());
            }

            return Task.FromResult(_assessmentPhotoRepository.GetPhotosByAssessment(assessmentId)
                .Select(ToDto)
                .Where(photo => photo != null)
                .ToList());
        }

        public Task<AssessmentPhotoDto> AddPhotoAsync(Models.AssessmentPhoto photo, string mentorUsername = null)
        {
            if (photo == null || !CanAccessAssessment(photo.AssessmentId, mentorUsername) ||
                _assessmentPhotoRepository.GetPhotoCount(photo.AssessmentId) >= AssessmentPhotoRules.MaxPhotosPerAssessment)
            {
                return Task.FromResult<AssessmentPhotoDto>(null);
            }

            var folder = GetAssessmentPhotoFolder(photo.AssessmentId, mentorUsername);
            var file = _fileRepository.GetFile(photo.FileId);
            if (folder == null || file == null || file.FolderId != folder.FolderId ||
                file.Size > AssessmentPhotoRules.MaxPhotoBytes || !AssessmentPhotoRules.IsAllowedExtension(file.Extension))
            {
                return Task.FromResult<AssessmentPhotoDto>(null);
            }

            if (_assessmentPhotoRepository.GetPhotosByAssessment(photo.AssessmentId).Any(existing => existing.FileId == photo.FileId))
            {
                return Task.FromResult<AssessmentPhotoDto>(null);
            }

            photo.AssessmentPhotoId = 0;
            photo.Url = file.Url;
            var created = _assessmentPhotoRepository.AddPhoto(photo);
            try
            {
                file.Name = AssessmentPhotoRules.CreateStorageFileName(created.AssessmentId, created.AssessmentPhotoId, file.Extension);
                file = _fileRepository.UpdateFile(file);
                created.Url = file.Url;
                created = _assessmentPhotoRepository.UpdatePhoto(created);
                return Task.FromResult(ToDto(created));
            }
            catch
            {
                _assessmentPhotoRepository.DeletePhoto(created.AssessmentPhotoId);
                DeleteOqtaneFile(file.FileId);
                throw;
            }
        }

        public Task<bool> DeletePhotoAsync(int assessmentPhotoId, string mentorUsername = null)
        {
            var photo = _assessmentPhotoRepository.GetPhoto(assessmentPhotoId);
            if (photo == null || !CanAccessAssessment(photo.AssessmentId, mentorUsername))
            {
                return Task.FromResult(false);
            }

            DeleteOqtaneFile(photo.FileId);
            _assessmentPhotoRepository.DeletePhoto(assessmentPhotoId);
            return Task.FromResult(true);
        }

        private Folder GetAssessmentPhotoFolder(int assessmentId, string mentorUsername)
        {
            if (!CanAccessAssessment(assessmentId, mentorUsername))
            {
                return null;
            }

            var assessment = _assessmentRepository.GetAssessment(assessmentId, tracking: false);
            var grower = assessment == null ? null : _growerRepository.GetGrower(assessment.GrowerId);
            var mentor = string.IsNullOrEmpty(grower?.MentorUsername) ? null : _userRepository.GetUser(grower.MentorUsername);
            if (mentor == null || mentor.SiteId <= 0)
            {
                return null;
            }

            var root = _folderRepository.GetFolder(mentor.SiteId, AssessmentPhotoRules.FolderPath);
            if (root == null)
            {
                var siteRoot = _folderRepository.GetFolder(mentor.SiteId, string.Empty);
                if (siteRoot == null)
                {
                    return null;
                }

                root = _folderRepository.AddFolder(new Folder
                {
                    SiteId = mentor.SiteId,
                    ParentId = siteRoot.FolderId,
                    Name = "AssessmentPhotos",
                    Type = FolderTypes.Private,
                    Path = AssessmentPhotoRules.FolderPath,
                    Order = 1,
                    ImageSizes = string.Empty,
                    Capacity = 0,
                    CacheControl = "no-store",
                    IsSystem = true,
                    PermissionList = AssessmentPhotoFolderPermissions()
                });
            }

            return root;
        }

        private static List<Permission> AssessmentPhotoFolderPermissions()
        {
            return new List<Permission>
            {
                new Permission(PermissionNames.Browse, AppRoleNames.Admin, true),
                new Permission(PermissionNames.View, AppRoleNames.Admin, true),
                new Permission(PermissionNames.Edit, AppRoleNames.Admin, true),
                new Permission(PermissionNames.Browse, AppRoleNames.TenTreesAdmin, true),
                new Permission(PermissionNames.View, AppRoleNames.TenTreesAdmin, true),
                new Permission(PermissionNames.Edit, AppRoleNames.TenTreesAdmin, true),
                new Permission(PermissionNames.Browse, AppRoleNames.Educator, true),
                new Permission(PermissionNames.View, AppRoleNames.Educator, true),
                new Permission(PermissionNames.Browse, AppRoleNames.ProjectManager, true),
                new Permission(PermissionNames.View, AppRoleNames.ProjectManager, true),
                new Permission(PermissionNames.Browse, AppRoleNames.Mentor, true),
                new Permission(PermissionNames.View, AppRoleNames.Mentor, true),
                new Permission(PermissionNames.Edit, AppRoleNames.Mentor, true)
            };
        }

        private AssessmentPhotoDto ToDto(Models.AssessmentPhoto photo)
        {
            var file = _fileRepository.GetFile(photo.FileId);
            if (file == null)
            {
                return null;
            }

            return new AssessmentPhotoDto
            {
                AssessmentPhotoId = photo.AssessmentPhotoId,
                AssessmentId = photo.AssessmentId,
                FileId = photo.FileId,
                FileName = file.Name,
                FileSize = file.Size,
                Url = photo.Url,
                CreatedBy = photo.CreatedBy,
                CreatedOn = photo.CreatedOn
            };
        }

        private void DeleteOqtaneFile(int fileId)
        {
            var file = _fileRepository.GetFile(fileId);
            if (file == null)
            {
                return;
            }

            var path = _fileRepository.GetFilePath(file);
            var directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory) && Directory.Exists(directory))
            {
                foreach (var matchingFile in Directory.GetFiles(directory, Path.GetFileNameWithoutExtension(path) + ".*"))
                {
                    System.IO.File.Delete(matchingFile);
                }
            }

            _fileRepository.DeleteFile(fileId);
        }

        private bool CanAccessAssessment(int assessmentId, string mentorUsername)
        {
            var assessment = _assessmentRepository.GetAssessment(assessmentId, tracking: false);
            if (assessment == null)
            {
                return false;
            }

            if (mentorUsername == null)
            {
                return true;
            }

            var grower = _growerRepository.GetGrower(assessment.GrowerId);
            return grower != null && grower.MentorUsername == mentorUsername;
        }
    }
}
