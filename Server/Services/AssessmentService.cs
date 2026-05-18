using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Shared;
using OpenEug.TenTrees.Module.Assessment.Repository;
using OpenEug.TenTrees.Models;
using OpenEug.TenTrees.Module.Grower.Repository;
using OpenEug.TenTrees.Module.Cohort.Repository;
using OpenEug.TenTrees.Shared;

namespace OpenEug.TenTrees.Module.Assessment.Services
{
    public class ServerAssessmentService : IAssessmentService
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IAssessmentNoteRepository _assessmentNoteRepository;
        private readonly IGrowerRepository _growerRepository;
        private readonly ICohortRepository _cohortRepository;
        private readonly ILogManager _logger;
        private readonly IHttpContextAccessor _accessor;

        public ServerAssessmentService(
            IAssessmentRepository assessmentRepository,
            IAssessmentNoteRepository assessmentNoteRepository,
            IGrowerRepository growerRepository,
            ICohortRepository cohortRepository,
            ILogManager logger,
            IHttpContextAccessor accessor)
        {
            _assessmentRepository = assessmentRepository;
            _assessmentNoteRepository = assessmentNoteRepository;
            _growerRepository = growerRepository;
            _cohortRepository = cohortRepository;
            _logger = logger;
            _accessor = accessor;
        }

        public Task<Models.Assessment> GetAssessmentAsync(int assessmentId)
        {
            var assessment = _assessmentRepository.GetAssessment(assessmentId);
            if (assessment != null && IsMentor())
            {
                var grower = _growerRepository.GetGrower(assessment.GrowerId);
                if (grower == null || grower.MentorUsername != CurrentUsername())
                    return Task.FromResult<Models.Assessment>(null);
            }
            return Task.FromResult(assessment);
        }

        public Task<List<Models.Assessment>> GetAssessmentsAsync()
        {
            var list = _assessmentRepository.GetAssessments().ToList();
            if (IsMentor())
            {
                var mentorGrowerIds = _growerRepository.GetGrowersByMentor(CurrentUsername())
                    .Select(g => g.GrowerId).ToHashSet();
                list = list.Where(a => mentorGrowerIds.Contains(a.GrowerId)).ToList();
            }
            return Task.FromResult(list);
        }

        public Task<List<Models.Assessment>> GetAssessmentsByGrowerAsync(int growerId)
        {
            if (IsMentor())
            {
                var grower = _growerRepository.GetGrower(growerId);
                if (grower == null || grower.MentorUsername != CurrentUsername())
                    return Task.FromResult(new List<Models.Assessment>());
            }
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
            if (IsMentor())
                mentorUsername = CurrentUsername();
            var list = _assessmentRepository.GetAssessmentList(villageId, cohortId, mentorUsername, growerId).ToList();
            return Task.FromResult(list);
        }

        public Task<List<Models.AssessmentNote>> GetNotesByAssessmentAsync(int assessmentId)
        {
            return Task.FromResult(_assessmentNoteRepository.GetNotesByAssessment(assessmentId).ToList());
        }

        public Task<List<Models.AssessmentNote>> GetNotesByGrowerAsync(int growerId)
        {
            return Task.FromResult(_assessmentNoteRepository.GetNotesByGrower(growerId).ToList());
        }

        public Task<Models.AssessmentNote> AddNoteAsync(Models.AssessmentNote note)
        {
            if (note == null)
            {
                return Task.FromResult<Models.AssessmentNote>(null);
            }

            // Normalise note type to a known value so we don't end up with
            // arbitrary strings in the database from a malformed client.
            if (note.NoteType != AssessmentNoteTypes.HomeVisit)
            {
                note.NoteType = AssessmentNoteTypes.General;
            }

            var assessment = _assessmentRepository.GetAssessment(note.AssessmentId, tracking: false);
            if (assessment == null)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Create, "AssessmentNote Add Failed — Assessment not found {AssessmentId}", note.AssessmentId);
                return Task.FromResult<Models.AssessmentNote>(null);
            }

            var created = _assessmentNoteRepository.AddNote(note);
            _logger.Log(LogLevel.Information, this, LogFunction.Create, "AssessmentNote Added {AssessmentNote}", created);
            return Task.FromResult(created);
        }

        private bool IsMentor() =>
            _accessor.HttpContext.User.IsInRole(AppRoleNames.Mentor);

        private string CurrentUsername() =>
            _accessor.HttpContext.User.Identity?.Name;

        public Task<List<Models.AssessmentProblem>> GetProblemsByAssessmentAsync(int assessmentId)
        {
            return Task.FromResult(_assessmentRepository.GetProblemsByAssessment(assessmentId).ToList());
        }

        public Task ReplaceProblemsAsync(int assessmentId, List<Models.AssessmentProblem> problems)
        {
            foreach (var p in problems)
                p.AssessmentId = assessmentId;
            _assessmentRepository.ReplaceProblems(assessmentId, problems);
            return Task.CompletedTask;
        }
    }
}
