using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Oqtane.Services;
using OE.TenTrees.Models;

namespace OE.TenTrees.Services
{
    public class TreeService : ServiceBase, ITreeService
    {
        public TreeService(HttpClient http, SiteState siteState) : base(http, siteState) { }

        private string Apiurl => CreateApiUrl("Tree");

        public async Task<List<Tree>> GetTreesAsync(int ModuleId)
        {
            return await GetJsonAsync<List<Tree>>($"{Apiurl}?moduleid={ModuleId}");
        }

        public async Task<Tree?> GetTreeAsync(int TreeId, int ModuleId)
        {
            return await GetJsonAsync<Tree>($"{Apiurl}/{TreeId}?moduleid={ModuleId}");
        }

        public async Task<Tree> AddTreeAsync(Tree tree)
        {
            return await PostJsonAsync<Tree>(Apiurl, tree);
        }

        public async Task<Tree> UpdateTreeAsync(Tree tree)
        {
            return await PutJsonAsync<Tree>($"{Apiurl}/{tree.TreeId}", tree);
        }

        public async Task DeleteTreeAsync(int TreeId, int ModuleId)
        {
            await DeleteAsync($"{Apiurl}/{TreeId}?moduleid={ModuleId}");
        }
    }
}