using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Models;
using Oqtane.Security;
using Oqtane.Shared;
using OpenEug.TenTrees.Module.Training.Repository;
using OpenEug.TenTrees.Models;

namespace OpenEug.TenTrees.Module.Training.Services
{
    public class ServerTrainingService : ITrainingService
    {
        private readonly ITrainingRepository _trainingRepository;
        private readonly IUserPermissions _userPermissions;
        private readonly ILogManager _logger;
        private readonly IHttpContextAccessor _accessor;
        private readonly Alias _alias;

        public ServerTrainingService(
            ITrainingRepository trainingRepository,
            IUserPermissions userPermissions,
            ITenantManager tenantManager,
            ILogManager logger,
            IHttpContextAccessor accessor)
        {
            _trainingRepository = trainingRepository;
            _userPermissions = userPermissions;
            _logger = logger;
            _accessor = accessor;
            _alias = tenantManager.GetAlias();
        }

        public Task<List<TrainingClass>> GetTrainingClassesAsync(int moduleId, int? villageId = null)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.View))
            {
                return Task.FromResult(_trainingRepository.GetTrainingClasses(moduleId, villageId).ToList());
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Training Classes Get Attempt {ModuleId}", moduleId);
                return Task.FromResult<List<TrainingClass>>(null);
            }
        }

        public Task<TrainingClass> GetTrainingClassAsync(int classId, int moduleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.View))
            {
                return Task.FromResult(_trainingRepository.GetTrainingClass(classId));
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Training Class Get Attempt {ClassId} {ModuleId}", classId, moduleId);
                return Task.FromResult<TrainingClass>(null);
            }
        }

        public Task<TrainingClass> AddTrainingClassAsync(TrainingClass trainingClass)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, trainingClass.ModuleId, PermissionNames.Edit))
            {
                trainingClass = _trainingRepository.AddTrainingClass(trainingClass);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "Training Class Added {TrainingClass}", trainingClass);
                return Task.FromResult(trainingClass);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Training Class Add Attempt {TrainingClass}", trainingClass);
                return Task.FromResult<TrainingClass>(null);
            }
        }

        public Task<TrainingClass> UpdateTrainingClassAsync(TrainingClass trainingClass)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, trainingClass.ModuleId, PermissionNames.Edit))
            {
                trainingClass = _trainingRepository.UpdateTrainingClass(trainingClass);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "Training Class Updated {TrainingClass}", trainingClass);
                return Task.FromResult(trainingClass);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Training Class Update Attempt {TrainingClass}", trainingClass);
                return Task.FromResult<TrainingClass>(null);
            }
        }

        public Task DeleteTrainingClassAsync(int classId, int moduleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.Edit))
            {
                _trainingRepository.DeleteTrainingClass(classId);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Training Class Deleted {ClassId}", classId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Training Class Delete Attempt {ClassId} {ModuleId}", classId, moduleId);
            }
            return Task.CompletedTask;
        }

        public Task<List<ClassAttendance>> GetAttendanceForClassAsync(int classId, int moduleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.View))
            {
                return Task.FromResult(_trainingRepository.GetAttendanceForClass(classId).ToList());
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Attendance Get Attempt {ClassId} {ModuleId}", classId, moduleId);
                return Task.FromResult<List<ClassAttendance>>(null);
            }
        }

        public Task MarkAttendanceAsync(MarkAttendanceRequest request)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, request.ModuleId, PermissionNames.Edit))
            {
                var records = request.Entries.Select(e => new ClassAttendance
                {
                    TrainingClassId = request.TrainingClassId,
                    GrowerId = e.GrowerId,
                    IsPresent = e.IsPresent
                }).ToList();

                _trainingRepository.MarkAttendance(records, request.TrainingClassId);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "Attendance Marked for Class {ClassId} with {Count} entries", request.TrainingClassId, request.Entries.Count);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Attendance Mark Attempt {ClassId} {ModuleId}", request.TrainingClassId, request.ModuleId);
            }
            return Task.CompletedTask;
        }

        public Task<List<AttendanceSummaryViewModel>> GetAttendanceSummariesAsync(int moduleId, int? villageId = null)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.View))
            {
                return Task.FromResult(_trainingRepository.GetAttendanceSummaries(moduleId, villageId).ToList());
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Attendance Summaries Get Attempt {ModuleId}", moduleId);
                return Task.FromResult<List<AttendanceSummaryViewModel>>(null);
            }
        }

        public Task<AttendanceSummaryViewModel> GetGrowerAttendanceSummaryAsync(int growerId, int moduleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.View))
            {
                return Task.FromResult(_trainingRepository.GetGrowerAttendanceSummary(growerId, moduleId));
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Grower Attendance Summary Get Attempt {GrowerId} {ModuleId}", growerId, moduleId);
                return Task.FromResult<AttendanceSummaryViewModel>(null);
            }
        }

        public Task<TrainingStatusSummary> GetTrainingStatusSummaryAsync(int moduleId, int? villageId = null)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.View))
            {
                return Task.FromResult(_trainingRepository.GetTrainingStatusSummary(moduleId, villageId));
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Training Status Summary Get Attempt {ModuleId}", moduleId);
                return Task.FromResult<TrainingStatusSummary>(null);
            }
        }
    }
}
