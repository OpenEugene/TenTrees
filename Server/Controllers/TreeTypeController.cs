using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using OpenEug.TenTrees.Module.TreeType.Services;
using Models = OpenEug.TenTrees.Models;
using Oqtane.Controllers;
using System;
using System.Threading.Tasks;

namespace OpenEug.TenTrees.Module.TreeType.Controllers
{
    [Route(ControllerRoutes.ApiRoute)]
    public class TreeTypeController : ModuleControllerBase
    {
        private readonly ITreeTypeService _treeTypeService;

        public TreeTypeController(ITreeTypeService treeTypeService, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _treeTypeService = treeTypeService;
        }

        // GET: api/<controller>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Models.TreeType>>> Get()
        {
            try
            {
                var treeTypes = await _treeTypeService.GetTreeTypesAsync();
                return Ok(treeTypes);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "TreeType Get Failed {Error}", ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Models.TreeType>> Get(int id)
        {
            try
            {
                var treeType = await _treeTypeService.GetTreeTypeAsync(id);
                if (treeType == null)
                {
                    _logger.Log(LogLevel.Warning, this, LogFunction.Read, "TreeType Not Found {TreeTypeId}", id);
                    return NotFound();
                }

                return Ok(treeType);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "TreeType Get Failed {TreeTypeId} {Error}", id, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET api/<controller>/active
        [HttpGet("active")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Models.TreeType>>> GetActive()
        {
            try
            {
                var treeTypes = await _treeTypeService.GetActiveTreeTypesAsync();
                return Ok(treeTypes);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "TreeType GetActive Failed {Error}", ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Models.TreeType>> Post([FromBody] Models.TreeType treeType)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.Log(LogLevel.Error, this, LogFunction.Create, "TreeType Add Failed Validation {TreeType}", treeType);
                    return BadRequest(ModelState);
                }

                var created = await _treeTypeService.AddTreeTypeAsync(treeType);
                return Ok(created);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Create, "TreeType Add Failed {TreeType} {Error}", treeType, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<Models.TreeType>> Put(int id, [FromBody] Models.TreeType treeType)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.Log(LogLevel.Error, this, LogFunction.Update, "TreeType Update Failed Validation {TreeType}", treeType);
                    return BadRequest(ModelState);
                }

                if (treeType == null || treeType.TreeTypeId != id)
                {
                    _logger.Log(LogLevel.Error, this, LogFunction.Update, "TreeType Update Failed Id Mismatch {RouteId} {TreeTypeId}", id, treeType?.TreeTypeId);
                    return BadRequest();
                }

                var updated = await _treeTypeService.UpdateTreeTypeAsync(treeType);
                if (updated == null)
                {
                    return NotFound();
                }

                return Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Update, "TreeType Update Failed {TreeTypeId} {Error}", id, ex.ToString());
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
                await _treeTypeService.DeleteTreeTypeAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Delete, "TreeType Delete Failed {TreeTypeId} {Error}", id, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
