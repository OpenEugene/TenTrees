using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Oqtane.Infrastructure;
using Oqtane.Enums;
using Models = OpenEug.TenTrees.Models;
using OpenEug.TenTrees.Module.Cohort.Repository;
using OpenEug.TenTrees.Module.Village.Repository;

namespace OpenEug.TenTrees.Module.Cohort.Services
{
    public interface ICohortService
    {
        Task<List<Models.Cohort>> GetCohortsAsync();
        Task<List<Models.Cohort>> GetCohortsByVillageAsync(int villageId);
        Task<Models.Cohort> GetCohortAsync(int cohortId);
        Task<string> SuggestCohortNameAsync(int villageId, int year);
        Task<Models.Cohort> AddCohortAsync(Models.Cohort cohort);
        Task<Models.Cohort> UpdateCohortAsync(Models.Cohort cohort);
        Task DeleteCohortAsync(int cohortId);

        Task<List<Models.GrowerCohort>> GetGrowerCohortsAsync(int cohortId);
        Task<List<Models.Cohort>> GetCohortsByGrowerAsync(int growerId);
        Task<Models.GrowerCohort> AddGrowerCohortAsync(int growerId, int cohortId);
        Task DeleteGrowerCohortAsync(int growerId, int cohortId);

        Task<List<Models.MentorCohort>> GetMentorCohortsAsync(int cohortId);
        Task<List<Models.Cohort>> GetCohortsByMentorAsync(string mentorId);
        Task<Models.MentorCohort> AddMentorCohortAsync(string mentorId, int cohortId);
        Task DeleteMentorCohortAsync(string mentorId, int cohortId);

        Task<List<Models.CohortClass>> GetClassesForCohortAsync(int cohortId);
        Task<List<Models.CohortClass>> GetCohortsForClassAsync(int trainingClassId);
        Task<Models.CohortClass> AddCohortClassAsync(int cohortId, int trainingClassId);
        Task DeleteCohortClassAsync(int cohortId, int trainingClassId);
    }

    public class ServerCohortService : ICohortService
    {
        private readonly ICohortRepository _cohortRepository;
        private readonly IVillageRepository _villageRepository;
        private readonly ILogManager _logger;

        public ServerCohortService(ICohortRepository cohortRepository, IVillageRepository villageRepository, ILogManager logger)
        {
            _cohortRepository = cohortRepository;
            _villageRepository = villageRepository;
            _logger = logger;
        }

        public Task<List<Models.Cohort>> GetCohortsAsync()
            => Task.FromResult(_cohortRepository.GetCohorts().ToList());

        public Task<List<Models.Cohort>> GetCohortsByVillageAsync(int villageId)
            => Task.FromResult(_cohortRepository.GetCohortsByVillage(villageId).ToList());

        public Task<Models.Cohort> GetCohortAsync(int cohortId)
            => Task.FromResult(_cohortRepository.GetCohort(cohortId));

        public Task<string> SuggestCohortNameAsync(int villageId, int year)
        {
            var village = _villageRepository.GetVillage(villageId);
            if (village == null) return Task.FromResult(string.Empty);

            var count = _cohortRepository.CountCohortsForVillageYear(villageId, year);
            var name = count == 0
                ? $"{village.VillageName} {year}"
                : $"{village.VillageName} {count + 1} {year}";

            return Task.FromResult(name);
        }

        public Task<Models.Cohort> AddCohortAsync(Models.Cohort cohort)
        {
            cohort = _cohortRepository.AddCohort(cohort);
            _logger.Log(LogLevel.Information, this, LogFunction.Create, "Cohort Added {Cohort}", cohort);
            return Task.FromResult(cohort);
        }

        public Task<Models.Cohort> UpdateCohortAsync(Models.Cohort cohort)
        {
            var existing = _cohortRepository.GetCohort(cohort.CohortId);
            if (existing != null && existing.Status == Models.CohortStatus.Completed && cohort.Status != Models.CohortStatus.Completed)
            {
                _logger.Log(LogLevel.Warning, this, LogFunction.Update, "Attempt to revert completed cohort {CohortId} rejected", cohort.CohortId);
                return Task.FromResult(existing);
            }

            // Set or preserve ActivatedOn for Active cohorts
            if (existing != null)
            {
                // When first transitioning to Active, set ActivatedOn if not provided
                if (existing.Status != Models.CohortStatus.Active && cohort.Status == Models.CohortStatus.Active && cohort.ActivatedOn == null)
                {
                    cohort.ActivatedOn = DateTime.UtcNow;
                }
                // When already Active, preserve existing ActivatedOn if the client omits it
                else if (existing.Status == Models.CohortStatus.Active && cohort.ActivatedOn == null)
                {
                    cohort.ActivatedOn = existing.ActivatedOn;
                }
            }

            cohort = _cohortRepository.UpdateCohort(cohort);
            _logger.Log(LogLevel.Information, this, LogFunction.Update, "Cohort Updated {Cohort}", cohort);
            return Task.FromResult(cohort);
        }

