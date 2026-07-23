using System.Collections.Generic;
using System.Threading.Tasks;
using OpenEug.TenTrees.Models;

namespace OpenEug.TenTrees.Module.Cohort.Services
{
    public interface ICohortService
    {
        Task<List<Models.Cohort>> GetCohortsAsync();
        Task<List<Models.Cohort>> GetCohortsByVillageAsync(int villageId);
        Task<Models.Cohort> GetCohortAsync(int cohortId);
        Task<string> GetSuggestedNameAsync(int villageId, int year);
        Task<Models.Cohort> AddCohortAsync(Models.Cohort cohort);
        Task<Models.Cohort> UpdateCohortAsync(Models.Cohort cohort);
        Task DeleteCohortAsync(int cohortId);

        Task<List<Models.GrowerCohort>> GetGrowerCohortsAsync(int cohortId);
        Task<List<Models.Cohort>> GetCohortsByGrowerAsync(int growerId);
        Task<Models.GrowerCohort> AddGrowerToCohortAsync(int cohortId, int growerId);
        Task RemoveGrowerFromCohortAsync(int cohortId, int growerId);

        Task<List<Models.MentorCohort>> GetMentorCohortsAsync(int cohortId);
        Task<List<Models.Cohort>> GetCohortsByMentorAsync(string mentorId);
        Task<Models.MentorCohort> AssignMentorToCohortAsync(int cohortId, string mentorId);
        Task RemoveMentorFromCohortAsync(int cohortId, string mentorId);

        Task<List<Models.CohortClass>> GetClassesForCohortAsync(int cohortId);
        Task<List<Models.CohortClass>> GetCohortsForClassAsync(int trainingClassId);
        Task<Models.CohortClass> AddCohortClassAsync(int cohortId, int trainingClassId);
        Task RemoveCohortClassAsync(int cohortId, int trainingClassId);
    }
}
