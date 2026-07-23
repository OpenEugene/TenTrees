using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Oqtane.Services;
using Oqtane.Shared;
using Models = OpenEug.TenTrees.Models;

namespace OpenEug.TenTrees.Module.TreeType.Services
{
    public class TreeTypeService : ServiceBase, ITreeTypeService
    {
        public TreeTypeService(HttpClient http, SiteState siteState) : base(http, siteState) { }

        private string Apiurl => CreateApiUrl("TreeType");

        public async Task<List<Models.TreeType>> GetTreeTypesAsync()
        {
            List<Models.TreeType> treeTypes = await GetJsonAsync<List<Models.TreeType>>($"{Apiurl}", Enumerable.Empty<Models.TreeType>().ToList());
            return treeTypes.OrderBy(item => item.Category).ThenBy(item => item.SortOrder).ToList();
        }

        public async Task<Models.TreeType> GetTreeTypeAsync(int treeTypeId)
        {
            return await GetJsonAsync<Models.TreeType>($"{Apiurl}/{treeTypeId}");
        }

        public async Task<Models.TreeType> AddTreeTypeAsync(Models.TreeType treeType)
        {
            return await PostJsonAsync<Models.TreeType>($"{Apiurl}", treeType);
        }

        public async Task<Models.TreeType> UpdateTreeTypeAsync(Models.TreeType treeType)
        {
            return await PutJsonAsync<Models.TreeType>($"{Apiurl}/{treeType.TreeTypeId}", treeType);
        }

        public async Task DeleteTreeTypeAsync(int treeTypeId)
        {
            await DeleteAsync($"{Apiurl}/{treeTypeId}");
        }

        public async Task<List<Models.TreeType>> GetActiveTreeTypesAsync()
        {
            return await GetJsonAsync<List<Models.TreeType>>($"{Apiurl}/active", Enumerable.Empty<Models.TreeType>().ToList());
        }
    }
}
