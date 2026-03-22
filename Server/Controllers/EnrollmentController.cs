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
using System;
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
        [Authorize]
        public async Task<ActionResult<IEnumerable<Models.Enrollment>>> Get(string moduleid)
        {
            if (!int.TryParse(moduleid, out var moduleId) || !IsAuthorizedEntityId(EntityNames.Module, moduleId))
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Enrollment Get Attempt {ModuleId}", moduleid);
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            try
            {
                var enrollments = await _EnrollmentService.GetEnrollmentsAsync(moduleId);
                return Ok(enrollments);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Enrollment Get Failed {ModuleId} {Error}", moduleId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/<controller>/listviewmodels?moduleid=x
        [HttpGet("listviewmodels")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<EnrollmentListViewModel>>> GetListViewModels(string moduleid)
        {
            if (!int.TryParse(moduleid, out var moduleId) || !IsAuthorizedEntityId(EntityNames.Module, moduleId))
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Enrollment List ViewModel Get Attempt {ModuleId}", moduleid);
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            try
            {
                var list = await _EnrollmentService.GetEnrollmentListViewModelsAsync(moduleId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Enrollment List ViewModel Get Failed {ModuleId} {Error}", moduleId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET api/<controller>/users?moduleid=x
        [HttpGet("users")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserInfo>>> GetUsers(string moduleid)
        {
            if (!int.TryParse(moduleid, out var moduleId) || !IsAuthorizedEntityId(EntityNames.Module, moduleId))
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized GetUsers Attempt {ModuleId}", moduleid);
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            try
            {
                var users = await _EnrollmentService.GetSiteUsersAsync(moduleId);
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Enrollment GetUsers Failed {ModuleId} {Error}", moduleId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET api/<controller>/5
        [HttpGet("{id}/{moduleid}")]
        [Authorize]
        public async Task<ActionResult<Models.Enrollment>> Get(int id, int moduleid)
        {
            try
            {
                var enrollment = await _EnrollmentService.GetEnrollmentAsync(id, moduleid);
                if (enrollment == null)
                {
                    return NotFound();
                }

                if (!IsAuthorizedEntityId(EntityNames.Module, enrollment.ModuleId))
                {
                    _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Enrollment Get Attempt {EnrollmentId} {ModuleId}", id, moduleid);
                    return StatusCode(StatusCodes.Status403Forbidden);
                }

                return Ok(enrollment);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Enrollment Get Failed {EnrollmentId} {ModuleId} {Error}", id, moduleid, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult<Models.Enrollment>> Post([FromBody] Models.Enrollment Enrollment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!IsAuthorizedEntityId(EntityNames.Module, Enrollment.ModuleId))
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Enrollment Post Attempt {Enrollment}", Enrollment);
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            try
            {
                var created = await _EnrollmentService.AddEnrollmentAsync(Enrollment);
                return Ok(created);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Create, "Enrollment Post Failed {EnrollmentId} {ModuleId} {Error}", Enrollment?.EnrollmentId, Enrollment?.ModuleId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult<Models.Enrollment>> Put(int id, [FromBody] Models.Enrollment Enrollment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (Enrollment == null || Enrollment.EnrollmentId != id)
            {
                return BadRequest();
            }

            if (!IsAuthorizedEntityId(EntityNames.Module, Enrollment.ModuleId))
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Enrollment Put Attempt {Enrollment}", Enrollment);
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            try
            {
                var updated = await _EnrollmentService.UpdateEnrollmentAsync(Enrollment);
                if (updated == null)
                {
                    return NotFound();
                }

                return Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Update, "Enrollment Put Failed {EnrollmentId} {ModuleId} {Error}", id, Enrollment.ModuleId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}/{moduleid}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult> Delete(int id, int moduleid)
        {
            try
            {
                var enrollment = await _EnrollmentService.GetEnrollmentAsync(id, moduleid);
                if (enrollment == null)
                {
                    return NotFound();
                }

                if (!IsAuthorizedEntityId(EntityNames.Module, enrollment.ModuleId))
                {
                    _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Enrollment Delete Attempt {EnrollmentId} {ModuleId}", id, moduleid);
                    return StatusCode(StatusCodes.Status403Forbidden);
                }

                await _EnrollmentService.DeleteEnrollmentAsync(id, enrollment.ModuleId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Delete, "Enrollment Delete Failed {EnrollmentId} {ModuleId} {Error}", id, moduleid, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        // POST api/<controller>/validate
        [HttpPost("validate")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult<ValidationResult>> Validate([FromBody] Models.Enrollment Enrollment)
        {
            if (!IsAuthorizedEntityId(EntityNames.Module, Enrollment.ModuleId))
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Enrollment Validate Attempt");
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            try
            {
                var validation = await _EnrollmentService.ValidateRequiredAsync(Enrollment);
                return Ok(validation);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Enrollment Validate Failed {EnrollmentId} {ModuleId} {Error}", Enrollment?.EnrollmentId, Enrollment?.ModuleId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        // POST api/<controller>/5/signature
        [HttpPost("{id}/signature")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult<bool>> CaptureSignature(int id, [FromBody] SignatureRequest request)
        {
            try
            {
                var enrollment = await _EnrollmentService.GetEnrollmentAsync(id, request.ModuleId);
                if (enrollment == null)
                {
                    return NotFound();
                }

                if (!IsAuthorizedEntityId(EntityNames.Module, enrollment.ModuleId))
                {
                    _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Signature Capture Attempt {EnrollmentId}", id);
                    return StatusCode(StatusCodes.Status403Forbidden);
                }

                var captured = await _EnrollmentService.CaptureSignatureAsync(id, request.SignatureData);
                return Ok(captured);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Update, "Signature Capture Failed {EnrollmentId} {ModuleId} {Error}", id, request?.ModuleId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST api/<controller>/5/photoconsent
        [HttpPost("{id}/photoconsent")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult<bool>> CapturePhotoConsent(int id, [FromBody] PhotoConsentRequest request)
        {
            try
            {
                var enrollment = await _EnrollmentService.GetEnrollmentAsync(id, request.ModuleId);
                if (enrollment == null)
                {
                    return NotFound();
                }

                if (!IsAuthorizedEntityId(EntityNames.Module, enrollment.ModuleId))
                {
                    _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Photo Consent Capture Attempt {EnrollmentId}", id);
                    return StatusCode(StatusCodes.Status403Forbidden);
                }

                var captured = await _EnrollmentService.CapturePhotoConsentAsync(id, request.ModuleId, request.ConsentLevel, request.SignatureData);
                return Ok(captured);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Update, "Photo Consent Capture Failed {EnrollmentId} {ModuleId} {Error}", id, request?.ModuleId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        // GET api/<controller>/mentor/5
        [HttpGet("mentor/{userid}")]
        [Authorize]
        public async Task<ActionResult<MentorInfo>> GetMentorInfo(int userid)
        {
            try
            {
                var mentor = await _EnrollmentService.AutoFillMentorAsync(userid);
                if (mentor == null)
                {
                    return NotFound();
                }

                return Ok(mentor);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Enrollment MentorInfo Get Failed {UserId} {Error}", userid, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        // GET api/<controller>/status/{status}?moduleid=x
        [HttpGet("status/{status}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Models.Enrollment>>> GetByStatus(Models.EnrollmentStatus status, string moduleid)
        {
            if (!int.TryParse(moduleid, out var moduleId) || !IsAuthorizedEntityId(EntityNames.Module, moduleId))
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Enrollment Get By Status Attempt {ModuleId}", moduleid);
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            try
            {
                var enrollments = await _EnrollmentService.GetByStatusAsync(moduleId, status);
                return Ok(enrollments);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Enrollment GetByStatus Failed {ModuleId} {Status} {Error}", moduleId, status, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        // GET api/<controller>/village/5
        [HttpGet("village/{villageid}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Models.Enrollment>>> GetByVillage(int villageid)
        {
            try
            {
                var enrollments = await _EnrollmentService.GetByVillageAsync(villageid);
                return Ok(enrollments);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Enrollment GetByVillage Failed {VillageId} {Error}", villageid, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST api/<controller>/backfill-growers
        [HttpPost("backfill-growers")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult<int>> BackfillGrowers(int moduleId)
        {
            if (!IsAuthorizedEntityId(EntityNames.Module, moduleId))
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Backfill Growers Attempt {ModuleId}", moduleId);
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            try
            {
                var count = await _EnrollmentService.BackfillGrowersFromEnrollmentsAsync(moduleId);
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Update, "Backfill Growers Failed {ModuleId} {Error}", moduleId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
    
    public class SignatureRequest
    {
        public int ModuleId { get; set; }
        public string SignatureData { get; set; }
    }

    public class PhotoConsentRequest
    {
        public int ModuleId { get; set; }
        public Models.PhotoConsentLevel ConsentLevel { get; set; }
        public string SignatureData { get; set; }
    }
}
