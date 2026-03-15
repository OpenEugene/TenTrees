using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Models = OpenEug.TenTrees.Models;
using Oqtane.Controllers;
using System.Net;
using System.Threading.Tasks;
using OpenEug.TenTrees.Module.Cohort.Services;

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
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<IEnumerable<Models.Cohort>> Get()
            => await _cohortService.GetCohortsAsync();

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<Models.Cohort> Get(int id)
        {
            var cohort = await _cohortService.GetCohortAsync(id);
            if (cohort != null) return cohort;

            _logger.Log(LogLevel.Warning, this, LogFunction.Security, "Cohort Not Found {CohortId}", id);
            HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return null;
        }

        // GET api/<controller>/village/5
        [HttpGet("village/{villageId}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<IEnumerable<Models.Cohort>> GetByVillage(int villageId)
            => await _cohortService.GetCohortsByVillageAsync(villageId);

        // GET api/<controller>/suggest?villageId=1&year=2026
        [HttpGet("suggest")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<string> Suggest([FromQuery] int villageId, [FromQuery] int year)
            => await _cohortService.SuggestCohortNameAsync(villageId, year);

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<Models.Cohort> Post([FromBody] Models.Cohort cohort)
        {
            if (!ModelState.IsValid)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Cohort Post Attempt {Cohort}", cohort);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
            return await _cohortService.AddCohortAsync(cohort);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<Models.Cohort> Put(int id, [FromBody] Models.Cohort cohort)
        {
            if (!ModelState.IsValid || cohort.CohortId != id)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Cohort Put Attempt {Cohort}", cohort);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
            return await _cohortService.UpdateCohortAsync(cohort);
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task Delete(int id)
            => await _cohortService.DeleteCohortAsync(id);

        // ── Grower membership ──────────────────────────────────────────────────

        // GET api/<controller>/5/growers
        [HttpGet("{id}/growers")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<IEnumerable<Models.GrowerCohort>> GetGrowerCohorts(int id)
            => await _cohortService.GetGrowerCohortsAsync(id);

        // GET api/<controller>/grower/5
        [HttpGet("grower/{growerId}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<IEnumerable<Models.Cohort>> GetByGrower(int growerId)
            => await _cohortService.GetCohortsByGrowerAsync(growerId);

        // POST api/<controller>/5/growers/7
        [HttpPost("{id}/growers/{growerId}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<Models.GrowerCohort> AddGrowerCohort(int id, int growerId)
            => await _cohortService.AddGrowerCohortAsync(growerId, id);

        // DELETE api/<controller>/5/growers/7
        [HttpDelete("{id}/growers/{growerId}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task DeleteGrowerCohort(int id, int growerId)
            => await _cohortService.DeleteGrowerCohortAsync(growerId, id);

        // ── Mentor assignment ──────────────────────────────────────────────────

        // GET api/<controller>/5/mentors
        [HttpGet("{id}/mentors")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<IEnumerable<Models.MentorCohort>> GetMentorCohorts(int id)
            => await _cohortService.GetMentorCohortsAsync(id);

        // GET api/<controller>/mentor/abc123
        [HttpGet("mentor/{mentorId}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<IEnumerable<Models.Cohort>> GetByMentor(string mentorId)
            => await _cohortService.GetCohortsByMentorAsync(mentorId);

        // POST api/<controller>/5/mentors/abc123
        [HttpPost("{id}/mentors/{mentorId}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<Models.MentorCohort> AddMentorCohort(int id, string mentorId)
            => await _cohortService.AddMentorCohortAsync(mentorId, id);

        // DELETE api/<controller>/5/mentors/abc123
        [HttpDelete("{id}/mentors/{mentorId}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task DeleteMentorCohort(int id, string mentorId)
            => await _cohortService.DeleteMentorCohortAsync(mentorId, id);

        // ── Class association ──────────────────────────────────────────────────

        // GET api/<controller>/5/classes
        [HttpGet("{id}/classes")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<IEnumerable<Models.CohortClass>> GetClassesForCohort(int id)
            => await _cohortService.GetClassesForCohortAsync(id);

        // GET api/<controller>/class/7/cohorts
        [HttpGet("class/{classId}/cohorts")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<IEnumerable<Models.CohortClass>> GetCohortsForClass(int classId)
            => await _cohortService.GetCohortsForClassAsync(classId);

        // POST api/<controller>/5/classes/7
        [HttpPost("{id}/classes/{classId}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<Models.CohortClass> AddCohortClass(int id, int classId)
            => await _cohortService.AddCohortClassAsync(id, classId);

        // DELETE api/<controller>/5/classes/7
        [HttpDelete("{id}/classes/{classId}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task DeleteCohortClass(int id, int classId)
            => await _cohortService.DeleteCohortClassAsync(id, classId);
    }
}
