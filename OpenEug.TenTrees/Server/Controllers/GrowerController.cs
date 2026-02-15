using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenEug.TenTrees.Module.Grower.Services;
using OpenEug.TenTrees.Models;
using Oqtane.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenEug.TenTrees.Module.Grower.Controllers
{
    [Route("api/[controller]")]
    public class GrowerController : Controller
    {
        private readonly IGrowerService _growerService;

        public GrowerController(IGrowerService growerService)
        {
            _growerService = growerService;
        }

        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<Models.Grower> Get(int id, int moduleId)
        {
            return await _growerService.GetGrowerAsync(id, moduleId);
        }

        [HttpGet("all")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<List<Models.Grower>> GetAll(int moduleId, int? villageId = null)
        {
            return await _growerService.GetAllGrowersAsync(moduleId, villageId);
        }

        [HttpPost("toggle-status")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<Models.Grower> ToggleStatus([FromBody] StatusToggleRequest request)
        {
            return await _growerService.ToggleActiveStatusAsync(request.GrowerId, request.ModuleId);
        }

        [HttpPost("exit")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<Models.Grower> RecordExit([FromBody] ProgramExitRequest request)
        {
            return await _growerService.RecordProgramExitAsync(request.GrowerId, request, request.ModuleId);
        }

        [HttpGet("active")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<List<Models.Grower>> GetActiveGrowers(int moduleId, int? villageId = null)
        {
            return await _growerService.GetActiveGrowersAsync(moduleId, villageId);
        }

        [HttpGet("by-status")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<List<Models.Grower>> GetByStatus(int moduleId, GrowerStatus status, int? villageId = null)
        {
            return await _growerService.GetGrowersByStatusAsync(status, moduleId, villageId);
        }

        [HttpGet("status-summary")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<GrowerStatusSummary> GetStatusSummary(int moduleId, int? villageId = null)
        {
            return await _growerService.GetStatusSummaryAsync(moduleId, villageId);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<Models.Grower> Put(int id, [FromBody] Models.Grower grower, int moduleId)
        {
            return await _growerService.UpdateGrowerAsync(grower, moduleId);
        }
    }
}
