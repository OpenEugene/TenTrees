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

        public async Task<Models.Assessment> GetAssessmentAsync(int assessmentId, int moduleId)
        {
            return await GetJsonAsync<Models.Assessment>(CreateAuthorizationPolicyUrl($"{ApiUrl}/{assessmentId}?moduleId={moduleId}", EntityNames.Module, moduleId));
        }

        public async Task<List<Models.Assessment>> GetAssessmentsAsync(int moduleId)
        {
            return await GetJsonAsync<List<Models.Assessment>>(CreateAuthorizationPolicyUrl($"{ApiUrl}?moduleId={moduleId}", EntityNames.Module, moduleId));
        }

        public async Task<List<Models.Assessment>> GetAssessmentsByGrowerAsync(int growerId, int moduleId)
        {
            return await GetJsonAsync<List<Models.Assessment>>(CreateAuthorizationPolicyUrl($"{ApiUrl}/grower/{growerId}?moduleId={moduleId}", EntityNames.Module, moduleId));
        }

        public async Task<Models.Assessment> AddAssessmentAsync(Models.Assessment assessment)
        {
            return await PostJsonAsync<Models.Assessment>(CreateAuthorizationPolicyUrl($"{ApiUrl}", EntityNames.Module, assessment.ModuleId), assessment);
        }

        public async Task<Models.Assessment> UpdateAssessmentAsync(Models.Assessment assessment)
        {
            return await PutJsonAsync<Models.Assessment>(CreateAuthorizationPolicyUrl($"{ApiUrl}/{assessment.AssessmentId}", EntityNames.Module, assessment.ModuleId), assessment);
        }

        public async Task DeleteAssessmentAsync(int assessmentId, int moduleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{ApiUrl}/{assessmentId}?moduleId={moduleId}", EntityNames.Module, moduleId));
        }

        public async Task<bool> CanSubmitAssessmentAsync(int growerId, int moduleId)
        {
            return await GetJsonAsync<bool>(CreateAuthorizationPolicyUrl($"{ApiUrl}/can-submit/{growerId}?moduleId={moduleId}", EntityNames.Module, moduleId));
        }
    }
}
