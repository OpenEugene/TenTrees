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

        public async Task<Models.Grower> GetGrowerAsync(int growerId, int moduleId)
        {
            return await GetJsonAsync<Models.Grower>(CreateAuthorizationPolicyUrl($"{ApiUrl}/{growerId}?moduleId={moduleId}", EntityNames.Module, moduleId));
        }

        public async Task<List<Models.Grower>> GetAllGrowersAsync(int moduleId, int? villageId = null)
        {
            string url = $"{ApiUrl}/all?moduleId={moduleId}";
            if (villageId.HasValue)
            {
                url += $"&villageId={villageId.Value}";
            }
            return await GetJsonAsync<List<Models.Grower>>(CreateAuthorizationPolicyUrl(url, EntityNames.Module, moduleId));
        }

        public async Task<Models.Grower> ToggleActiveStatusAsync(int growerId, int moduleId)
        {
            var request = new StatusToggleRequest { GrowerId = growerId, ModuleId = moduleId };
            return await PostJsonAsync<StatusToggleRequest, Models.Grower>(CreateAuthorizationPolicyUrl($"{ApiUrl}/toggle-status", EntityNames.Module, moduleId), request);
        }

        public async Task<Models.Grower> RecordProgramExitAsync(int growerId, ProgramExitRequest request, int moduleId)
        {
            request.GrowerId = growerId;
            request.ModuleId = moduleId;
            return await PostJsonAsync<ProgramExitRequest, Models.Grower>(CreateAuthorizationPolicyUrl($"{ApiUrl}/exit", EntityNames.Module, moduleId), request);
        }

        public async Task<List<Models.Grower>> GetActiveGrowersAsync(int moduleId, int? villageId = null)
        {
            string url = $"{ApiUrl}/active?moduleId={moduleId}";
            if (villageId.HasValue)
            {
                url += $"&villageId={villageId.Value}";
            }
            return await GetJsonAsync<List<Models.Grower>>(CreateAuthorizationPolicyUrl(url, EntityNames.Module, moduleId));
        }

        public async Task<List<Models.Grower>> GetGrowersByStatusAsync(GrowerStatus status, int moduleId, int? villageId = null)
        {
            string url = $"{ApiUrl}/by-status?status={status}&moduleId={moduleId}";
            if (villageId.HasValue)
            {
                url += $"&villageId={villageId.Value}";
            }
            return await GetJsonAsync<List<Models.Grower>>(CreateAuthorizationPolicyUrl(url, EntityNames.Module, moduleId));
        }

        public async Task<GrowerStatusSummary> GetStatusSummaryAsync(int moduleId, int? villageId = null)
        {
            string url = $"{ApiUrl}/status-summary?moduleId={moduleId}";
            if (villageId.HasValue)
            {
                url += $"&villageId={villageId.Value}";
            }
            return await GetJsonAsync<GrowerStatusSummary>(CreateAuthorizationPolicyUrl(url, EntityNames.Module, moduleId));
        }

        public async Task<Models.Grower> UpdateGrowerAsync(Models.Grower grower, int moduleId)
        {
            return await PutJsonAsync<Models.Grower>(CreateAuthorizationPolicyUrl($"{ApiUrl}/{grower.GrowerId}?moduleId={moduleId}", EntityNames.Module, moduleId), grower);
        }
    }
}
