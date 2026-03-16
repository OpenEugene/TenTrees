using OpenEug.TenTrees.Models;
using OpenEug.TenTrees.Module.Mentor.Services;
using Oqtane.Services;
using Oqtane.Shared;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenEug.TenTrees.Module.Mentor.Services
{
    public class MentorService : ServiceBase, IMentorService
    {
        public MentorService(HttpClient http, SiteState siteState) : base(http, siteState) { }

        private string ApiUrl => CreateApiUrl("Mentor");

        public async Task<List<MentorViewModel>> GetMentorsAsync(int moduleId)
        {
            return await GetJsonAsync<List<MentorViewModel>>(
                CreateAuthorizationPolicyUrl($"{ApiUrl}?moduleId={moduleId}", EntityNames.Module, moduleId),
                new List<MentorViewModel>());
        }

        public async Task<MentorViewModel> GetMentorAsync(string username, int moduleId)
        {
            return await GetJsonAsync<MentorViewModel>(
                CreateAuthorizationPolicyUrl($"{ApiUrl}/{username}?moduleId={moduleId}", EntityNames.Module, moduleId));
        }

        public async Task<MentorViewModel> CreateMentorAsync(MentorViewModel model, int moduleId)
        {
            return await PostJsonAsync<MentorViewModel, MentorViewModel>(
                CreateAuthorizationPolicyUrl($"{ApiUrl}?moduleId={moduleId}", EntityNames.Module, moduleId),
                model);
        }

        public async Task<MentorViewModel> UpdateMentorProfileAsync(MentorViewModel model, int moduleId)
        {
            return await PutJsonAsync<MentorViewModel, MentorViewModel>(
                CreateAuthorizationPolicyUrl($"{ApiUrl}/{model.Username}/profile?moduleId={moduleId}", EntityNames.Module, moduleId),
                model);
        }

        public async Task SetMentorActiveAsync(string username, bool isActive, int moduleId)
        {
            string action = isActive ? "activate" : "deactivate";
            await PutAsync(
                CreateAuthorizationPolicyUrl($"{ApiUrl}/{username}/{action}?moduleId={moduleId}", EntityNames.Module, moduleId));
        }

        public async Task ReassignGrowerAsync(int growerId, string newMentorUsername, int moduleId)
        {
            await PutAsync(
                CreateAuthorizationPolicyUrl($"{ApiUrl}/grower/{growerId}?newMentorUsername={newMentorUsername ?? ""}&moduleId={moduleId}", EntityNames.Module, moduleId));
        }
    }
}
