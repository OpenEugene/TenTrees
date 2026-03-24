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
        [Authorize]
        public async Task<ActionResult<Models.Grower>> Get(int id)
        {
            try
            {
                var grower = await _growerService.GetGrowerAsync(id);
                if (grower == null)
                {
                    return NotFound();
                }

                return Ok(grower);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Grower Get Failed {GrowerId} {Error}", id, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("all")]
        [Authorize]
        public async Task<ActionResult<List<Models.Grower>>> GetAll(int? villageId = null)
        {
            try
            {
                var growers = await _growerService.GetAllGrowersAsync(villageId);
                return Ok(growers);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Grower GetAll Failed {VillageId} {Error}", villageId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("toggle-status")]
        [Authorize]
        public async Task<ActionResult<Models.Grower>> ToggleStatus([FromBody] StatusToggleRequest request, int moduleId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var grower = await _growerService.ToggleActiveStatusAsync(request.GrowerId, moduleId);
                if (grower == null)
                {
                    return NotFound();
                }

                return Ok(grower);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Update, "Grower ToggleStatus Failed {GrowerId} {ModuleId} {Error}", request?.GrowerId, moduleId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("exit")]
        [Authorize]
        public async Task<ActionResult<Models.Grower>> RecordExit([FromBody] ProgramExitRequest request, int moduleId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var grower = await _growerService.RecordProgramExitAsync(request.GrowerId, request, moduleId);
                if (grower == null)
                {
                    return NotFound();
                }

                return Ok(grower);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Update, "Grower RecordExit Failed {GrowerId} {ModuleId} {Error}", request?.GrowerId, moduleId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("active")]
        [Authorize]
        public async Task<ActionResult<List<Models.Grower>>> GetActiveGrowers(int? villageId = null)
        {
            try
            {
                var growers = await _growerService.GetActiveGrowersAsync(villageId);
                return Ok(growers);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Grower GetActive Failed {VillageId} {Error}", villageId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("by-status")]
        [Authorize]
        public async Task<ActionResult<List<Models.Grower>>> GetByStatus(GrowerStatus status, int? villageId = null)
        {
            try
            {
                var growers = await _growerService.GetGrowersByStatusAsync(status, villageId);
                return Ok(growers);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Grower GetByStatus Failed {Status} {VillageId} {Error}", status, villageId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("status-summary")]
        [Authorize]
        public async Task<ActionResult<GrowerStatusSummary>> GetStatusSummary(int? villageId = null)
        {
            try
            {
                var summary = await _growerService.GetStatusSummaryAsync(villageId);
                return Ok(summary);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Grower StatusSummary Failed {VillageId} {Error}", villageId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
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
