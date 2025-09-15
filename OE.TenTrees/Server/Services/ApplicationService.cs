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
using OE.TenTrees.Repository;
using OE.TenTrees.Models;

namespace OE.TenTrees.Services
{
    public interface IApplicationService
    {
        Task<List<TreePlantingApplication>> GetApplicationsAsync(int ModuleId);
        Task<TreePlantingApplication> GetApplicationAsync(int ApplicationId, int ModuleId);
        Task<TreePlantingApplication> AddApplicationAsync(TreePlantingApplication Application);
        Task<TreePlantingApplication> UpdateApplicationAsync(TreePlantingApplication Application);
        Task DeleteApplicationAsync(int ApplicationId, int ModuleId);
        Task<TreePlantingApplication> ApproveApplicationAsync(int ApplicationId, int ModuleId, string Comments = null);
        Task<TreePlantingApplication> RejectApplicationAsync(int ApplicationId, int ModuleId, string RejectionReason);
        Task<ApplicationReview> AddReviewAsync(ApplicationReview Review);
        Task<List<ApplicationReview>> GetApplicationReviewsAsync(int ApplicationId);
    }

    public class ServerApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IUserPermissions _userPermissions;
        private readonly ILogManager _logger;
        private readonly IHttpContextAccessor _accessor;
        private readonly Alias _alias;

        public ServerApplicationService(
            IApplicationRepository applicationRepository, 
            IUserPermissions userPermissions, 
            ITenantManager tenantManager, 
            ILogManager logger, 
            IHttpContextAccessor accessor)
        {
            _applicationRepository = applicationRepository;
            _userPermissions = userPermissions;
            _logger = logger;
            _accessor = accessor;
            _alias = tenantManager.GetAlias();
        }

