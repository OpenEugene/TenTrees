using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Oqtane.Services;
using Oqtane.Shared;
using OE.TenTrees.Models;

namespace OE.TenTrees.Services
{
    public interface IApplicationService
    {
        Task<List<TreePlantingApplication>> GetApplicationsAsync(int ModuleId);
        Task<TreePlantingApplication> GetApplicationAsync(int ApplicationId, int ModuleId);
        Task<TreePlantingApplication> AddApplicationAsync(TreePlantingApplication Application);
        Task<TreePlantingApplication> UpdateApplicationAsync(TreePlantingApplication Application);
        Task DeleteApplicationAsync(int ApplicationId, int ModuleId);
        Task<TreePlantingApplication> ApproveApplicationAsync(int ApplicationId, int ModuleId, string Comments = null);
        Task<TreePlantingApplication> RejectApplicationAsync(int ApplicationId, int ModuleId, string RejectionReason);
        Task<ApplicationReview> AddReviewAsync(ApplicationReview Review);
        Task<List<ApplicationReview>> GetApplicationReviewsAsync(int ApplicationId);
    }

    public class ApplicationService : ServiceBase, IApplicationService
    {
        public ApplicationService(HttpClient http, SiteState siteState) : base(http, siteState) { }

        private string ApiUrl => CreateApiUrl("Application");

        public async Task<List<TreePlantingApplication>> GetApplicationsAsync(int ModuleId)
        {
            var applications = await GetJsonAsync<List<TreePlantingApplication>>(
                CreateAuthorizationPolicyUrl($"{ApiUrl}?moduleid={ModuleId}", EntityNames.Module, ModuleId), 
                new List<TreePlantingApplication>());
            return applications.OrderByDescending(item => item.CreatedOn).ToList();
        }

        public async Task<TreePlantingApplication> GetApplicationAsync(int ApplicationId, int ModuleId)
        {
            return await GetJsonAsync<TreePlantingApplication>(
                CreateAuthorizationPolicyUrl($"{ApiUrl}/{ApplicationId}/{ModuleId}", EntityNames.Module, ModuleId));
        }

        public async Task<TreePlantingApplication> AddApplicationAsync(TreePlantingApplication Application)
        {
            return await PostJsonAsync<TreePlantingApplication>(
                CreateAuthorizationPolicyUrl($"{ApiUrl}", EntityNames.Module, Application.ModuleId), Application);
        }

        public async Task<TreePlantingApplication> UpdateApplicationAsync(TreePlantingApplication Application)
        {
            return await PutJsonAsync<TreePlantingApplication>(
                CreateAuthorizationPolicyUrl($"{ApiUrl}/{Application.ApplicationId}", EntityNames.Module, Application.ModuleId), Application);
        }

        public async Task DeleteApplicationAsync(int ApplicationId, int ModuleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{ApiUrl}/{ApplicationId}/{ModuleId}", EntityNames.Module, ModuleId));
        }

        public async Task<TreePlantingApplication> ApproveApplicationAsync(int ApplicationId, int ModuleId, string Comments = null)
        {
            var request = new { ApplicationId, Comments };
            return await PostJsonAsync<object, TreePlantingApplication>(
                CreateAuthorizationPolicyUrl($"{ApiUrl}/approve/{ApplicationId}", EntityNames.Module, ModuleId), request);
        }

        public async Task<TreePlantingApplication> RejectApplicationAsync(int ApplicationId, int ModuleId, string RejectionReason)
        {
            var request = new { ApplicationId, RejectionReason };
            return await PostJsonAsync<object, TreePlantingApplication>(
                CreateAuthorizationPolicyUrl($"{ApiUrl}/reject/{ApplicationId}", EntityNames.Module, ModuleId), request);
        }

        public async Task<ApplicationReview> AddReviewAsync(ApplicationReview Review)
        {
            return await PostJsonAsync<ApplicationReview>(
                CreateAuthorizationPolicyUrl($"{ApiUrl}/review", EntityNames.Module, Review.ApplicationId), Review);
        }

        public async Task<List<ApplicationReview>> GetApplicationReviewsAsync(int ApplicationId)
        {
            return await GetJsonAsync<List<ApplicationReview>>(
                CreateAuthorizationPolicyUrl($"{ApiUrl}/reviews/{ApplicationId}", EntityNames.Module, ApplicationId), 
                new List<ApplicationReview>());
        }
    }
}