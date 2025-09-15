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
        Task<List<TreePlantingApplication>> GetApplicationsAsync();
        Task<TreePlantingApplication> GetApplicationAsync(int ApplicationId);
        Task<TreePlantingApplication> AddApplicationAsync(TreePlantingApplication Application);
        Task<TreePlantingApplication> UpdateApplicationAsync(TreePlantingApplication Application);
        Task DeleteApplicationAsync(int ApplicationId);
        Task<TreePlantingApplication> ApproveApplicationAsync(int ApplicationId, string Comments = null);
        Task<TreePlantingApplication> RejectApplicationAsync(int ApplicationId, string RejectionReason);
        Task<ApplicationReview> AddReviewAsync(ApplicationReview Review);
        Task<List<ApplicationReview>> GetApplicationReviewsAsync(int ApplicationId);
    }

    public class ApplicationService : ServiceBase, IApplicationService
    {
        public ApplicationService(HttpClient http, SiteState siteState) : base(http, siteState) { }

        private string ApiUrl => CreateApiUrl("Application");

        public async Task<List<TreePlantingApplication>> GetApplicationsAsync()
        {
            var applications = await GetJsonAsync<List<TreePlantingApplication>>(
                $"{ApiUrl}", 
                new List<TreePlantingApplication>());
            return applications.OrderByDescending(item => item.CreatedOn).ToList();
        }

        public async Task<TreePlantingApplication> GetApplicationAsync(int ApplicationId)
        {
            return await GetJsonAsync<TreePlantingApplication>($"{ApiUrl}/{ApplicationId}");
        }

        public async Task<TreePlantingApplication> AddApplicationAsync(TreePlantingApplication Application)
        {
            return await PostJsonAsync<TreePlantingApplication>($"{ApiUrl}", Application);
        }

        public async Task<TreePlantingApplication> UpdateApplicationAsync(TreePlantingApplication Application)
        {
            return await PutJsonAsync<TreePlantingApplication>($"{ApiUrl}/{Application.ApplicationId}", Application);
        }

        public async Task DeleteApplicationAsync(int ApplicationId)
        {
            await DeleteAsync($"{ApiUrl}/{ApplicationId}");
        }

        public async Task<TreePlantingApplication> ApproveApplicationAsync(int ApplicationId, string Comments = null)
        {
            var request = new { ApplicationId, Comments };
            return await PostJsonAsync<object, TreePlantingApplication>($"{ApiUrl}/approve/{ApplicationId}", request);
        }

        public async Task<TreePlantingApplication> RejectApplicationAsync(int ApplicationId, string RejectionReason)
        {
            var request = new { ApplicationId, RejectionReason };
            return await PostJsonAsync<object, TreePlantingApplication>($"{ApiUrl}/reject/{ApplicationId}", request);
        }

        public async Task<ApplicationReview> AddReviewAsync(ApplicationReview Review)
        {
            return await PostJsonAsync<ApplicationReview>($"{ApiUrl}/review", Review);
        }

        public async Task<List<ApplicationReview>> GetApplicationReviewsAsync(int ApplicationId)
        {
            return await GetJsonAsync<List<ApplicationReview>>(
                $"{ApiUrl}/reviews/{ApplicationId}", 
                new List<ApplicationReview>());
        }
    }
}