using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Models = OpenEug.TenTrees.Models;
using Oqtane.Controllers;
using System.Threading.Tasks;
using OpenEug.TenTrees.Module.Cohort.Services;
using System;

namespace OpenEug.TenTrees.Module.Cohort.Controllers
{
    [Route(ControllerRoutes.ApiRoute)]
    public class CohortController : ModuleControllerBase
    {
        private readonly ICohortService _cohortService;

        public CohortController(ICohortService cohortService, ILogManager logger, IHttpContextAccessor accessor)
            : base(logger, accessor)
        {
            _cohortService = cohortService;
        }

        // GET: api/<controller>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Models.Cohort>>> Get()
        {
            try
            {
                var cohorts = await _cohortService.GetCohortsAsync();
                return Ok(cohorts);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Cohort Get Failed {Error}", ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Models.Cohort>> Get(int id)
        {
            try
            {
                var cohort = await _cohortService.GetCohortAsync(id);
                if (cohort == null)
                {
                    _logger.Log(LogLevel.Warning, this, LogFunction.Read, "Cohort Not Found {CohortId}", id);
                    return NotFound();
                }

                return Ok(cohort);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Cohort Get Failed {CohortId} {Error}", id, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET api/<controller>/village/5
        [HttpGet("village/{villageId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Models.Cohort>>> GetByVillage(int villageId)
        {
            try
            {
                var cohorts = await _cohortService.GetCohortsByVillageAsync(villageId);
                return Ok(cohorts);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Cohort GetByVillage Failed {VillageId} {Error}", villageId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET api/<controller>/suggest?villageId=1&year=2026
        [HttpGet("suggest")]
        [Authorize]
        public async Task<ActionResult<CohortNameSuggestion>> Suggest([FromQuery] int villageId, [FromQuery] int year)
        {
            try
            {
                var suggestion = await _cohortService.SuggestCohortNameAsync(villageId, year);
                return Ok(new CohortNameSuggestion { Name = suggestion });
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Cohort Suggest Failed {VillageId} {Year} {Error}", villageId, year, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult<Models.Cohort>> Post([FromBody] Models.Cohort cohort)
        {
            if (!ModelState.IsValid)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Create, "Cohort Post Failed Validation {Cohort}", cohort);
                return BadRequest(ModelState);
            }

            try
            {
                var createdCohort = await _cohortService.AddCohortAsync(cohort);
                return Ok(createdCohort);
            }
            catch (InvalidOperationException ex)
            {
                _logger.Log(LogLevel.Warning, this, LogFunction.Create, "Invalid Cohort Post Attempt {Error}", ex.Message);
                return Conflict();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Create, "Cohort Post Failed {Cohort} {Error}", cohort, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult<Models.Cohort>> Put(int id, [FromBody] Models.Cohort cohort)
        {
            if (!ModelState.IsValid)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Update, "Cohort Put Failed Validation {Cohort}", cohort);
                return BadRequest(ModelState);
            }

            if (cohort == null || cohort.CohortId != id)
            {
                return BadRequest();
            }

            try
            {
                var updated = await _cohortService.UpdateCohortAsync(cohort);
                if (updated == null)
                {
                    return NotFound();
                }

                return Ok(updated);
            }
            catch (InvalidOperationException ex)
            {
                _logger.Log(LogLevel.Warning, this, LogFunction.Update, "Invalid Cohort Put Attempt {Error}", ex.Message);
                return Conflict();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Update, "Cohort Put Failed {CohortId} {Error}", id, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _cohortService.DeleteCohortAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Delete, "Cohort Delete Failed {CohortId} {Error}", id, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // ── Grower membership ──────────────────────────────────────────────────

        // GET api/<controller>/5/growers
        [HttpGet("{id}/growers")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Models.GrowerCohort>>> GetGrowerCohorts(int id)
        {
            try
            {
                var growerCohorts = await _cohortService.GetGrowerCohortsAsync(id);
                return Ok(growerCohorts);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Cohort GetGrowerCohorts Failed {CohortId} {Error}", id, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET api/<controller>/grower/5
        [HttpGet("grower/{growerId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Models.Cohort>>> GetByGrower(int growerId)
        {
            try
            {
                var cohorts = await _cohortService.GetCohortsByGrowerAsync(growerId);
                return Ok(cohorts);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Cohort GetByGrower Failed {GrowerId} {Error}", growerId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST api/<controller>/5/growers/7
        [HttpPost("{id}/growers/{growerId}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult<Models.GrowerCohort>> AddGrowerCohort(int id, int growerId)
        {
            try
            {
                var created = await _cohortService.AddGrowerCohortAsync(growerId, id);
                return Ok(created);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Create, "Cohort AddGrower Failed {CohortId} {GrowerId} {Error}", id, growerId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE api/<controller>/5/growers/7
        [HttpDelete("{id}/growers/{growerId}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult> DeleteGrowerCohort(int id, int growerId)
        {
            try
            {
                await _cohortService.DeleteGrowerCohortAsync(growerId, id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Delete, "Cohort DeleteGrower Failed {CohortId} {GrowerId} {Error}", id, growerId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // ── Mentor assignment ──────────────────────────────────────────────────

        // GET api/<controller>/5/mentors
        [HttpGet("{id}/mentors")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Models.MentorCohort>>> GetMentorCohorts(int id)
        {
            try
            {
                var mentorCohorts = await _cohortService.GetMentorCohortsAsync(id);
                return Ok(mentorCohorts);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Cohort GetMentorCohorts Failed {CohortId} {Error}", id, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET api/<controller>/mentor/abc123
        [HttpGet("mentor/{mentorId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Models.Cohort>>> GetByMentor(string mentorId)
        {
            try
            {
                var cohorts = await _cohortService.GetCohortsByMentorAsync(mentorId);
                return Ok(cohorts);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Cohort GetByMentor Failed {MentorId} {Error}", mentorId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST api/<controller>/5/mentors/abc123
        [HttpPost("{id}/mentors/{mentorId}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult<Models.MentorCohort>> AddMentorCohort(int id, string mentorId)
        {
            try
            {
                var created = await _cohortService.AddMentorCohortAsync(mentorId, id);
                return Ok(created);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Create, "Cohort AddMentor Failed {CohortId} {MentorId} {Error}", id, mentorId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE api/<controller>/5/mentors/abc123
        [HttpDelete("{id}/mentors/{mentorId}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult> DeleteMentorCohort(int id, string mentorId)
        {
            try
            {
                await _cohortService.DeleteMentorCohortAsync(mentorId, id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Delete, "Cohort DeleteMentor Failed {CohortId} {MentorId} {Error}", id, mentorId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // ── Class association ──────────────────────────────────────────────────

        // GET api/<controller>/5/classes
        [HttpGet("{id}/classes")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Models.CohortClass>>> GetClassesForCohort(int id)
        {
            try
            {
                var classes = await _cohortService.GetClassesForCohortAsync(id);
                return Ok(classes);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Cohort GetClasses Failed {CohortId} {Error}", id, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET api/<controller>/class/7/cohorts
        [HttpGet("class/{classId}/cohorts")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Models.CohortClass>>> GetCohortsForClass(int classId)
        {
            try
            {
                var cohorts = await _cohortService.GetCohortsForClassAsync(classId);
                return Ok(cohorts);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Cohort GetCohortsForClass Failed {ClassId} {Error}", classId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST api/<controller>/5/classes/7
        [HttpPost("{id}/classes/{classId}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult<Models.CohortClass>> AddCohortClass(int id, int classId)
        {
            try
            {
                var created = await _cohortService.AddCohortClassAsync(id, classId);
                return Ok(created);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Create, "Cohort AddClass Failed {CohortId} {ClassId} {Error}", id, classId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE api/<controller>/5/classes/7
        [HttpDelete("{id}/classes/{classId}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult> DeleteCohortClass(int id, int classId)
        {
            try
            {
                await _cohortService.DeleteCohortClassAsync(id, classId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Delete, "Cohort DeleteClass Failed {CohortId} {ClassId} {Error}", id, classId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }

    public class CohortNameSuggestion
    {
        public string Name { get; set; }
    }
}
