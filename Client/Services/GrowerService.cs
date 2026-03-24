using OpenEug.TenTrees.Models;
using Oqtane.Services;
using Oqtane.Shared;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenEug.TenTrees.Module.Grower.Services
{
    public class GrowerService : ServiceBase, IGrowerService
    {
        public GrowerService(HttpClient http, SiteState siteState) : base(http, siteState) { }

        private string ApiUrl => CreateApiUrl("Grower");

        public async Task<Models.Grower> GetGrowerAsync(int growerId)
        {
            return await GetJsonAsync<Models.Grower>($"{ApiUrl}/{growerId}");
        }

        public async Task<List<Models.Grower>> GetAllGrowersAsync(int? villageId = null)
        {
            string url = $"{ApiUrl}/all";
            if (villageId.HasValue)
            {
                url += $"?villageId={villageId.Value}";
            }
            return await GetJsonAsync<List<Models.Grower>>(url);
        }

        public async Task<Models.Grower> ToggleActiveStatusAsync(int growerId, int moduleId)
        {
            var request = new StatusToggleRequest { GrowerId = growerId };
            return await PostJsonAsync<StatusToggleRequest, Models.Grower>(CreateAuthorizationPolicyUrl($"{ApiUrl}/toggle-status?moduleId={moduleId}", EntityNames.Module, moduleId), request);
        }

        public async Task<Models.Grower> RecordProgramExitAsync(int growerId, ProgramExitRequest request, int moduleId)
        {
            request.GrowerId = growerId;
            return await PostJsonAsync<ProgramExitRequest, Models.Grower>(CreateAuthorizationPolicyUrl($"{ApiUrl}/exit?moduleId={moduleId}", EntityNames.Module, moduleId), request);
        }

        public async Task<List<Models.Grower>> GetActiveGrowersAsync(int? villageId = null)
        {
            string url = $"{ApiUrl}/active";
            if (villageId.HasValue)
            {
                url += $"?villageId={villageId.Value}";
            }
            return await GetJsonAsync<List<Models.Grower>>(url);
        }

        public async Task<List<Models.Grower>> GetGrowersByStatusAsync(GrowerStatus status, int? villageId = null)
        {
            string url = $"{ApiUrl}/by-status?status={status}";
            if (villageId.HasValue)
            {
                url += $"&villageId={villageId.Value}";
            }
            return await GetJsonAsync<List<Models.Grower>>(url);
        }

        public async Task<GrowerStatusSummary> GetStatusSummaryAsync(int? villageId = null)
        {
            string url = $"{ApiUrl}/status-summary";
            if (villageId.HasValue)
            {
                url += $"?villageId={villageId.Value}";
            }
            return await GetJsonAsync<GrowerStatusSummary>(url);
        }

        public async Task<Models.Grower> UpdateGrowerAsync(Models.Grower grower, int moduleId)
        {
            return await PutJsonAsync<Models.Grower>(CreateAuthorizationPolicyUrl($"{ApiUrl}/{grower.GrowerId}?moduleId={moduleId}", EntityNames.Module, moduleId), grower);
        }
    }
}
