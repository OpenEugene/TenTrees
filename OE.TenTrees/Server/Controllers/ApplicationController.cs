using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using OE.TenTrees.Services;
using Oqtane.Controllers;
using System.Net;
using System.Threading.Tasks;
using OE.TenTrees.Models;

namespace OE.TenTrees.Controllers
{
    [Route(ControllerRoutes.ApiRoute)]
    public class ApplicationController : ModuleControllerBase
    {
        private readonly IApplicationService _applicationService;

        public ApplicationController(IApplicationService applicationService, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _applicationService = applicationService;
        }

        // GET: api/<controller>?moduleid=x
        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<IEnumerable<TreePlantingApplication>> Get(string moduleid)
        {
            int ModuleId;
            if (int.TryParse(moduleid, out ModuleId) && IsAuthorizedEntityId(EntityNames.Module, ModuleId))
            {
                return await _applicationService.GetApplicationsAsync(ModuleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Application Get Attempt {ModuleId}", moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        // GET api/<controller>/5/1
        [HttpGet("{id}/{moduleid}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<TreePlantingApplication> Get(int id, int moduleid)
        {
            TreePlantingApplication application = await _applicationService.GetApplicationAsync(id, moduleid);
            if (application != null && IsAuthorizedEntityId(EntityNames.Module, application.ModuleId))
            {
                return application;
            }
            else
            { 
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Application Get Attempt {ApplicationId} {ModuleId}", id, moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<TreePlantingApplication> Post([FromBody] TreePlantingApplication application)
        {
            if (ModelState.IsValid && IsAuthorizedEntityId(EntityNames.Module, application.ModuleId))
            {
                application = await _applicationService.AddApplicationAsync(application);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Application Post Attempt {Application}", application);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                application = null;
            }
            return application;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<TreePlantingApplication> Put(int id, [FromBody] TreePlantingApplication application)
        {
            if (ModelState.IsValid && application.ApplicationId == id && IsAuthorizedEntityId(EntityNames.Module, application.ModuleId))
            {
                application = await _applicationService.UpdateApplicationAsync(application);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Application Put Attempt {Application}", application);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                application = null;
            }
            return application;
        }

        // DELETE api/<controller>/5/1
        [HttpDelete("{id}/{moduleid}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task Delete(int id, int moduleid)
        {
            TreePlantingApplication application = await _applicationService.GetApplicationAsync(id, moduleid);
            if (application != null && IsAuthorizedEntityId(EntityNames.Module, application.ModuleId))
            {
                await _applicationService.DeleteApplicationAsync(id, application.ModuleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Application Delete Attempt {ApplicationId} {ModuleId}", id, moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
        }

        // POST api/<controller>/approve/5
        [HttpPost("approve/{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<TreePlantingApplication> Approve(int id, [FromBody] ApproveApplicationRequest request)
        {
            TreePlantingApplication application = await _applicationService.GetApplicationAsync(id, 0); // We'll get the module ID from the application
            if (application != null && IsAuthorizedEntityId(EntityNames.Module, application.ModuleId))
            {
                application = await _applicationService.ApproveApplicationAsync(id, application.ModuleId, request.Comments);
                return application;
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Application Approve Attempt {ApplicationId}", id);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        // POST api/<controller>/reject/5
        [HttpPost("reject/{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<TreePlantingApplication> Reject(int id, [FromBody] RejectApplicationRequest request)
        {
            TreePlantingApplication application = await _applicationService.GetApplicationAsync(id, 0); // We'll get the module ID from the application
            if (application != null && IsAuthorizedEntityId(EntityNames.Module, application.ModuleId))
            {
                application = await _applicationService.RejectApplicationAsync(id, application.ModuleId, request.RejectionReason);
                return application;
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Application Reject Attempt {ApplicationId}", id);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        // POST api/<controller>/review
        [HttpPost("review")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ApplicationReview> PostReview([FromBody] ApplicationReview review)
        {
            if (ModelState.IsValid)
            {
                review = await _applicationService.AddReviewAsync(review);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Application Review Post Attempt {Review}", review);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                review = null;
            }
            return review;
        }

        // GET api/<controller>/reviews/5
        [HttpGet("reviews/{applicationId}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<IEnumerable<ApplicationReview>> GetReviews(int applicationId)
        {
            return await _applicationService.GetApplicationReviewsAsync(applicationId);
        }
    }

    // Request DTOs for approve/reject actions
    public class ApproveApplicationRequest
    {
        public int ApplicationId { get; set; }
        public string Comments { get; set; }
    }

    public class RejectApplicationRequest
    {
        public int ApplicationId { get; set; }
        public string RejectionReason { get; set; }
    }
}