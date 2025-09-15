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
    public class MonitoringController : ModuleControllerBase
    {
        private readonly IMonitoringService _monitoringService;

        public MonitoringController(IMonitoringService monitoringService, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _monitoringService = monitoringService;
        }

        // GET: api/<controller>
        [HttpGet]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task<IEnumerable<MonitoringSession>> Get()
        {
            return await _monitoringService.GetMonitoringSessionsAsync();
        }

        // GET: api/<controller>/application/5
        [HttpGet("application/{applicationId}")]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task<IEnumerable<MonitoringSession>> GetByApplication(int applicationId)
        {
            return await _monitoringService.GetMonitoringSessionsByApplicationAsync(applicationId);
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task<MonitoringSession> Get(int id)
        {
            return await _monitoringService.GetMonitoringSessionAsync(id);
        }

        // GET api/<controller>/details/5
        [HttpGet("details/{id}")]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task<MonitoringSessionVm> GetDetails(int id)
        {
            return await _monitoringService.GetMonitoringSessionWithDetailsAsync(id);
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task<MonitoringSession> Post([FromBody] MonitoringSession session)
        {
            if (ModelState.IsValid)
            {
                return await _monitoringService.AddMonitoringSessionAsync(session);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Invalid Monitoring Session Post Attempt {Session}", session);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return null;
            }
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task<MonitoringSession> Put(int id, [FromBody] MonitoringSession session)
        {
            if (ModelState.IsValid && session.MonitoringSessionId == id)
            {
                return await _monitoringService.UpdateMonitoringSessionAsync(session);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Invalid Monitoring Session Put Attempt {Session}", session);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return null;
            }
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task Delete(int id)
        {
            await _monitoringService.DeleteMonitoringSessionAsync(id);
        }

        // GET api/<controller>/photos/5
        [HttpGet("photos/{sessionId}")]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task<IEnumerable<MonitoringPhoto>> GetPhotos(int sessionId)
        {
            return await _monitoringService.GetMonitoringPhotosAsync(sessionId);
        }

        // POST api/<controller>/photos
        [HttpPost("photos")]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task<MonitoringPhoto> PostPhoto([FromBody] MonitoringPhoto photo)
        {
            if (ModelState.IsValid)
            {
                return await _monitoringService.AddMonitoringPhotoAsync(photo);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Invalid Monitoring Photo Post Attempt {Photo}", photo);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return null;
            }
        }

        // DELETE api/<controller>/photos/5
        [HttpDelete("photos/{id}")]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task DeletePhoto(int id)
        {
            await _monitoringService.DeleteMonitoringPhotoAsync(id);
        }

        // GET api/<controller>/applications
        [HttpGet("applications")]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task<IEnumerable<TreePlantingApplication>> GetApplicationsForMonitoring()
        {
            _logger.Log(LogLevel.Information, this, LogFunction.Read, "GetApplicationsForMonitoring called");
            
            var applications = await _monitoringService.GetApplicationsForMonitoringAsync();
            _logger.Log(LogLevel.Information, this, LogFunction.Read, "Found {ApplicationCount} applications for monitoring", applications?.Count ?? 0);
            return applications;
        }

        // GET: api/<controller>/listitems
        [HttpGet("listitems")]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task<IEnumerable<MonitoringListItemVm>> GetListItems()
        {
            return await _monitoringService.GetMonitoringSessionListItemsAsync();
        }
    }
}