using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Controllers;
using OpenEug.TenTrees.Module.Assessment.Services;
using OpenEug.TenTrees.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenEug.TenTrees.Module.Assessment.Controllers
{
    [Route(ControllerRoutes.ApiRoute)]
    public class AssessmentController : ModuleControllerBase
    {
        private readonly IAssessmentService _assessmentService;

        public AssessmentController(IAssessmentService assessmentService, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _assessmentService = assessmentService;
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

        [HttpGet("list")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<AssessmentListDto>>> GetList([FromQuery] int? villageId = null, [FromQuery] int? cohortId = null, [FromQuery] string mentor = null, [FromQuery] int? growerId = null)
        {
            try
            {
                var list = await _assessmentService.GetAssessmentListAsync(villageId, cohortId, mentor, growerId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Assessment GetList Failed {Error}", ex.ToString());
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

        [HttpGet("{id}/notes")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Models.AssessmentNote>>> GetNotes(int id)
        {
            try
            {
                var notes = await _assessmentService.GetNotesByAssessmentAsync(id);
                return Ok(notes);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "AssessmentNote Get Failed {AssessmentId} {Error}", id, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("grower/{growerId}/notes")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Models.AssessmentNote>>> GetNotesByGrower(int growerId)
        {
            try
            {
                var notes = await _assessmentService.GetNotesByGrowerAsync(growerId);
                return Ok(notes);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "AssessmentNote Get By Grower Failed {GrowerId} {Error}", growerId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("{id}/notes")]
        [Authorize]
        public async Task<ActionResult<Models.AssessmentNote>> PostNote(int id, [FromBody] Models.AssessmentNote note)
        {
            try
            {
                if (note == null || note.AssessmentId != id)
                {
                    _logger.Log(LogLevel.Error, this, LogFunction.Create, "AssessmentNote Add Failed Id Mismatch {RouteId} {AssessmentId}", id, note?.AssessmentId);
                    return BadRequest();
                }

                if (string.IsNullOrWhiteSpace(note.Text))
                {
                    _logger.Log(LogLevel.Error, this, LogFunction.Create, "AssessmentNote Add Failed Empty Text {AssessmentId}", id);
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                {
                    _logger.Log(LogLevel.Error, this, LogFunction.Create, "AssessmentNote Add Failed Validation {AssessmentNote}", note);
                    return BadRequest(ModelState);
                }

                var created = await _assessmentService.AddNoteAsync(note);
                if (created == null)
                {
                    return NotFound();
                }

                _logger.Log(LogLevel.Information, this, LogFunction.Create, "AssessmentNote Added {AssessmentNote}", created);
                return Ok(created);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Create, "AssessmentNote Add Failed {AssessmentId} {Error}", id, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}/problems")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Models.AssessmentProblem>>> GetProblems(int id)
        {
            try
            {
                var problems = await _assessmentService.GetProblemsByAssessmentAsync(id);
                return Ok(problems);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "AssessmentProblem Get Failed {AssessmentId} {Error}", id, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}/problems")]
        [Authorize]
        public async Task<ActionResult> PutProblems(int id, [FromBody] List<Models.AssessmentProblem> problems)
        {
            try
            {
                if (problems == null)
                    return BadRequest();

                await _assessmentService.ReplaceProblemsAsync(id, problems);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Update, "AssessmentProblem Replace Failed {AssessmentId} {Error}", id, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
