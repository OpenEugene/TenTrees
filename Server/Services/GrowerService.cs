using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Models;
using Oqtane.Security;
using Oqtane.Shared;
using OpenEug.TenTrees.Module.Grower.Repository;
using OpenEug.TenTrees.Models;
using OpenEug.TenTrees.Shared;

namespace OpenEug.TenTrees.Module.Grower.Services
{
    public class ServerGrowerService : IGrowerService
    {
        private readonly IGrowerRepository _growerRepository;
        private readonly IUserPermissions _userPermissions;
        private readonly ILogManager _logger;
        private readonly IHttpContextAccessor _accessor;
        private readonly Alias _alias;
        private bool _userResolved;
        private bool _isMentor;
        private string _currentUsername;

        public ServerGrowerService(
            IGrowerRepository growerRepository, 
            IUserPermissions userPermissions, 
            ITenantManager tenantManager, 
            ILogManager logger, 
            IHttpContextAccessor accessor)
        {
            _growerRepository = growerRepository;
            _userPermissions = userPermissions;
            _logger = logger;
            _accessor = accessor;
            _alias = tenantManager.GetAlias();
        }

        public Task<Models.Grower> GetGrowerAsync(int growerId)
        {
            var grower = _growerRepository.GetGrower(growerId);
            if (grower != null && IsMentor() && grower.MentorUsername != CurrentUsername())
                return Task.FromResult<Models.Grower>(null);
            return Task.FromResult(grower);
        }

        public Task<List<Models.Grower>> GetAllGrowersAsync(int? villageId = null)
        {
            var growers = _growerRepository.GetAllGrowers(villageId).ToList();
            if (IsMentor())
            {
                growers = growers.Where(g => g.MentorUsername == CurrentUsername()).ToList();
            }
            return Task.FromResult(growers);
        }

        public Task<Models.Grower> ToggleActiveStatusAsync(int growerId, int moduleId)
        {
            // Admin-only operation
            if (!_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.Edit) ||
                !_accessor.HttpContext.User.IsInRole(AppRoleNames.TenTreesAdmin))
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Status Toggle Attempt {GrowerId} {ModuleId}", growerId, moduleId);
                throw new UnauthorizedAccessException($"Unauthorized status toggle attempt for grower {growerId} in module {moduleId}.");
            }

            var grower = _growerRepository.GetGrower(growerId);
            if (grower == null)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Grower Not Found {GrowerId}", growerId);
                return Task.FromResult<Models.Grower>(null);
            }

            // Toggle between Active and Inactive only
            if (grower.Status == GrowerStatus.Active)
            {
                grower.Status = GrowerStatus.Inactive;
            }
            else if (grower.Status == GrowerStatus.Inactive)
            {
                grower.Status = GrowerStatus.Active;
            }
            else
            {
                _logger.Log(LogLevel.Warning, this, LogFunction.Update, "Cannot toggle status for Exited grower {GrowerId}", growerId);
                return Task.FromResult<Models.Grower>(null);
            }

            grower = _growerRepository.UpdateGrower(grower);
            _logger.Log(LogLevel.Information, this, LogFunction.Update, "Grower Status Toggled {GrowerId} to {Status}", growerId, grower.Status);
            
            return Task.FromResult(grower);
        }

        public Task<Models.Grower> RecordProgramExitAsync(int growerId, ProgramExitRequest request, int moduleId)
        {
            // Admin-only operation
            if (!_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.Edit) ||
                !_accessor.HttpContext.User.IsInRole(AppRoleNames.TenTreesAdmin))
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Program Exit Attempt {GrowerId} {ModuleId}", growerId, moduleId);
                return Task.FromResult<Models.Grower>(null);
            }

            var grower = _growerRepository.GetGrower(growerId);
            if (grower == null)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Grower Not Found {GrowerId}", growerId);
                return Task.FromResult<Models.Grower>(null);
            }

            // Validate exit data
            if (string.IsNullOrWhiteSpace(request.ExitReason))
            {
                _logger.Log(LogLevel.Warning, this, LogFunction.Update, "Exit reason required {GrowerId}", growerId);
                return Task.FromResult<Models.Grower>(null);
            }

            // Record formal exit
            grower.Status = GrowerStatus.Exited;
            grower.ExitDate = request.ExitDate;
            grower.ExitReason = request.ExitReason;
            grower.ExitNotes = request.Notes; // Store notes separately

            grower = _growerRepository.UpdateGrower(grower);
            _logger.Log(LogLevel.Information, this, LogFunction.Update, "Program Exit Recorded {GrowerId} Reason: {Reason}", growerId, request.ExitReason);

            return Task.FromResult(grower);
        }

        public Task<List<Models.Grower>> GetActiveGrowersAsync(int? villageId = null)
        {
            var growers = _growerRepository.GetActiveGrowers(villageId).ToList();
            if (IsMentor())
            {
                growers = growers.Where(g => g.MentorUsername == CurrentUsername()).ToList();
            }
            return Task.FromResult(growers);
        }

        public Task<List<Models.Grower>> GetGrowersByStatusAsync(GrowerStatus status, int? villageId = null)
        {
            var growers = _growerRepository.GetGrowersByStatus(status, villageId).ToList();
            if (IsMentor())
            {
                growers = growers.Where(g => g.MentorUsername == CurrentUsername()).ToList();
            }
            return Task.FromResult(growers);
        }

        public Task<GrowerStatusSummary> GetStatusSummaryAsync(int? villageId = null)
        {
            if (IsMentor())
            {
                var growers = _growerRepository.GetGrowersByMentor(CurrentUsername()).ToList();
                return Task.FromResult(new GrowerStatusSummary
                {
                    Active = growers.Count(g => g.Status == GrowerStatus.Active),
                    Inactive = growers.Count(g => g.Status == GrowerStatus.Inactive),
                    Exited = growers.Count(g => g.Status == GrowerStatus.Exited),
                    Total = growers.Count
                });
            }
            var summary = _growerRepository.GetStatusSummary(villageId);
            return Task.FromResult(summary);
        }

        public Task<Models.Grower> AddGrowerAsync(Models.Grower grower, int moduleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.Edit))
            {
                grower.Status = GrowerStatus.Active;
                grower = _growerRepository.AddGrower(grower);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "Grower Added {Grower}", grower);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Grower Add Attempt {Grower}", grower);
                grower = null;
            }
            return Task.FromResult(grower);
        }

        public Task<Models.Grower> UpdateGrowerAsync(Models.Grower grower, int moduleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.Edit))
            {
                grower = _growerRepository.UpdateGrower(grower);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "Grower Updated {Grower}", grower);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Grower Update Attempt {Grower}", grower);
                grower = null;
            }
            return Task.FromResult(grower);
        }

        public Task DeleteGrowerAsync(int growerId, int moduleId)
        {
            _growerRepository.DeleteGrower(growerId);
            return Task.CompletedTask;
        }

        private bool IsMentor()
        {
            ResolveUser();
            return _isMentor;
        }

        private string CurrentUsername()
        {
            ResolveUser();
            return _currentUsername;
        }

        private void ResolveUser()
        {
            if (_userResolved) return;
            var user = _accessor.HttpContext?.User;
            _isMentor = user?.IsInRole(AppRoleNames.Mentor) ?? false;
            _currentUsername = user?.Identity?.Name;
            _userResolved = true;
        }
    }
}
