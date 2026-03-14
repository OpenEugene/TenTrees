using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using OpenEug.TenTrees.Module.Village.Services;
using Models = OpenEug.TenTrees.Models;
using Oqtane.Controllers;
using System.Net;
using System.Threading.Tasks;

namespace OpenEug.TenTrees.Module.Village.Controllers
{
    [Route(ControllerRoutes.ApiRoute)]
    public class VillageController : ModuleControllerBase
    {
        private readonly IVillageService _villageService;

        public VillageController(IVillageService villageService, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _villageService = villageService;
        }

        // GET: api/<controller>
        [HttpGet]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<IEnumerable<Models.Village>> Get()
        {
            return await _villageService.GetVillagesAsync();
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<Models.Village> Get(int id)
        {
            Models.Village village = await _villageService.GetVillageAsync(id);
            if (village != null)
            {
                return village;
            }
            else
            { 
                _logger.Log(LogLevel.Warning, this, LogFunction.Security, "Village Not Found {VillageId}", id);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return null;
            }
        }

        // GET api/<controller>/active
        [HttpGet("active")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<IEnumerable<Models.Village>> GetActive()
        {
            return await _villageService.GetActiveVillagesAsync();
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<Models.Village> Post([FromBody] Models.Village village)
        {
            if (ModelState.IsValid)
            {
                village = await _villageService.AddVillageAsync(village);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Village Post Attempt {Village}", village);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                village = null;
            }
            return village;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<Models.Village> Put(int id, [FromBody] Models.Village village)
        {
            if (ModelState.IsValid && village.VillageId == id)
            {
                village = await _villageService.UpdateVillageAsync(village);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Village Put Attempt {Village}", village);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                village = null;
            }
            return village;
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task Delete(int id)
        {
            await _villageService.DeleteVillageAsync(id);
        }
    }
}
