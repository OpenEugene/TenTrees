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

namespace OpenEug.TenTrees.Module.Enrollment.Services
{
    public class ServerEnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _EnrollmentRepository;
        private readonly IUserPermissions _userPermissions;
        private readonly ILogManager _logger;
        private readonly IHttpContextAccessor _accessor;
        private readonly Alias _alias;

        public ServerEnrollmentService(IEnrollmentRepository EnrollmentRepository, IUserPermissions userPermissions, ITenantManager tenantManager, ILogManager logger, IHttpContextAccessor accessor)
        {
            _EnrollmentRepository = EnrollmentRepository;
            _userPermissions = userPermissions;
            _logger = logger;
            _accessor = accessor;
            _alias = tenantManager.GetAlias();
        }

        public Task<List<Models.Enrollment>> GetEnrollmentsAsync(int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.View))
            {
                return Task.FromResult(_EnrollmentRepository.GetEnrollments(ModuleId).ToList());
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Enrollment Get Attempt {ModuleId}", ModuleId);
                return null;
            }
        }

        public Task<Models.Enrollment> GetEnrollmentAsync(int EnrollmentId, int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.View))
            {
                return Task.FromResult(_EnrollmentRepository.GetEnrollment(EnrollmentId));
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Enrollment Get Attempt {EnrollmentId} {ModuleId}", EnrollmentId, ModuleId);
                return null;
            }
        }

        public Task<Models.Enrollment> AddEnrollmentAsync(Models.Enrollment Enrollment)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, Enrollment.ModuleId, PermissionNames.Edit))
            {
                Enrollment = _EnrollmentRepository.AddEnrollment(Enrollment);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "Enrollment Added {Enrollment}", Enrollment);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Enrollment Add Attempt {Enrollment}", Enrollment);
                Enrollment = null;
            }
            return Task.FromResult(Enrollment);
        }

        public Task<Models.Enrollment> UpdateEnrollmentAsync(Models.Enrollment Enrollment)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, Enrollment.ModuleId, PermissionNames.Edit))
            {
                Enrollment = _EnrollmentRepository.UpdateEnrollment(Enrollment);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "Enrollment Updated {Enrollment}", Enrollment);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Enrollment Update Attempt {Enrollment}", Enrollment);
                Enrollment = null;
            }
            return Task.FromResult(Enrollment);
        }

        public Task DeleteEnrollmentAsync(int EnrollmentId, int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.Edit))
            {
                _EnrollmentRepository.DeleteEnrollment(EnrollmentId);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Enrollment Deleted {EnrollmentId}", EnrollmentId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Enrollment Delete Attempt {EnrollmentId} {ModuleId}", EnrollmentId, ModuleId);
            }
            return Task.CompletedTask;
        }
        
        public Task<ValidationResult> ValidateRequiredAsync(Models.Enrollment Enrollment)
        {
            var result = new ValidationResult { IsValid = true, Errors = new List<string>() };
            
            if (string.IsNullOrWhiteSpace(Enrollment.BeneficiaryName))
            {
                result.IsValid = false;
                result.Errors.Add("Beneficiary name is required");
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
                var enrollment = _EnrollmentRepository.GetEnrollment(EnrollmentId);
                if (enrollment != null)
                {
                    enrollment.SignatureData = SignatureData;
                    enrollment.SignatureCollected = true;
                    enrollment.SignatureDate = System.DateTime.UtcNow;
                    _EnrollmentRepository.UpdateEnrollment(enrollment);
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
                EvaluatorName = "Mentor Name", // Would come from user profile
                VillageId = 1 // Would come from user's village assignment
            };
            return Task.FromResult(mentorInfo);
        }
        
        public Task<List<Models.Enrollment>> GetByStatusAsync(int ModuleId, Models.EnrollmentStatus Status)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.View))
            {
                var enrollments = _EnrollmentRepository.GetEnrollments(ModuleId)
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
            var enrollments = _EnrollmentRepository.GetEnrollments()
                .Where(e => e.VillageId == VillageId)
                .ToList();
            return Task.FromResult(enrollments);
        }
    }
}
