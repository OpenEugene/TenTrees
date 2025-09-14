using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using OE.TenTrees.Models;
using OE.TenTrees.Repository;

namespace OE.TenTrees.Controllers
{
    [Route(ControllerRoutes.ApiRoute)]
    public class PlantingEventController : Controller
    {
        private readonly IPlantingEventRepository _PlantingEventRepository;
        private readonly ILogManager _logger;

        public PlantingEventController(IPlantingEventRepository PlantingEventRepository, ILogManager logger)
        {
            _PlantingEventRepository = PlantingEventRepository;
            _logger = logger;
        }

        // GET: api/<controller>?moduleid=x
        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<IEnumerable<PlantingEvent>> Get(string moduleid)
        {
            int ModuleId;
            if (int.TryParse(moduleid, out ModuleId) && IsAuthorized(EntityNames.Module, ModuleId, PermissionNames.View))
            {
                return await _PlantingEventRepository.GetPlantingEventsAsync(ModuleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PlantingEvent Get Attempt {ModuleId}", moduleid);
                HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return new List<PlantingEvent>();
            }
        }

        // GET api/<controller>/5?moduleid=x
        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<PlantingEvent?> Get(int id, string moduleid)
        {
            int ModuleId;
            if (int.TryParse(moduleid, out ModuleId) && IsAuthorized(EntityNames.Module, ModuleId, PermissionNames.View))
            {
                return await _PlantingEventRepository.GetPlantingEventAsync(id, ModuleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PlantingEvent Get Attempt {PlantingEventId} {ModuleId}", id, moduleid);
                HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return null;
            }
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<PlantingEvent> Post([FromBody] PlantingEvent PlantingEvent)
        {
            if (ModelState.IsValid && IsAuthorized(EntityNames.Module, PlantingEvent.ModuleId, PermissionNames.Edit))
            {
                PlantingEvent = await _PlantingEventRepository.AddPlantingEventAsync(PlantingEvent);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "PlantingEvent Added {PlantingEvent}", PlantingEvent);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PlantingEvent Post Attempt {PlantingEvent}", PlantingEvent);
                HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                PlantingEvent = null;
            }
            return PlantingEvent;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<PlantingEvent> Put(int id, [FromBody] PlantingEvent PlantingEvent)
        {
            if (ModelState.IsValid && PlantingEvent.PlantingEventId == id && IsAuthorized(EntityNames.Module, PlantingEvent.ModuleId, PermissionNames.Edit))
            {
                PlantingEvent = await _PlantingEventRepository.UpdatePlantingEventAsync(PlantingEvent);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "PlantingEvent Updated {PlantingEvent}", PlantingEvent);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PlantingEvent Put Attempt {PlantingEvent}", PlantingEvent);
                HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                PlantingEvent = null;
            }
            return PlantingEvent;
        }

        // DELETE api/<controller>/5?moduleid=x
        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task Delete(int id, string moduleid)
        {
            int ModuleId;
            if (int.TryParse(moduleid, out ModuleId) && IsAuthorized(EntityNames.Module, ModuleId, PermissionNames.Edit))
            {
                await _PlantingEventRepository.DeletePlantingEventAsync(id, ModuleId);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "PlantingEvent Deleted {PlantingEventId}", id);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized PlantingEvent Delete Attempt {PlantingEventId} {ModuleId}", id, moduleid);
                HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
            }
        }

        private bool IsAuthorized(string entityName, int entityId, string permissionName)
        {
            return User.IsInRole(RoleNames.Admin) || 
                   UserPermissions.IsAuthorized(User, entityName, entityId, permissionName);
        }
    }
}