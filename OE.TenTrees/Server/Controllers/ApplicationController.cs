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

        // GET: api/<controller>
        [HttpGet]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task<IEnumerable<TreePlantingApplication>> Get()
        {
            return await _applicationService.GetApplicationsAsync();
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task<TreePlantingApplication> Get(int id)
        {
            return await _applicationService.GetApplicationAsync(id);
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task<TreePlantingApplication> Post([FromBody] TreePlantingApplication application)
        {
            if (ModelState.IsValid)
            {
                return await _applicationService.AddApplicationAsync(application);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Invalid Application Post Attempt {Application}", application);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return null;
            }
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task<TreePlantingApplication> Put(int id, [FromBody] TreePlantingApplication application)
        {
            if (ModelState.IsValid && application.ApplicationId == id)
            {
                return await _applicationService.UpdateApplicationAsync(application);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Invalid Application Put Attempt {Application}", application);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return null;
            }
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task Delete(int id)
        {
            await _applicationService.DeleteApplicationAsync(id);
        }

        // POST api/<controller>/approve/5
        [HttpPost("approve/{id}")]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task<TreePlantingApplication> Approve(int id, [FromBody] ApproveApplicationRequest request)
        {
            return await _applicationService.ApproveApplicationAsync(id, request.Comments);
        }

        // POST api/<controller>/reject/5
        [HttpPost("reject/{id}")]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task<TreePlantingApplication> Reject(int id, [FromBody] RejectApplicationRequest request)
        {
            return await _applicationService.RejectApplicationAsync(id, request.RejectionReason);
        }

        // POST api/<controller>/review
        [HttpPost("review")]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task<ApplicationReview> PostReview([FromBody] ApplicationReview review)
        {
            if (ModelState.IsValid)
            {
                return await _applicationService.AddReviewAsync(review);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Invalid Application Review Post Attempt {Review}", review);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return null;
            }
        }

        // GET api/<controller>/reviews/5
        [HttpGet("reviews/{applicationId}")]
        [Authorize(Roles = RoleNames.Registered)]
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