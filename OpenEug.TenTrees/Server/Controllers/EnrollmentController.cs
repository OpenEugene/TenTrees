using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using OpenEug.TenTrees.Module.Enrollment.Services;
using Oqtane.Controllers;
using System.Net;
using System.Threading.Tasks;

namespace OpenEug.TenTrees.Module.Enrollment.Controllers
{
    [Route(ControllerRoutes.ApiRoute)]
    public class EnrollmentController : ModuleControllerBase
    {
        private readonly IEnrollmentService _EnrollmentService;

        public EnrollmentController(IEnrollmentService EnrollmentService, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _EnrollmentService = EnrollmentService;
        }

        // GET: api/<controller>?moduleid=x
        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<IEnumerable<Models.Enrollment>> Get(string moduleid)
        {
            int ModuleId;
            if (int.TryParse(moduleid, out ModuleId) && IsAuthorizedEntityId(EntityNames.Module, ModuleId))
            {
                return await _EnrollmentService.GetEnrollmentsAsync(ModuleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Enrollment Get Attempt {ModuleId}", moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        // GET api/<controller>/5
        [HttpGet("{id}/{moduleid}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<Models.Enrollment> Get(int id, int moduleid)
        {
            Models.Enrollment Enrollment = await _EnrollmentService.GetEnrollmentAsync(id, moduleid);
            if (Enrollment != null && IsAuthorizedEntityId(EntityNames.Module, Enrollment.ModuleId))
            {
                return Enrollment;
            }
            else
            { 
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Enrollment Get Attempt {EnrollmentId} {ModuleId}", id, moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<Models.Enrollment> Post([FromBody] Models.Enrollment Enrollment)
        {
            if (ModelState.IsValid && IsAuthorizedEntityId(EntityNames.Module, Enrollment.ModuleId))
            {
                Enrollment = await _EnrollmentService.AddEnrollmentAsync(Enrollment);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Enrollment Post Attempt {Enrollment}", Enrollment);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                Enrollment = null;
            }
            return Enrollment;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<Models.Enrollment> Put(int id, [FromBody] Models.Enrollment Enrollment)
        {
            if (ModelState.IsValid && Enrollment.EnrollmentId == id && IsAuthorizedEntityId(EntityNames.Module, Enrollment.ModuleId))
            {
                Enrollment = await _EnrollmentService.UpdateEnrollmentAsync(Enrollment);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Enrollment Put Attempt {Enrollment}", Enrollment);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                Enrollment = null;
            }
            return Enrollment;
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}/{moduleid}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task Delete(int id, int moduleid)
        {
            Models.Enrollment Enrollment = await _EnrollmentService.GetEnrollmentAsync(id, moduleid);
            if (Enrollment != null && IsAuthorizedEntityId(EntityNames.Module, Enrollment.ModuleId))
            {
                await _EnrollmentService.DeleteEnrollmentAsync(id, Enrollment.ModuleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Enrollment Delete Attempt {EnrollmentId} {ModuleId}", id, moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
        }
    }
}
