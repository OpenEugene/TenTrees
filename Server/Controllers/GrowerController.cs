using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenEug.TenTrees.Module.Grower.Services;
using OpenEug.TenTrees.Models;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenEug.TenTrees.Module.Grower.Controllers
{
    [Route("api/[controller]")]
    public class GrowerController : Controller
    {
        private readonly IGrowerService _growerService;
        private readonly ILogManager _logger;

        public GrowerController(IGrowerService growerService, ILogManager logger)
        {
            _growerService = growerService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<ActionResult<Models.Grower>> Get(int id, int moduleId)
        {
            try
            {
                var grower = await _growerService.GetGrowerAsync(id, moduleId);
                if (grower == null)
                {
                    return NotFound();
                }

                return Ok(grower);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Grower Get Failed {GrowerId} {ModuleId} {Error}", id, moduleId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("all")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<ActionResult<List<Models.Grower>>> GetAll(int moduleId, int? villageId = null)
        {
            try
            {
                var growers = await _growerService.GetAllGrowersAsync(moduleId, villageId);
                return Ok(growers);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Grower GetAll Failed {ModuleId} {VillageId} {Error}", moduleId, villageId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("toggle-status")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult<Models.Grower>> ToggleStatus([FromBody] StatusToggleRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var grower = await _growerService.ToggleActiveStatusAsync(request.GrowerId, request.ModuleId);
                if (grower == null)
                {
                    return NotFound();
                }

                return Ok(grower);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Update, "Grower ToggleStatus Failed {GrowerId} {ModuleId} {Error}", request?.GrowerId, request?.ModuleId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("exit")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult<Models.Grower>> RecordExit([FromBody] ProgramExitRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var grower = await _growerService.RecordProgramExitAsync(request.GrowerId, request, request.ModuleId);
                if (grower == null)
                {
                    return NotFound();
                }

                return Ok(grower);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Update, "Grower RecordExit Failed {GrowerId} {ModuleId} {Error}", request?.GrowerId, request?.ModuleId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("active")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<ActionResult<List<Models.Grower>>> GetActiveGrowers(int moduleId, int? villageId = null)
        {
            try
            {
                var growers = await _growerService.GetActiveGrowersAsync(moduleId, villageId);
                return Ok(growers);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Grower GetActive Failed {ModuleId} {VillageId} {Error}", moduleId, villageId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("by-status")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<ActionResult<List<Models.Grower>>> GetByStatus(int moduleId, GrowerStatus status, int? villageId = null)
        {
            try
            {
                var growers = await _growerService.GetGrowersByStatusAsync(status, moduleId, villageId);
                return Ok(growers);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Grower GetByStatus Failed {Status} {ModuleId} {VillageId} {Error}", status, moduleId, villageId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("status-summary")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<ActionResult<GrowerStatusSummary>> GetStatusSummary(int moduleId, int? villageId = null)
        {
            try
            {
                var summary = await _growerService.GetStatusSummaryAsync(moduleId, villageId);
                return Ok(summary);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Grower StatusSummary Failed {ModuleId} {VillageId} {Error}", moduleId, villageId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult<Models.Grower>> Put(int id, [FromBody] Models.Grower grower, int moduleId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (grower == null || grower.GrowerId != id)
                {
                    return BadRequest();
                }

                var updated = await _growerService.UpdateGrowerAsync(grower, moduleId);
                if (updated == null)
                {
                    return NotFound();
                }

                return Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Update, "Grower Update Failed {GrowerId} {ModuleId} {Error}", id, moduleId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
