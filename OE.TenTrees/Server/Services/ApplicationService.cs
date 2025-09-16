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


    public class ServerApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly ILogManager _logger;
        private readonly IHttpContextAccessor _accessor;

        public ServerApplicationService(
            IApplicationRepository applicationRepository, 
            ILogManager logger, 
            IHttpContextAccessor accessor)
        {
            _applicationRepository = applicationRepository;
            _logger = logger;
            _accessor = accessor;
        }

        public Task<List<TreePlantingApplication>> GetApplicationsAsync()
        {
            return Task.FromResult(_applicationRepository.GetApplications().ToList());
        }

        public Task<TreePlantingApplication> GetApplicationAsync(int ApplicationId)
        {
            return Task.FromResult(_applicationRepository.GetApplication(ApplicationId));
        }

        public Task<TreePlantingApplication> AddApplicationAsync(TreePlantingApplication Application)
        {
            Application.ApplicantUserId = _accessor.HttpContext.User.Identity.Name;
            Application = _applicationRepository.AddApplication(Application);
            _logger.Log(LogLevel.Information, this, LogFunction.Create, "Application Added {Application}", Application);
            return Task.FromResult(Application);
        }

        public Task<TreePlantingApplication> UpdateApplicationAsync(TreePlantingApplication Application)
        {
            Application = _applicationRepository.UpdateApplication(Application);
            _logger.Log(LogLevel.Information, this, LogFunction.Update, "Application Updated {Application}", Application);
            return Task.FromResult(Application);
        }

        public Task DeleteApplicationAsync(int ApplicationId)
        {
            _applicationRepository.DeleteApplication(ApplicationId);
            _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Application Deleted {ApplicationId}", ApplicationId);
            return Task.CompletedTask;
        }

        public Task<TreePlantingApplication> ApproveApplicationAsync(int ApplicationId, string Comments = null)
        {
            var application = _applicationRepository.GetApplication(ApplicationId);
            if (application != null)
            {
                application.Status = ApplicationStatus.Approved;
                application.Notes = Comments ?? application.Notes;
                
                // Add status history
                _applicationRepository.AddApplicationStatusHistory(new ApplicationStatusHistory
                {
                    ApplicationId = ApplicationId,
                    Status = ApplicationStatus.Approved,
                    Comment = Comments
                });
                
                application = _applicationRepository.UpdateApplication(application);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "Application Approved {ApplicationId}", ApplicationId);
            }
            return Task.FromResult(application);
        }

        public Task<TreePlantingApplication> RejectApplicationAsync(int ApplicationId, string RejectionReason)
        {
            var application = _applicationRepository.GetApplication(ApplicationId);
            if (application != null)
            {
                application.Status = ApplicationStatus.Rejected;
                application.RejectionReason = RejectionReason;
                
                // Add status history
                _applicationRepository.AddApplicationStatusHistory(new ApplicationStatusHistory
                {
                    ApplicationId = ApplicationId,
                    Status = ApplicationStatus.Rejected,
                    Comment = RejectionReason
                });
                
                application = _applicationRepository.UpdateApplication(application);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "Application Rejected {ApplicationId}", ApplicationId);
            }
            return Task.FromResult(application);
        }

        public Task<ApplicationReview> AddReviewAsync(ApplicationReview Review)
        {
            Review.ReviewerUserId = _accessor.HttpContext.User.Identity.Name;
            Review = _applicationRepository.AddApplicationReview(Review);
            _logger.Log(LogLevel.Information, this, LogFunction.Create, "Application Review Added {Review}", Review);
            return Task.FromResult(Review);
        }

        public Task<List<ApplicationReview>> GetApplicationReviewsAsync(int ApplicationId)
        {
            return Task.FromResult(_applicationRepository.GetApplicationReviews(ApplicationId).ToList());
        }
    }
}