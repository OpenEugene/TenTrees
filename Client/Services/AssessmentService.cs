using Microsoft.Extensions.Logging;
using OpenEug.TenTrees.Models;
using Oqtane.Services;
using Oqtane.Shared;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenEug.TenTrees.Module.Assessment.Services
{
    public class AssessmentService : ServiceBase, IAssessmentService
    {
        private readonly ILogger<AssessmentService> _logger;

        public AssessmentService(HttpClient http, SiteState siteState, ILogger<AssessmentService> logger) : base(http, siteState)
        {
            _logger = logger;
        }

        private string ApiUrl => CreateApiUrl("Assessment");

        public async Task<Models.Assessment> GetAssessmentAsync(int assessmentId)
        {
            return await GetJsonAsync<Models.Assessment>($"{ApiUrl}/{assessmentId}");
        }

        public async Task<List<Models.Assessment>> GetAssessmentsAsync()
        {
            try
            {
                return await GetJsonAsync<List<Models.Assessment>>($"{ApiUrl}", new List<Models.Assessment>());
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "AssessmentService.GetAssessmentsAsync failed for url {ApiUrl}", ApiUrl);
                throw;
            }
        }

        public async Task<List<Models.Assessment>> GetAssessmentsByGrowerAsync(int growerId)
        {
            return await GetJsonAsync<List<Models.Assessment>>($"{ApiUrl}/grower/{growerId}", new List<Models.Assessment>());
        }

        public async Task<Models.Assessment> AddAssessmentAsync(Models.Assessment assessment)
        {
            return await PostJsonAsync<Models.Assessment>($"{ApiUrl}", assessment);
        }

        public async Task<Models.Assessment> UpdateAssessmentAsync(Models.Assessment assessment)
        {
            return await PutJsonAsync<Models.Assessment>($"{ApiUrl}/{assessment.AssessmentId}", assessment);
        }

        public async Task DeleteAssessmentAsync(int assessmentId)
        {
            await DeleteAsync($"{ApiUrl}/{assessmentId}");
        }

        public async Task<bool> CanSubmitAssessmentAsync(int growerId)
        {
            return await GetJsonAsync<bool>($"{ApiUrl}/can-submit/{growerId}");
        }

        public async Task<List<AssessmentListDto>> GetAssessmentListAsync(int? villageId = null, int? cohortId = null, string mentorUsername = null, int? growerId = null)
        {
            var url = $"{ApiUrl}/list";
            var queryParams = new List<string>();
            if (villageId.HasValue) queryParams.Add($"villageId={villageId.Value}");
            if (cohortId.HasValue) queryParams.Add($"cohortId={cohortId.Value}");
            if (!string.IsNullOrEmpty(mentorUsername)) queryParams.Add($"mentor={System.Uri.EscapeDataString(mentorUsername)}");
            if (growerId.HasValue) queryParams.Add($"growerId={growerId.Value}");
            if (queryParams.Count > 0) url += "?" + string.Join("&", queryParams);
            return await GetJsonAsync<List<AssessmentListDto>>(url, new List<AssessmentListDto>());
        }
    }
}
