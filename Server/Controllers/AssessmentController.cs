using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using OpenEug.TenTrees.Module.Assessment.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenEug.TenTrees.Module.Assessment.Controllers
{
    [Route("api/[controller]")]
    public class AssessmentController : Controller
    {
        private readonly IAssessmentService _assessmentService;
        private readonly ILogManager _logger;

        public AssessmentController(IAssessmentService assessmentService, ILogManager logger)
        {
            _assessmentService = assessmentService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Models.Assessment>>> Get()
        {
            try
            {
                var assessments = await _assessmentService.GetAssessmentsAsync();
                return Ok(assessments);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Assessment Get Failed {Error}", ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Models.Assessment>> Get(int id)
        {
            try
            {
                var assessment = await _assessmentService.GetAssessmentAsync(id);
                if (assessment == null)
                {
                    return NotFound();
                }

                return Ok(assessment);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Assessment Get Failed {AssessmentId} {Error}", id, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("grower/{growerId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Models.Assessment>>> GetByGrower(int growerId)
        {
            try
            {
                var assessments = await _assessmentService.GetAssessmentsByGrowerAsync(growerId);
                return Ok(assessments);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Assessment Get By Grower Failed {GrowerId} {Error}", growerId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Models.Assessment>> Post([FromBody] Models.Assessment assessment)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.Log(LogLevel.Error, this, LogFunction.Create, "Assessment Add Failed Validation {Assessment}", assessment);
                    return BadRequest(ModelState);
                }

                var created = await _assessmentService.AddAssessmentAsync(assessment);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "Assessment Added {Assessment}", created);
                return Ok(created);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Create, "Assessment Add Failed {Assessment} {Error}", assessment, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<Models.Assessment>> Put(int id, [FromBody] Models.Assessment assessment)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.Log(LogLevel.Error, this, LogFunction.Update, "Assessment Update Failed Validation {Assessment}", assessment);
                    return BadRequest(ModelState);
                }

                if (assessment == null || assessment.AssessmentId != id)
                {
                    _logger.Log(LogLevel.Error, this, LogFunction.Update, "Assessment Update Failed Id Mismatch {RouteId} {AssessmentId}", id, assessment?.AssessmentId);
                    return BadRequest();
                }

                var updated = await _assessmentService.UpdateAssessmentAsync(assessment);
                if (updated == null)
                {
                    return NotFound();
                }

                _logger.Log(LogLevel.Information, this, LogFunction.Update, "Assessment Updated {Assessment}", updated);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Update, "Assessment Update Failed {AssessmentId} {Error}", id, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _assessmentService.DeleteAssessmentAsync(id);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Assessment Deleted {AssessmentId}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Delete, "Assessment Delete Failed {AssessmentId} {Error}", id, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("can-submit/{growerId}")]
        [Authorize]
        public async Task<ActionResult<bool>> CanSubmit(int growerId)
        {
            try
            {
                var canSubmit = await _assessmentService.CanSubmitAssessmentAsync(growerId);
                return Ok(canSubmit);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Assessment CanSubmit Failed {GrowerId} {Error}", growerId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
