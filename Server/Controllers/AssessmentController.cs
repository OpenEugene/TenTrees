using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using OpenEug.TenTrees.Module.Assessment.Services;
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
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<IEnumerable<Models.Assessment>> Get(int moduleId)
        {
            return await _assessmentService.GetAssessmentsAsync(moduleId);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<Models.Assessment> Get(int id, int moduleId)
        {
            return await _assessmentService.GetAssessmentAsync(id, moduleId);
        }

        [HttpGet("grower/{growerId}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<IEnumerable<Models.Assessment>> GetByGrower(int growerId, int moduleId)
        {
            return await _assessmentService.GetAssessmentsByGrowerAsync(growerId, moduleId);
        }

        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<Models.Assessment> Post([FromBody] Models.Assessment assessment)
        {
            if (ModelState.IsValid)
            {
                assessment = await _assessmentService.AddAssessmentAsync(assessment);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "Assessment Added {Assessment}", assessment);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Create, "Assessment Add Failed {Assessment}", assessment);
            }
            return assessment;
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<Models.Assessment> Put(int id, [FromBody] Models.Assessment assessment)
        {
            if (ModelState.IsValid && assessment.AssessmentId == id)
            {
                assessment = await _assessmentService.UpdateAssessmentAsync(assessment);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "Assessment Updated {Assessment}", assessment);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Update, "Assessment Update Failed {Assessment}", assessment);
            }
            return assessment;
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task Delete(int id, int moduleId)
        {
            await _assessmentService.DeleteAssessmentAsync(id, moduleId);
            _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Assessment Deleted {AssessmentId}", id);
        }

        [HttpGet("can-submit/{growerId}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<bool> CanSubmit(int growerId, int moduleId)
        {
            return await _assessmentService.CanSubmitAssessmentAsync(growerId, moduleId);
        }
    }
}
