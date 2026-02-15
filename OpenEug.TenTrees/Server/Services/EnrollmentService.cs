using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Models;
using Oqtane.Security;
using Oqtane.Shared;
using OpenEug.TenTrees.Module.Enrollment.Repository;
using OpenEug.TenTrees.Models;

namespace OpenEug.TenTrees.Module.Enrollment.Services
{
    public class ServerEnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IUserPermissions _userPermissions;
        private readonly ILogManager _logger;
        private readonly IHttpContextAccessor _accessor;
        private readonly Alias _alias;

        public ServerEnrollmentService(IEnrollmentRepository enrollmentRepository, IUserPermissions userPermissions, ITenantManager tenantManager, ILogManager logger, IHttpContextAccessor accessor)
        {
            _enrollmentRepository = enrollmentRepository;
            _userPermissions = userPermissions;
            _logger = logger;
            _accessor = accessor;
            _alias = tenantManager.GetAlias();
        }

        public Task<List<Models.Enrollment>> GetEnrollmentsAsync(int moduleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.View))
            {
                return Task.FromResult(_enrollmentRepository.GetEnrollments(moduleId).ToList());
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Enrollment Get Attempt {ModuleId}", moduleId);
                return null;
            }
        }

        public Task<List<EnrollmentListViewModel>> GetEnrollmentListViewModelsAsync(int moduleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.View))
            {
                return Task.FromResult(_enrollmentRepository.GetEnrollmentListViewModels(moduleId).ToList());
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Enrollment List ViewModel Get Attempt {ModuleId}", moduleId);
                return null;
            }
        }

        public Task<Models.Enrollment> GetEnrollmentAsync(int enrollmentId, int moduleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.View))
            {
                return Task.FromResult(_enrollmentRepository.GetEnrollment(enrollmentId));
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Enrollment Get Attempt {EnrollmentId} {ModuleId}", enrollmentId, moduleId);
                return null;
            }
        }

        public Task<Models.Enrollment> AddEnrollmentAsync(Models.Enrollment enrollment)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, enrollment.ModuleId, PermissionNames.Edit))
            {
                enrollment = _enrollmentRepository.AddEnrollment(enrollment);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "Enrollment Added {Enrollment}", enrollment);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Enrollment Add Attempt {Enrollment}", enrollment);
                enrollment = null;
            }
            return Task.FromResult(enrollment);
        }

        public Task<Models.Enrollment> UpdateEnrollmentAsync(Models.Enrollment enrollment)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, enrollment.ModuleId, PermissionNames.Edit))
            {
                enrollment = _enrollmentRepository.UpdateEnrollment(enrollment);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "Enrollment Updated {Enrollment}", enrollment);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Enrollment Update Attempt {Enrollment}", enrollment);
                enrollment = null;
            }
            return Task.FromResult(enrollment);
        }

        public Task DeleteEnrollmentAsync(int enrollmentId, int moduleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.Edit))
            {
                _enrollmentRepository.DeleteEnrollment(enrollmentId);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Enrollment Deleted {EnrollmentId}", enrollmentId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Enrollment Delete Attempt {EnrollmentId} {ModuleId}", enrollmentId, moduleId);
            }
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
        
        public Task<MentorInfo> AutoFillMentorAsync(int UserId)
        {
            // This would need to query the user repository and get mentor information
            // For now, returning a placeholder implementation
            var mentorInfo = new MentorInfo
            {
                MentorId = UserId,
                TreeMentorName = "Mentor Name", // Would come from user profile
                VillageId = 1 // Would come from user's village assignment
            };
            return Task.FromResult(mentorInfo);
        }
        
        public Task<List<Models.Enrollment>> GetByStatusAsync(int ModuleId, Models.EnrollmentStatus Status)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.View))
            {
                var enrollments = _enrollmentRepository.GetEnrollments(ModuleId)
                    .Where(e => e.Status == Status)
                    .ToList();
                return Task.FromResult(enrollments);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Enrollment Get By Status Attempt {ModuleId}", ModuleId);
                return null;
            }
        }
        
        public Task<List<Models.Enrollment>> GetByVillageAsync(int VillageId)
        {
            var enrollments = _enrollmentRepository.GetEnrollments()
                .Where(e => e.VillageId == VillageId)
                .ToList();
            return Task.FromResult(enrollments);
        }
    }
}
