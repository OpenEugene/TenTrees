using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Controllers;
using OpenEug.TenTrees.Module.Training.Services;
using OpenEug.TenTrees.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenEug.TenTrees.Module.Training.Controllers
{
    [Route(ControllerRoutes.ApiRoute)]
    public class TrainingController : ModuleControllerBase
    {
        private readonly ITrainingService _trainingService;

        public TrainingController(ITrainingService trainingService, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _trainingService = trainingService;
        }

        // GET: api/<controller>?moduleid=x&villageid=y
        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<ActionResult<IEnumerable<TrainingClass>>> Get(string moduleid, int? villageid = null)
        {
            if (!int.TryParse(moduleid, out var moduleId) || !IsAuthorizedEntityId(EntityNames.Module, moduleId))
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Training Classes Get Attempt {ModuleId}", moduleid);
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            try
            {
                var classes = await _trainingService.GetTrainingClassesAsync(moduleId, villageid);
                return Ok(classes);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Training Classes Get Failed {ModuleId} {VillageId} {Error}", moduleId, villageid, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET api/<controller>/5?moduleid=x
        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<ActionResult<TrainingClass>> Get(int id, int moduleid)
        {
            try
            {
                var trainingClass = await _trainingService.GetTrainingClassAsync(id, moduleid);
                if (trainingClass == null)
                {
                    return NotFound();
                }

                if (!IsAuthorizedEntityId(EntityNames.Module, trainingClass.ModuleId))
                {
                    _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Training Class Get Attempt {ClassId} {ModuleId}", id, moduleid);
                    return StatusCode(StatusCodes.Status403Forbidden);
                }

                return Ok(trainingClass);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Training Class Get Failed {ClassId} {ModuleId} {Error}", id, moduleid, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult<TrainingClass>> Post([FromBody] TrainingClass trainingClass)
        {
            if (trainingClass == null)
            {
                return BadRequest();
            }

            if (!IsAuthorizedEntityId(EntityNames.Module, trainingClass.ModuleId))
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Training Class Add Attempt {TrainingClass}", trainingClass);
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            try
            {
                var created = await _trainingService.AddTrainingClassAsync(trainingClass);
                return Ok(created);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Create, "Training Class Add Failed {ClassId} {ModuleId} {Error}", trainingClass?.TrainingClassId, trainingClass?.ModuleId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult<TrainingClass>> Put(int id, [FromBody] TrainingClass trainingClass)
        {
            if (trainingClass == null)
            {
                return BadRequest();
            }

            if (!IsAuthorizedEntityId(EntityNames.Module, trainingClass.ModuleId))
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Training Class Update Attempt {TrainingClass}", trainingClass);
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            if (trainingClass.TrainingClassId != 0 && trainingClass.TrainingClassId != id)
            {
                return BadRequest();
            }

            try
            {
                var updated = await _trainingService.UpdateTrainingClassAsync(trainingClass);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Update, "Training Class Update Failed {ClassId} {ModuleId} {Error}", id, trainingClass.ModuleId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE api/<controller>/5?moduleid=x
        [HttpDelete("{id}/{moduleid}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult> Delete(int id, int moduleid)
        {
            if (!IsAuthorizedEntityId(EntityNames.Module, moduleid))
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Training Class Delete Attempt {ClassId} {ModuleId}", id, moduleid);
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            try
            {
                await _trainingService.DeleteTrainingClassAsync(id, moduleid);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Delete, "Training Class Delete Failed {ClassId} {ModuleId} {Error}", id, moduleid, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET api/<controller>/attendance/5?moduleid=x
        [HttpGet("attendance/{classId}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<ActionResult<IEnumerable<ClassAttendance>>> GetAttendance(int classId, int moduleid)
        {
            if (!IsAuthorizedEntityId(EntityNames.Module, moduleid))
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Attendance Get Attempt {ClassId} {ModuleId}", classId, moduleid);
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            try
            {
                var attendance = await _trainingService.GetAttendanceForClassAsync(classId, moduleid);
                return Ok(attendance);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Attendance Get Failed {ClassId} {ModuleId} {Error}", classId, moduleid, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST api/<controller>/attendance
        [HttpPost("attendance")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult> MarkAttendance([FromBody] MarkAttendanceRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            if (!IsAuthorizedEntityId(EntityNames.Module, request.ModuleId))
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Attendance Mark Attempt {ClassId} {ModuleId}", request.TrainingClassId, request.ModuleId);
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            try
            {
                await _trainingService.MarkAttendanceAsync(request);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Update, "Attendance Mark Failed {ClassId} {ModuleId} {Error}", request.TrainingClassId, request.ModuleId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET api/<controller>/summaries?moduleid=x&villageid=y
        [HttpGet("summaries")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<ActionResult<IEnumerable<AttendanceSummaryViewModel>>> GetSummaries(int moduleid, int? villageid = null)
        {
            if (!IsAuthorizedEntityId(EntityNames.Module, moduleid))
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Attendance Summaries Get Attempt {ModuleId}", moduleid);
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            try
            {
                var summaries = await _trainingService.GetAttendanceSummariesAsync(moduleid, villageid);
                return Ok(summaries);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Attendance Summaries Get Failed {ModuleId} {VillageId} {Error}", moduleid, villageid, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET api/<controller>/grower-summary/5?moduleid=x
        [HttpGet("grower-summary/{growerId}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<ActionResult<AttendanceSummaryViewModel>> GetGrowerSummary(int growerId, int moduleid)
        {
            if (!IsAuthorizedEntityId(EntityNames.Module, moduleid))
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Grower Attendance Summary Get Attempt {GrowerId} {ModuleId}", growerId, moduleid);
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            try
            {
                var summary = await _trainingService.GetGrowerAttendanceSummaryAsync(growerId, moduleid);
                if (summary == null)
                {
                    return NotFound();
                }

                return Ok(summary);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Grower Attendance Summary Get Failed {GrowerId} {ModuleId} {Error}", growerId, moduleid, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET api/<controller>/status-summary?moduleid=x&villageid=y
        [HttpGet("status-summary")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<ActionResult<TrainingStatusSummary>> GetStatusSummary(int moduleid, int? villageid = null)
        {
            if (!IsAuthorizedEntityId(EntityNames.Module, moduleid))
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Training Status Summary Get Attempt {ModuleId}", moduleid);
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            try
            {
                var summary = await _trainingService.GetTrainingStatusSummaryAsync(moduleid, villageid);
                return Ok(summary);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Training Status Summary Get Failed {ModuleId} {VillageId} {Error}", moduleid, villageid, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
