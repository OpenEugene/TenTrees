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
        public AssessmentService(HttpClient http, SiteState siteState) : base(http, siteState) { }

        private string ApiUrl => CreateApiUrl("Assessment");

        public async Task<Models.Assessment> GetAssessmentAsync(int assessmentId)
        {
            return await GetJsonAsync<Models.Assessment>($"{ApiUrl}/{assessmentId}");
        }

        public async Task<List<Models.Assessment>> GetAssessmentsAsync()
        {
            return await GetJsonAsync<List<Models.Assessment>>($"{ApiUrl}");
        }

        public async Task<List<Models.Assessment>> GetAssessmentsByGrowerAsync(int growerId)
        {
            return await GetJsonAsync<List<Models.Assessment>>($"{ApiUrl}/grower/{growerId}");
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
    }
}
