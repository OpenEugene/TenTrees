using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Oqtane.Services;
using Oqtane.Shared;
using Models = OpenEug.TenTrees.Models;

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

    public class CohortService : ServiceBase, ICohortService
    {
        public CohortService(HttpClient http, SiteState siteState) : base(http, siteState) { }

        private string Apiurl => CreateApiUrl("Cohort");

        public async Task<List<Models.Cohort>> GetCohortsAsync()
            => await GetJsonAsync<List<Models.Cohort>>(Apiurl) ?? new();

        public async Task<List<Models.Cohort>> GetCohortsByVillageAsync(int villageId)
            => await GetJsonAsync<List<Models.Cohort>>($"{Apiurl}/village/{villageId}") ?? new();

        public async Task<Models.Cohort> GetCohortAsync(int cohortId)
            => await GetJsonAsync<Models.Cohort>($"{Apiurl}/{cohortId}");

        public async Task<string> GetSuggestedNameAsync(int villageId, int year)
            => await GetJsonAsync<string>($"{Apiurl}/suggest?villageId={villageId}&year={year}") ?? string.Empty;

        public async Task<Models.Cohort> AddCohortAsync(Models.Cohort cohort)
            => await PostJsonAsync<Models.Cohort>(Apiurl, cohort);

        public async Task<Models.Cohort> UpdateCohortAsync(Models.Cohort cohort)
            => await PutJsonAsync<Models.Cohort>($"{Apiurl}/{cohort.CohortId}", cohort);

        public async Task DeleteCohortAsync(int cohortId)
            => await DeleteAsync($"{Apiurl}/{cohortId}");

        // ── Grower membership ──────────────────────────────────────────────────

        public async Task<List<Models.GrowerCohort>> GetGrowerCohortsAsync(int cohortId)
            => await GetJsonAsync<List<Models.GrowerCohort>>($"{Apiurl}/{cohortId}/growers") ?? new();

        public async Task<List<Models.Cohort>> GetCohortsByGrowerAsync(int growerId)
            => await GetJsonAsync<List<Models.Cohort>>($"{Apiurl}/grower/{growerId}") ?? new();

        public async Task<Models.GrowerCohort> AddGrowerToCohortAsync(int cohortId, int growerId)
            => await PostJsonAsync<Models.GrowerCohort>($"{Apiurl}/{cohortId}/growers/{growerId}", null);

        public async Task RemoveGrowerFromCohortAsync(int cohortId, int growerId)
            => await DeleteAsync($"{Apiurl}/{cohortId}/growers/{growerId}");

        // ── Mentor assignment ──────────────────────────────────────────────────

        public async Task<List<Models.MentorCohort>> GetMentorCohortsAsync(int cohortId)
            => await GetJsonAsync<List<Models.MentorCohort>>($"{Apiurl}/{cohortId}/mentors") ?? new();

        public async Task<List<Models.Cohort>> GetCohortsByMentorAsync(string mentorId)
            => await GetJsonAsync<List<Models.Cohort>>($"{Apiurl}/mentor/{mentorId}") ?? new();

        public async Task<Models.MentorCohort> AssignMentorToCohortAsync(int cohortId, string mentorId)
            => await PostJsonAsync<Models.MentorCohort>($"{Apiurl}/{cohortId}/mentors/{mentorId}", null);

        public async Task RemoveMentorFromCohortAsync(int cohortId, string mentorId)
            => await DeleteAsync($"{Apiurl}/{cohortId}/mentors/{mentorId}");

        // ── Class association ──────────────────────────────────────────────────

        public async Task<List<Models.CohortClass>> GetClassesForCohortAsync(int cohortId)
            => await GetJsonAsync<List<Models.CohortClass>>($"{Apiurl}/{cohortId}/classes") ?? new();

        public async Task<List<Models.CohortClass>> GetCohortsForClassAsync(int trainingClassId)
            => await GetJsonAsync<List<Models.CohortClass>>($"{Apiurl}/class/{trainingClassId}/cohorts") ?? new();

        public async Task<Models.CohortClass> AddCohortClassAsync(int cohortId, int trainingClassId)
            => await PostJsonAsync<Models.CohortClass>($"{Apiurl}/{cohortId}/classes/{trainingClassId}", null);

        public async Task RemoveCohortClassAsync(int cohortId, int trainingClassId)
            => await DeleteAsync($"{Apiurl}/{cohortId}/classes/{trainingClassId}");
    }
}
