using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenEug.TenTrees.Module.Grower.Services;
using OpenEug.TenTrees.Models;
using Oqtane.Controllers;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenEug.TenTrees.Module.Grower.Controllers
{
    [Route(ControllerRoutes.ApiRoute)]
    public class OrchardController : ModuleControllerBase
    {
        private readonly IOrchardService _orchardService;

        public OrchardController(IOrchardService orchardService, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _orchardService = orchardService;
        }

        [HttpGet("grower/{growerId}")]
        [Authorize]
        public async Task<ActionResult<List<OrchardWithTreesDto>>> GetForGrower(int growerId)
        {
            try
            {
                var result = await _orchardService.GetOrchardsForGrowerAsync(growerId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Orchard GetForGrower Failed {GrowerId} {Error}", growerId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("tree")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult<TreeDetailDto>> AddTree([FromBody] AddTreeRequest request, [FromQuery] int moduleId)
        {
            try
            {
                var result = await _orchardService.AddTreeToGrowerAsync(request, moduleId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Create, "Orchard AddTree Failed {Request} {Error}", request, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("tree/{treeId}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult> DeleteTree(int treeId, [FromQuery] int moduleId)
        {
            try
            {
                await _orchardService.DeleteTreeAsync(treeId, moduleId);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Delete, "Orchard DeleteTree Failed {TreeId} {Error}", treeId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
