using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using OpenEug.TenTrees.Module.Enrollment.Services;
using OpenEug.TenTrees.Models;
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

        // GET: api/<controller>/listviewmodels?moduleid=x
        [HttpGet("listviewmodels")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<IEnumerable<EnrollmentListViewModel>> GetListViewModels(string moduleid)
        {
            int ModuleId;
            if (int.TryParse(moduleid, out ModuleId) && IsAuthorizedEntityId(EntityNames.Module, ModuleId))
            {
                return await _EnrollmentService.GetEnrollmentListViewModelsAsync(ModuleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Enrollment List ViewModel Get Attempt {ModuleId}", moduleid);
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
        
        // POST api/<controller>/validate
        [HttpPost("validate")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ValidationResult> Validate([FromBody] Models.Enrollment Enrollment)
        {
            if (IsAuthorizedEntityId(EntityNames.Module, Enrollment.ModuleId))
            {
                return await _EnrollmentService.ValidateRequiredAsync(Enrollment);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Enrollment Validate Attempt");
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }
        
        // POST api/<controller>/5/signature
        [HttpPost("{id}/signature")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<bool> CaptureSignature(int id, [FromBody] SignatureRequest request)
        {
            var enrollment = await _EnrollmentService.GetEnrollmentAsync(id, request.ModuleId);
            if (enrollment != null && IsAuthorizedEntityId(EntityNames.Module, enrollment.ModuleId))
            {
                return await _EnrollmentService.CaptureSignatureAsync(id, request.SignatureData);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Signature Capture Attempt {EnrollmentId}", id);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return false;
            }
        }
        
        // GET api/<controller>/mentor/5
        [HttpGet("mentor/{userid}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<MentorInfo> GetMentorInfo(int userid)
        {
            return await _EnrollmentService.AutoFillMentorAsync(userid);
        }
        
        // GET api/<controller>/status/{status}?moduleid=x
        [HttpGet("status/{status}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<IEnumerable<Models.Enrollment>> GetByStatus(Models.EnrollmentStatus status, string moduleid)
        {
            int ModuleId;
            if (int.TryParse(moduleid, out ModuleId) && IsAuthorizedEntityId(EntityNames.Module, ModuleId))
            {
                return await _EnrollmentService.GetByStatusAsync(ModuleId, status);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Enrollment Get By Status Attempt {ModuleId}", moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }
        
        // GET api/<controller>/village/5
        [HttpGet("village/{villageid}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<IEnumerable<Models.Enrollment>> GetByVillage(int villageid)
        {
            return await _EnrollmentService.GetByVillageAsync(villageid);
        }

        // POST api/<controller>/backfill-growers
        [HttpPost("backfill-growers")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<int> BackfillGrowers(int moduleId)
        {
            if (IsAuthorizedEntityId(EntityNames.Module, moduleId))
            {
                return await _EnrollmentService.BackfillGrowersFromEnrollmentsAsync(moduleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Backfill Growers Attempt {ModuleId}", moduleId);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return 0;
            }
        }
    }
    
    public class SignatureRequest
    {
        public int ModuleId { get; set; }
        public string SignatureData { get; set; }
    }
}
