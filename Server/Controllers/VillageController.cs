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
using System;
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
        [Authorize]
        public async Task<ActionResult<IEnumerable<Models.Village>>> Get()
        {
            try
            {
                var villages = await _villageService.GetVillagesAsync();
                return Ok(villages);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Village Get Failed {Error}", ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Models.Village>> Get(int id)
        {
            try
            {
                var village = await _villageService.GetVillageAsync(id);
                if (village == null)
                {
                    _logger.Log(LogLevel.Warning, this, LogFunction.Read, "Village Not Found {VillageId}", id);
                    return NotFound();
                }

                return Ok(village);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Village Get Failed {VillageId} {Error}", id, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET api/<controller>/active
        [HttpGet("active")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Models.Village>>> GetActive()
        {
            try
            {
                var villages = await _villageService.GetActiveVillagesAsync();
                return Ok(villages);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Village GetActive Failed {Error}", ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Models.Village>> Post([FromBody] Models.Village village)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.Log(LogLevel.Error, this, LogFunction.Create, "Village Add Failed Validation {Village}", village);
                    return BadRequest(ModelState);
                }

                var created = await _villageService.AddVillageAsync(village);
                return Ok(created);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Create, "Village Add Failed {Village} {Error}", village, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<Models.Village>> Put(int id, [FromBody] Models.Village village)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.Log(LogLevel.Error, this, LogFunction.Update, "Village Update Failed Validation {Village}", village);
                    return BadRequest(ModelState);
                }

                if (village == null || village.VillageId != id)
                {
                    _logger.Log(LogLevel.Error, this, LogFunction.Update, "Village Update Failed Id Mismatch {RouteId} {VillageId}", id, village?.VillageId);
                    return BadRequest();
                }

                var updated = await _villageService.UpdateVillageAsync(village);
                if (updated == null)
                {
                    return NotFound();
                }

                return Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Update, "Village Update Failed {VillageId} {Error}", id, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _villageService.DeleteVillageAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Delete, "Village Delete Failed {VillageId} {Error}", id, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
