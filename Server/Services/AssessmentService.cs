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
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
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
        private readonly IAssessmentPhotoFolderService _photoFolders;
        private readonly ITenantManager _tenantManager;
        private readonly ILogManager _logger;

        public ServerAssessmentService(
            IAssessmentRepository assessmentRepository,
            IAssessmentNoteRepository assessmentNoteRepository,
            IAssessmentPhotoRepository assessmentPhotoRepository,
            IGrowerRepository growerRepository,
            IFolderRepository folderRepository,
            IFileRepository fileRepository,
            IAssessmentPhotoFolderService photoFolders,
            ITenantManager tenantManager,
            ILogManager logger)
        {
            _assessmentRepository = assessmentRepository;
            _assessmentNoteRepository = assessmentNoteRepository;
            _assessmentPhotoRepository = assessmentPhotoRepository;
            _growerRepository = growerRepository;
            _folderRepository = folderRepository;
            _fileRepository = fileRepository;
            _photoFolders = photoFolders;
            _tenantManager = tenantManager;
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

        public Task<int?> GetPhotoFolderIdByGrowerAsync(int growerId, string mentorUsername = null)
        {
            var grower = _growerRepository.GetGrower(growerId);
            if (grower == null || (mentorUsername != null && grower.MentorUsername != mentorUsername))
            {
                return Task.FromResult<int?>(null);
            }

            var folder = ResolveGrowerFolder(grower);
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
                file.Size > AssessmentPhotoRules.MaxUploadBytes || !AssessmentPhotoRules.IsAllowedExtension(file.Extension))
            {
                return Task.FromResult<AssessmentPhotoDto>(null);
            }

            if (_assessmentPhotoRepository.GetPhotosByAssessment(photo.AssessmentId).Any(existing => existing.FileId == photo.FileId))
            {
                return Task.FromResult<AssessmentPhotoDto>(null);
            }

            // Rename the upload to a human-readable name such as 2026-Sep_A.jpg, unique within the
            // grower's folder. Oqtane's IFileRepository.UpdateFile only touches the database row, so
            // the physical move has to happen here (mirrors FileController.Put).
            var folderPath = _folderRepository.GetFolderPath(folder);
            var sourcePath = _fileRepository.GetFilePath(file);
            if (!System.IO.File.Exists(sourcePath))
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Create, "Uploaded assessment photo missing on disk {FileId} {Path}", file.FileId, sourcePath);
                DeleteOqtaneFile(file.FileId);
                return Task.FromResult<AssessmentPhotoDto>(null);
            }

            // Field users cannot shrink photos themselves, so accept the camera original and reduce
            // it here: fix EXIF rotation, cap the long edge, re-encode. Done before the rename so a
            // failure leaves nothing half-linked.
            try
            {
                var (width, height) = NormalizePhoto(sourcePath, file.Extension);
                file.ImageWidth = width;
                file.ImageHeight = height;
                file.Size = (int)new FileInfo(sourcePath).Length;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Create, "Uploaded assessment photo could not be processed as an image {FileId} {Name} {Error}", file.FileId, file.Name, ex.ToString());
                DeleteOqtaneFile(file.FileId);
                return Task.FromResult<AssessmentPhotoDto>(null);
            }

            if (file.Size > AssessmentPhotoRules.MaxPhotoBytes)
            {
                _logger.Log(LogLevel.Warning, this, LogFunction.Create, "Assessment photo still exceeds the stored size cap after resizing {FileId} {Size}", file.FileId, file.Size);
                DeleteOqtaneFile(file.FileId);
                return Task.FromResult<AssessmentPhotoDto>(null);
            }

            var assessment = _assessmentRepository.GetAssessment(photo.AssessmentId, tracking: false);
            var photoDate = assessment?.AssessmentDate ?? DateTime.UtcNow;
            var storageName = AssessmentPhotoRules.NextStorageFileName(photoDate, file.Extension, candidate =>
                string.Equals(candidate, file.Name, StringComparison.OrdinalIgnoreCase) ? false :
                _fileRepository.GetFile(folder.FolderId, candidate) != null || System.IO.File.Exists(Path.Combine(folderPath, candidate)));

            var targetPath = Path.Combine(folderPath, storageName);
            if (!string.Equals(sourcePath, targetPath, StringComparison.OrdinalIgnoreCase))
            {
                Directory.CreateDirectory(folderPath);
                System.IO.File.Move(sourcePath, targetPath);
                file.Name = storageName;
            }

            file = _fileRepository.UpdateFile(file);

            photo.AssessmentPhotoId = 0;
            // The app addresses photos by Oqtane FileId, never by name, so a later rename or move
            // of the file cannot break the stored link.
            photo.Url = Utilities.FileUrl(_tenantManager.GetAlias(), file.FileId);
            try
            {
                var created = _assessmentPhotoRepository.AddPhoto(photo);
                return Task.FromResult(ToDto(created));
            }
            catch
            {
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
            if (grower == null)
            {
                return null;
            }

            return ResolveGrowerFolder(grower);
        }

        /// <summary>
        /// Folder lives on the active site and is scoped per grower; the folder service re-syncs the
        /// assigned mentor's user-level permission every time it is resolved. Resolving is also when
        /// abandoned uploads (files with no association row) get cleaned out of that folder.
        /// </summary>
        private Folder ResolveGrowerFolder(Models.Grower grower)
        {
            var folder = _photoFolders.GetOrCreateGrowerFolder(grower);
            if (folder != null)
            {
                SweepOrphanedFiles(folder);
            }

            return folder;
        }

        /// <summary>
        /// Photos can be uploaded before the assessment is saved, so a file may legitimately have no
        /// association row for a while. Anything still unlinked after a day was abandoned.
        /// </summary>
        private void SweepOrphanedFiles(Folder folder)
        {
            try
            {
                var cutoff = DateTime.UtcNow.AddDays(-1);
                var files = _fileRepository.GetFiles(folder.FolderId, false).ToList();
                var inUse = _assessmentPhotoRepository.GetFileIdsInUse(files.Select(f => f.FileId));
                foreach (var file in files.Where(f => !inUse.Contains(f.FileId) && f.CreatedOn < cutoff))
                {
                    DeleteOqtaneFile(file.FileId);
                    _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Removed abandoned assessment photo {FileId} {Name} from {Path}", file.FileId, file.Name, folder.Path);
                }
            }
            catch (Exception ex)
            {
                // Never let housekeeping block the folder lookup the user is waiting on.
                _logger.Log(LogLevel.Warning, this, LogFunction.Delete, "Assessment photo orphan sweep failed for {Path} {Error}", folder.Path, ex.ToString());
            }
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

        /// <summary>
        /// Rewrites the image at <paramref name="path"/> in place: applies EXIF orientation, shrinks it so
        /// neither side exceeds <see cref="AssessmentPhotoRules.MaxLongEdgePixels"/>, and re-encodes in the
        /// same format. Mirrors what Oqtane's own ImageService does for thumbnails. Returns final dimensions.
        /// </summary>
        private static (int Width, int Height) NormalizePhoto(string path, string extension)
        {
            using var image = Image.Load(path);

            var max = AssessmentPhotoRules.MaxLongEdgePixels;
            image.Mutate(context =>
            {
                context.AutoOrient();
                if (image.Width > max || image.Height > max)
                {
                    context.Resize(new ResizeOptions { Mode = ResizeMode.Max, Size = new Size(max, max) });
                }
            });

            IImageEncoder encoder = (extension ?? string.Empty).TrimStart('.').ToLowerInvariant() switch
            {
                "png" => new PngEncoder(),
                "webp" => new WebpEncoder { Quality = AssessmentPhotoRules.ResizedQuality },
                _ => new JpegEncoder { Quality = AssessmentPhotoRules.ResizedQuality }
            };

            image.Save(path, encoder);
            return (image.Width, image.Height);
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
