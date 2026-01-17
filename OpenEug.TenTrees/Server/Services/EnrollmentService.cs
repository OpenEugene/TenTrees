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
    }
}
