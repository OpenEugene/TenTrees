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

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Models.Enrollment>>> Get()
        {
            try
            {
                var enrollments = await _EnrollmentService.GetEnrollmentsAsync();
                return Ok(enrollments);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Enrollment Get Failed {Error}", ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("listviewmodels")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<EnrollmentListViewModel>>> GetListViewModels()
        {
            try
            {
                var list = await _EnrollmentService.GetEnrollmentListViewModelsAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Enrollment List ViewModel Get Failed {Error}", ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("users")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserInfo>>> GetUsers()
        {
            try
            {
                var users = await _EnrollmentService.GetSiteUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Enrollment GetUsers Failed {Error}", ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Models.Enrollment>> Get(int id)
        {
            try
            {
                var enrollment = await _EnrollmentService.GetEnrollmentAsync(id);
                if (enrollment == null)
                {
                    return NotFound();
                }

                return Ok(enrollment);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Enrollment Get Failed {EnrollmentId} {Error}", id, ex.ToString());
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

            try
            {
                var created = await _EnrollmentService.AddEnrollmentAsync(Enrollment);
                return Ok(created);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Create, "Enrollment Post Failed {EnrollmentId} {Error}", Enrollment?.EnrollmentId, ex.ToString());
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
                _logger.Log(LogLevel.Error, this, LogFunction.Update, "Enrollment Put Failed {EnrollmentId} {Error}", id, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}/{moduleid}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult> Delete(int id, int moduleid)
        {
            try
            {
                var enrollment = await _EnrollmentService.GetEnrollmentAsync(id);
                if (enrollment == null)
                {
                    return NotFound();
                }

                await _EnrollmentService.DeleteEnrollmentAsync(id, moduleid);
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
            try
            {
                var validation = await _EnrollmentService.ValidateRequiredAsync(Enrollment);
                return Ok(validation);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Enrollment Validate Failed {EnrollmentId} {Error}", Enrollment?.EnrollmentId, ex.ToString());
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
                var enrollment = await _EnrollmentService.GetEnrollmentAsync(id);
                if (enrollment == null)
                {
                    return NotFound();
                }

                var captured = await _EnrollmentService.CaptureSignatureAsync(id, request.SignatureData);
                return Ok(captured);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Update, "Signature Capture Failed {EnrollmentId} {Error}", id, ex.ToString());
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
                var enrollment = await _EnrollmentService.GetEnrollmentAsync(id);
                if (enrollment == null)
                {
                    return NotFound();
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
        
        [HttpGet("status/{status}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Models.Enrollment>>> GetByStatus(Models.EnrollmentStatus status)
        {
            try
            {
                var enrollments = await _EnrollmentService.GetByStatusAsync(status);
                return Ok(enrollments);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Enrollment GetByStatus Failed {Status} {Error}", status, ex.ToString());
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