        public Task<List<TreePlantingApplication>> GetApplicationsAsync(int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.View))
            {
                return Task.FromResult(_applicationRepository.GetApplications(ModuleId).ToList());
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Application Get Attempt {ModuleId}", ModuleId);
                return Task.FromResult(new List<TreePlantingApplication>());
            }
        }

        public Task<TreePlantingApplication> GetApplicationAsync(int ApplicationId, int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.View))
            {
                return Task.FromResult(_applicationRepository.GetApplication(ApplicationId));
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Application Get Attempt {ApplicationId} {ModuleId}", ApplicationId, ModuleId);
                return Task.FromResult<TreePlantingApplication>(null);
            }
        }

        public Task<TreePlantingApplication> AddApplicationAsync(TreePlantingApplication Application)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, Application.ModuleId, PermissionNames.Edit))
            {
                Application = _applicationRepository.AddApplication(Application);
                
                // Add initial status history entry
                var statusHistory = new ApplicationStatusHistory
                {
                    ApplicationId = Application.ApplicationId,
                    Status = Application.Status,
                    ChangedByUserId = _accessor.HttpContext.User.Identity.Name,
                    Comment = "Application created",
                    CreatedBy = _accessor.HttpContext.User.Identity.Name,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = _accessor.HttpContext.User.Identity.Name,
                    ModifiedOn = DateTime.UtcNow
                };
                _applicationRepository.AddApplicationStatusHistory(statusHistory);
                
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "Application Added {Application}", Application);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Application Add Attempt {Application}", Application);
                Application = null;
            }
            return Task.FromResult(Application);
        }

        public Task<TreePlantingApplication> UpdateApplicationAsync(TreePlantingApplication Application)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, Application.ModuleId, PermissionNames.Edit))
            {
                var existingApplication = _applicationRepository.GetApplication(Application.ApplicationId);
                if (existingApplication != null && existingApplication.Status != Application.Status)
                {
                    // Status changed, add history entry
                    var statusHistory = new ApplicationStatusHistory
                    {
                        ApplicationId = Application.ApplicationId,
                        Status = Application.Status,
                        ChangedByUserId = _accessor.HttpContext.User.Identity.Name,
                        Comment = $"Status changed from {existingApplication.Status} to {Application.Status}",
                        CreatedBy = _accessor.HttpContext.User.Identity.Name,
                        CreatedOn = DateTime.UtcNow,
                        ModifiedBy = _accessor.HttpContext.User.Identity.Name,
                        ModifiedOn = DateTime.UtcNow
                    };
                    _applicationRepository.AddApplicationStatusHistory(statusHistory);
                }
                
                Application = _applicationRepository.UpdateApplication(Application);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "Application Updated {Application}", Application);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Application Update Attempt {Application}", Application);
                Application = null;
            }
            return Task.FromResult(Application);
        }

        public Task DeleteApplicationAsync(int ApplicationId, int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.Edit))
            {
                _applicationRepository.DeleteApplication(ApplicationId);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Application Deleted {ApplicationId}", ApplicationId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Application Delete Attempt {ApplicationId} {ModuleId}", ApplicationId, ModuleId);
            }
            return Task.CompletedTask;
        }

        public Task<TreePlantingApplication> ApproveApplicationAsync(int ApplicationId, int ModuleId, string Comments = null)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.Edit))
            {
                var application = _applicationRepository.GetApplication(ApplicationId);
                if (application != null)
                {
                    application.Status = ApplicationStatus.Approved;
                    application.ModifiedBy = _accessor.HttpContext.User.Identity.Name;
                    application.ModifiedOn = DateTime.UtcNow;
                    
                    application = _applicationRepository.UpdateApplication(application);
                    
                    // Add status history
                    var statusHistory = new ApplicationStatusHistory
                    {
                        ApplicationId = ApplicationId,
                        Status = ApplicationStatus.Approved,
                        ChangedByUserId = _accessor.HttpContext.User.Identity.Name,
                        Comment = Comments ?? "Application approved",
                        CreatedBy = _accessor.HttpContext.User.Identity.Name,
                        CreatedOn = DateTime.UtcNow,
                        ModifiedBy = _accessor.HttpContext.User.Identity.Name,
                        ModifiedOn = DateTime.UtcNow
                    };
                    _applicationRepository.AddApplicationStatusHistory(statusHistory);
                    
                    _logger.Log(LogLevel.Information, this, LogFunction.Update, "Application Approved {ApplicationId}", ApplicationId);
                    return Task.FromResult(application);
                }
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Application Approve Attempt {ApplicationId} {ModuleId}", ApplicationId, ModuleId);
            }
            return Task.FromResult<TreePlantingApplication>(null);
        }

        public Task<TreePlantingApplication> RejectApplicationAsync(int ApplicationId, int ModuleId, string RejectionReason)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.Edit))
            {
                var application = _applicationRepository.GetApplication(ApplicationId);
                if (application != null)
                {
                    application.Status = ApplicationStatus.Rejected;
                    application.RejectionReason = RejectionReason;
                    application.ModifiedBy = _accessor.HttpContext.User.Identity.Name;
                    application.ModifiedOn = DateTime.UtcNow;
                    
                    application = _applicationRepository.UpdateApplication(application);
                    
                    // Add status history
                    var statusHistory = new ApplicationStatusHistory
                    {
                        ApplicationId = ApplicationId,
                        Status = ApplicationStatus.Rejected,
                        ChangedByUserId = _accessor.HttpContext.User.Identity.Name,
                        Comment = $"Application rejected: {RejectionReason}",
                        CreatedBy = _accessor.HttpContext.User.Identity.Name,
                        CreatedOn = DateTime.UtcNow,
                        ModifiedBy = _accessor.HttpContext.User.Identity.Name,
                        ModifiedOn = DateTime.UtcNow
                    };
                    _applicationRepository.AddApplicationStatusHistory(statusHistory);
                    
                    _logger.Log(LogLevel.Information, this, LogFunction.Update, "Application Rejected {ApplicationId}", ApplicationId);
                    return Task.FromResult(application);
                }
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Application Reject Attempt {ApplicationId} {ModuleId}", ApplicationId, ModuleId);
            }
            return Task.FromResult<TreePlantingApplication>(null);
        }

        public Task<ApplicationReview> AddReviewAsync(ApplicationReview Review)
        {
            // For reviews, we'll check authorization against the application's module
            var application = _applicationRepository.GetApplication(Review.ApplicationId);
            if (application != null && _userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, application.ModuleId, PermissionNames.Edit))
            {
                Review = _applicationRepository.AddApplicationReview(Review);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "Application Review Added {Review}", Review);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Application Review Add Attempt {Review}", Review);
                Review = null;
            }
            return Task.FromResult(Review);
        }

        public Task<List<ApplicationReview>> GetApplicationReviewsAsync(int ApplicationId)
        {
            var application = _applicationRepository.GetApplication(ApplicationId);
            if (application != null && _userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, application.ModuleId, PermissionNames.View))
            {
                return Task.FromResult(_applicationRepository.GetApplicationReviews(ApplicationId).ToList());
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Application Reviews Get Attempt {ApplicationId}", ApplicationId);
                return Task.FromResult(new List<ApplicationReview>());
            }
        }
    }
}