        public Task DeleteCohortAsync(int cohortId)
        {
            _cohortRepository.DeleteCohort(cohortId);
            _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Cohort Deleted {CohortId}", cohortId);
            return Task.CompletedTask;
        }

        // ── Grower membership ──────────────────────────────────────────────────

        public Task<List<Models.GrowerCohort>> GetGrowerCohortsAsync(int cohortId)
            => Task.FromResult(_cohortRepository.GetGrowerCohorts(cohortId).ToList());

        public Task<List<Models.Cohort>> GetCohortsByGrowerAsync(int growerId)
            => Task.FromResult(_cohortRepository.GetCohortsByGrower(growerId).ToList());

        public Task<Models.GrowerCohort> AddGrowerCohortAsync(int growerId, int cohortId)
        {
            var gc = _cohortRepository.AddGrowerCohort(growerId, cohortId);
            _logger.Log(LogLevel.Information, this, LogFunction.Create, "Grower {GrowerId} added to Cohort {CohortId}", growerId, cohortId);
            return Task.FromResult(gc);
        }

        public Task DeleteGrowerCohortAsync(int growerId, int cohortId)
        {
            _cohortRepository.DeleteGrowerCohort(growerId, cohortId);
            _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Grower {GrowerId} removed from Cohort {CohortId}", growerId, cohortId);
            return Task.CompletedTask;
        }

        // ── Mentor assignment ──────────────────────────────────────────────────

        public Task<List<Models.MentorCohort>> GetMentorCohortsAsync(int cohortId)
            => Task.FromResult(_cohortRepository.GetMentorCohorts(cohortId).ToList());

        public Task<List<Models.Cohort>> GetCohortsByMentorAsync(string mentorId)
            => Task.FromResult(_cohortRepository.GetCohortsByMentor(mentorId).ToList());

        public Task<Models.MentorCohort> AddMentorCohortAsync(string mentorId, int cohortId)
        {
            var mc = _cohortRepository.AddMentorCohort(mentorId, cohortId);
            _logger.Log(LogLevel.Information, this, LogFunction.Create, "Mentor {MentorId} assigned to Cohort {CohortId}", mentorId, cohortId);
            return Task.FromResult(mc);
        }

        public Task DeleteMentorCohortAsync(string mentorId, int cohortId)
        {
            _cohortRepository.DeleteMentorCohort(mentorId, cohortId);
            _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Mentor {MentorId} removed from Cohort {CohortId}", mentorId, cohortId);
            return Task.CompletedTask;
        }

        // ── Class association ──────────────────────────────────────────────────

        public Task<List<Models.CohortClass>> GetClassesForCohortAsync(int cohortId)
            => Task.FromResult(_cohortRepository.GetClassesForCohort(cohortId).ToList());

        public Task<List<Models.CohortClass>> GetCohortsForClassAsync(int trainingClassId)
            => Task.FromResult(_cohortRepository.GetCohortsForClass(trainingClassId).ToList());

        public Task<Models.CohortClass> AddCohortClassAsync(int cohortId, int trainingClassId)
        {
            var cc = _cohortRepository.AddCohortClass(cohortId, trainingClassId);
            _logger.Log(LogLevel.Information, this, LogFunction.Create, "Class {TrainingClassId} linked to Cohort {CohortId}", trainingClassId, cohortId);
            return Task.FromResult(cc);
        }

        public Task DeleteCohortClassAsync(int cohortId, int trainingClassId)
        {
            _cohortRepository.DeleteCohortClass(cohortId, trainingClassId);
            _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Class {TrainingClassId} unlinked from Cohort {CohortId}", trainingClassId, cohortId);
            return Task.CompletedTask;
        }
    }
}
