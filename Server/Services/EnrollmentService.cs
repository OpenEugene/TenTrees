using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Models;
using Oqtane.Repository;
using Oqtane.Shared;
using OpenEug.TenTrees.Module.Enrollment.Repository;
using OpenEug.TenTrees.Models;

namespace OpenEug.TenTrees.Module.Enrollment.Services
{
    public class ServerEnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly ILogManager _logger;
        private readonly Alias _alias;

        public ServerEnrollmentService(IEnrollmentRepository enrollmentRepository, IUserRoleRepository userRoleRepository, ITenantManager tenantManager, ILogManager logger)
        {
            _enrollmentRepository = enrollmentRepository;
            _userRoleRepository = userRoleRepository;
            _logger = logger;
            _alias = tenantManager.GetAlias();
        }

        public Task<List<Models.Enrollment>> GetEnrollmentsAsync()
        {
            var enrollments = _enrollmentRepository.GetEnrollments().ToList();
            return Task.FromResult(enrollments);
        }

        public Task<List<EnrollmentListViewModel>> GetEnrollmentListViewModelsAsync()
        {
            var viewModels = _enrollmentRepository.GetEnrollmentListViewModels();
            return Task.FromResult(viewModels.ToList());
        }

        public Task<Models.Enrollment> GetEnrollmentAsync(int enrollmentId)
        {
            var enrollment = _enrollmentRepository.GetEnrollment(enrollmentId);
            return Task.FromResult(enrollment);
        }

        public Task<Models.Enrollment> AddEnrollmentAsync(Models.Enrollment enrollment)
        {
            enrollment = _enrollmentRepository.AddEnrollment(enrollment);
            _logger.Log(LogLevel.Information, this, LogFunction.Create, "Enrollment Added {Enrollment}", enrollment);
            return Task.FromResult(enrollment);
        }

        public Task<Models.Enrollment> UpdateEnrollmentAsync(Models.Enrollment enrollment)
        {
            enrollment = _enrollmentRepository.UpdateEnrollment(enrollment);
            _logger.Log(LogLevel.Information, this, LogFunction.Update, "Enrollment Updated {Enrollment}", enrollment);
            return Task.FromResult(enrollment);
        }

        public Task DeleteEnrollmentAsync(int enrollmentId, int moduleId)
        {
            _enrollmentRepository.DeleteEnrollment(enrollmentId);
            _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Enrollment Deleted {EnrollmentId}", enrollmentId);
            return Task.CompletedTask;
        }
        
        public Task<ValidationResult> ValidateRequiredAsync(Models.Enrollment Enrollment)
        {
            var result = new ValidationResult { IsValid = true, Errors = new List<string>() };
            
            if (string.IsNullOrWhiteSpace(Enrollment.GrowerName))
            {
                result.IsValid = false;
                result.Errors.Add("Grower name is required");
            }
            
            if (Enrollment.VillageId == 0)
            {
                result.IsValid = false;
                result.Errors.Add("Village is required");
            }
            
            if (Enrollment.HouseholdSize <= 0)
            {
                result.IsValid = false;
                result.Errors.Add("Household size must be greater than 0");
            }
            
            return Task.FromResult(result);
        }
        
        public Task<bool> CaptureSignatureAsync(int EnrollmentId, string SignatureData)
        {
            try
            {
                var enrollment = _enrollmentRepository.GetEnrollment(EnrollmentId);
                if (enrollment != null)
                {
                    enrollment.SignatureData = SignatureData;
                    enrollment.SignatureCollected = true;
                    enrollment.SignatureDate = System.DateTime.UtcNow;
                    _enrollmentRepository.UpdateEnrollment(enrollment);
                    return Task.FromResult(true);
                }
                return Task.FromResult(false);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        public Task<bool> CapturePhotoConsentAsync(int enrollmentId, int moduleId, Models.PhotoConsentLevel consentLevel, string signatureData)
        {
            try
            {
                var enrollment = _enrollmentRepository.GetEnrollment(enrollmentId);
                if (enrollment != null)
                {
                    enrollment.PhotoConsentLevel = consentLevel;
                    enrollment.PhotoConsentSignatureData = signatureData;
                    enrollment.PhotoConsentSignatureCollected = true;
                    enrollment.PhotoConsentSignatureDate = System.DateTime.UtcNow;
                    _enrollmentRepository.UpdateEnrollment(enrollment);
                    return Task.FromResult(true);
                }
                return Task.FromResult(false);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }
        
        public Task<MentorInfo> AutoFillMentorAsync(int UserId)
        {
            var mentorInfo = new MentorInfo
            {
                MentorUsername = UserId.ToString(),
                MentorName = "Mentor Name",
                VillageId = 1
            };
            return Task.FromResult(mentorInfo);
        }
        
        public Task<List<Models.Enrollment>> GetByStatusAsync(Models.EnrollmentStatus Status)
        {
            var enrollments = _enrollmentRepository.GetEnrollments()
                .Where(e => e.Status == Status)
                .ToList();
            return Task.FromResult(enrollments);
        }
        
        public Task<List<Models.Enrollment>> GetByVillageAsync(int VillageId)
        {
            var enrollments = _enrollmentRepository.GetEnrollments()
                .Where(e => e.VillageId == VillageId)
                .ToList();
            return Task.FromResult(enrollments);
        }

        public Task<int> BackfillGrowersFromEnrollmentsAsync(int moduleId)
        {
            int created = _enrollmentRepository.BackfillGrowersFromEnrollments();
            _logger.Log(LogLevel.Information, this, LogFunction.Create, "Backfilled {Count} Growers from Enrollments", created);
            return Task.FromResult(created);
        }

        public Task<List<UserInfo>> GetSiteUsersAsync()
        {
            var userRoles = _userRoleRepository.GetUserRoles(_alias.SiteId)
                .Where(ur => ur.User != null && !ur.User.IsDeleted)
                .GroupBy(ur => ur.UserId)
                .Select(g => g.First().User)
                .Select(u => new UserInfo { Username = u.Username, DisplayName = u.DisplayName })
                .OrderBy(u => u.DisplayName)
                .ToList();
            return Task.FromResult(userRoles);
        }
    }
}
