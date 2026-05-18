using OpenEug.TenTrees.Models;
using Oqtane.Services;
using Oqtane.Shared;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenEug.TenTrees.Module.Grower.Services
{
    public class OrchardService : ServiceBase, IOrchardService
    {
        public OrchardService(HttpClient http, SiteState siteState) : base(http, siteState) { }

        private string ApiUrl => CreateApiUrl("Orchard");

        public async Task<List<OrchardWithTreesDto>> GetOrchardsForGrowerAsync(int growerId)
        {
            return await GetJsonAsync<List<OrchardWithTreesDto>>($"{ApiUrl}/grower/{growerId}", new List<OrchardWithTreesDto>());
        }

        public async Task<TreeDetailDto> AddTreeToGrowerAsync(AddTreeRequest request, int moduleId)
        {
            return await PostJsonAsync<AddTreeRequest, TreeDetailDto>(
                CreateAuthorizationPolicyUrl($"{ApiUrl}/tree?moduleId={moduleId}", EntityNames.Module, moduleId),
                request);
        }

        public async Task DeleteTreeAsync(int treeId, int moduleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{ApiUrl}/tree/{treeId}?moduleId={moduleId}", EntityNames.Module, moduleId));
        }
    }
}
