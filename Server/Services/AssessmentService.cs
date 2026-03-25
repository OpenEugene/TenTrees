using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Shared;
using OpenEug.TenTrees.Module.Assessment.Repository;
using OpenEug.TenTrees.Models;
using OpenEug.TenTrees.Module.Grower.Repository;
using OpenEug.TenTrees.Module.Cohort.Repository;

namespace OpenEug.TenTrees.Module.Assessment.Services
{
    public class ServerAssessmentService : IAssessmentService
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IGrowerRepository _growerRepository;
        private readonly ICohortRepository _cohortRepository;
        private readonly ILogManager _logger;

        public ServerAssessmentService(
            IAssessmentRepository assessmentRepository,
            IGrowerRepository growerRepository,
            ICohortRepository cohortRepository,
            ILogManager logger)
        {
            _assessmentRepository = assessmentRepository;
            _growerRepository = growerRepository;
            _cohortRepository = cohortRepository;
            _logger = logger;
        }

        public Task<Models.Assessment> GetAssessmentAsync(int assessmentId)
        {
            return Task.FromResult(_assessmentRepository.GetAssessment(assessmentId));
        }

        public Task<List<Models.Assessment>> GetAssessmentsAsync()
        {
            var list = _assessmentRepository.GetAssessments().ToList();
            return Task.FromResult(list);
        }

        public Task<List<Models.Assessment>> GetAssessmentsByGrowerAsync(int growerId)
        {
            return Task.FromResult(_assessmentRepository.GetAssessmentsByGrower(growerId).ToList());
        }

        public Task<Models.Assessment> AddAssessmentAsync(Models.Assessment assessment)
        {
            var grower = _growerRepository.GetGrower(assessment.GrowerId);
            if (grower == null)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Create, "Grower not found {GrowerId}", assessment.GrowerId);
                return Task.FromResult<Models.Assessment>(null);
            }

            // Calculate Permaculture Principles Count
            assessment.PermaculturePrinciplesCount = 0;
            if (assessment.TreesLookHealthy) assessment.PermaculturePrinciplesCount++;
            if (!assessment.HasChemicalFertilizers) assessment.PermaculturePrinciplesCount++;
            if (!assessment.HasPesticides) assessment.PermaculturePrinciplesCount++;
            if (assessment.IsMulched) assessment.PermaculturePrinciplesCount++;
            if (assessment.IsMakingCompost) assessment.PermaculturePrinciplesCount++;
            if (assessment.IsCollectingWater) assessment.PermaculturePrinciplesCount++;
            if (!assessment.HasLeakyTaps) assessment.PermaculturePrinciplesCount++;
            if (assessment.IsGardenDesignedToCaptureWater) assessment.PermaculturePrinciplesCount++;
            if (assessment.IsUsingGreywater) assessment.PermaculturePrinciplesCount++;

            assessment = _assessmentRepository.AddAssessment(assessment);
            _logger.Log(LogLevel.Information, this, LogFunction.Create, "Assessment Added {Assessment}", assessment);
            return Task.FromResult(assessment);
        }

        private int CalculatePermaculturePrinciplesCount(Models.Assessment assessment)
        {
            var count = 0;

            if (assessment.TreesLookHealthy) count++;
            if (!assessment.HasChemicalFertilizers) count++;
            if (!assessment.HasPesticides) count++;
            if (assessment.IsMulched) count++;
            if (assessment.IsMakingCompost) count++;
            if (assessment.IsCollectingWater) count++;
            if (!assessment.HasLeakyTaps) count++;
            if (assessment.IsGardenDesignedToCaptureWater) count++;
            if (assessment.IsUsingGreywater) count++;

            return count;
        }

        public Task<Models.Assessment> UpdateAssessmentAsync(Models.Assessment assessment)
        {
            // Calculate Permaculture Principles Count
            assessment.PermaculturePrinciplesCount = CalculatePermaculturePrinciplesCount(assessment);

            assessment = _assessmentRepository.UpdateAssessment(assessment);
            _logger.Log(LogLevel.Information, this, LogFunction.Update, "Assessment Updated {Assessment}", assessment);
            return Task.FromResult(assessment);
        }

        public Task DeleteAssessmentAsync(int assessmentId)
        {
            _assessmentRepository.DeleteAssessment(assessmentId);
            _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Assessment Deleted {AssessmentId}", assessmentId);
            return Task.CompletedTask;
        }

        public Task<bool> CanSubmitAssessmentAsync(int growerId)
        {
            var grower = _growerRepository.GetGrower(growerId);
            if (grower == null || grower.Status != GrowerStatus.Active)
            {
                return Task.FromResult(false);
            }

            var assessments = _assessmentRepository.GetAssessmentsByGrower(growerId).OrderByDescending(a => a.AssessmentDate).ToList();
            if (!assessments.Any())
            {
                return Task.FromResult(true);
            }

            var lastAssessment = assessments.First();
            var daysSinceLast = (DateTime.UtcNow - lastAssessment.AssessmentDate).TotalDays;

            // Determine frequency from cohort: use the most recently activated cohort for this grower.
            // Year 1 (activated this year) → twice monthly (14 days).
            // Year 2+ → monthly (30 days).
            var cohorts = _cohortRepository.GetCohortsByGrower(growerId).ToList();
            var mostRecentActivation = cohorts
                .Where(c => c.ActivatedOn.HasValue)
                .OrderByDescending(c => c.ActivatedOn)
                .Select(c => c.ActivatedOn.Value)
                .FirstOrDefault();

            int minDays;
            if (mostRecentActivation != default && (DateTime.UtcNow - mostRecentActivation).TotalDays <= 365)
                minDays = 14; // Year 1 cohort — twice monthly
            else
                minDays = 30; // Year 2+ or no cohort assigned — monthly

            return Task.FromResult(daysSinceLast >= minDays);
        }

        public Task<List<AssessmentListDto>> GetAssessmentListAsync(int? villageId = null, int? cohortId = null, string mentorUsername = null, int? growerId = null)
        {
            var list = _assessmentRepository.GetAssessmentList(villageId, cohortId, mentorUsername, growerId).ToList();
            return Task.FromResult(list);
        }
    }
}
