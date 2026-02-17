using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Controllers;
using OpenEug.TenTrees.Module.Training.Services;
using OpenEug.TenTrees.Models;
using System.Collections.Generic;
using System.Net;
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
        public async Task<IEnumerable<TrainingClass>> Get(string moduleid, int? villageid = null)
        {
            int ModuleId;
            if (int.TryParse(moduleid, out ModuleId) && IsAuthorizedEntityId(EntityNames.Module, ModuleId))
            {
                return await _trainingService.GetTrainingClassesAsync(ModuleId, villageid);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Training Classes Get Attempt {ModuleId}", moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        // GET api/<controller>/5?moduleid=x
        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<TrainingClass> Get(int id, int moduleid)
        {
            TrainingClass trainingClass = await _trainingService.GetTrainingClassAsync(id, moduleid);
            if (trainingClass != null && IsAuthorizedEntityId(EntityNames.Module, trainingClass.ModuleId))
            {
                return trainingClass;
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Training Class Get Attempt {ClassId} {ModuleId}", id, moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<TrainingClass> Post([FromBody] TrainingClass trainingClass)
        {
            if (IsAuthorizedEntityId(EntityNames.Module, trainingClass.ModuleId))
            {
                return await _trainingService.AddTrainingClassAsync(trainingClass);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Training Class Add Attempt {TrainingClass}", trainingClass);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<TrainingClass> Put(int id, [FromBody] TrainingClass trainingClass)
        {
            if (IsAuthorizedEntityId(EntityNames.Module, trainingClass.ModuleId))
            {
                return await _trainingService.UpdateTrainingClassAsync(trainingClass);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Training Class Update Attempt {TrainingClass}", trainingClass);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        // DELETE api/<controller>/5?moduleid=x
        [HttpDelete("{id}/{moduleid}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task Delete(int id, int moduleid)
        {
            if (IsAuthorizedEntityId(EntityNames.Module, moduleid))
            {
                await _trainingService.DeleteTrainingClassAsync(id, moduleid);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Training Class Delete Attempt {ClassId} {ModuleId}", id, moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
        }

        // GET api/<controller>/attendance/5?moduleid=x
        [HttpGet("attendance/{classId}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<IEnumerable<ClassAttendance>> GetAttendance(int classId, int moduleid)
        {
            if (IsAuthorizedEntityId(EntityNames.Module, moduleid))
            {
                return await _trainingService.GetAttendanceForClassAsync(classId, moduleid);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Attendance Get Attempt {ClassId} {ModuleId}", classId, moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        // POST api/<controller>/attendance
        [HttpPost("attendance")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task MarkAttendance([FromBody] MarkAttendanceRequest request)
        {
            if (IsAuthorizedEntityId(EntityNames.Module, request.ModuleId))
            {
                await _trainingService.MarkAttendanceAsync(request);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Attendance Mark Attempt {ClassId} {ModuleId}", request.TrainingClassId, request.ModuleId);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
        }

        // GET api/<controller>/summaries?moduleid=x&villageid=y
        [HttpGet("summaries")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<IEnumerable<AttendanceSummaryViewModel>> GetSummaries(int moduleid, int? villageid = null)
        {
            if (IsAuthorizedEntityId(EntityNames.Module, moduleid))
            {
                return await _trainingService.GetAttendanceSummariesAsync(moduleid, villageid);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Attendance Summaries Get Attempt {ModuleId}", moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        // GET api/<controller>/grower-summary/5?moduleid=x
        [HttpGet("grower-summary/{growerId}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<AttendanceSummaryViewModel> GetGrowerSummary(int growerId, int moduleid)
        {
            if (IsAuthorizedEntityId(EntityNames.Module, moduleid))
            {
                return await _trainingService.GetGrowerAttendanceSummaryAsync(growerId, moduleid);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Grower Attendance Summary Get Attempt {GrowerId} {ModuleId}", growerId, moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        // GET api/<controller>/status-summary?moduleid=x&villageid=y
        [HttpGet("status-summary")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<TrainingStatusSummary> GetStatusSummary(int moduleid, int? villageid = null)
        {
            if (IsAuthorizedEntityId(EntityNames.Module, moduleid))
            {
                return await _trainingService.GetTrainingStatusSummaryAsync(moduleid, villageid);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Training Status Summary Get Attempt {ModuleId}", moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }
    }
}
