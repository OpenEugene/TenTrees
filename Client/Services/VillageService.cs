using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Oqtane.Services;
using Oqtane.Shared;
using Models = OpenEug.TenTrees.Models;

namespace OpenEug.TenTrees.Module.Village.Services
{
    public interface IVillageService 
    {
        Task<List<Models.Village>> GetVillagesAsync();

        Task<Models.Village> GetVillageAsync(int villageId);

        Task<Models.Village> AddVillageAsync(Models.Village village);

        Task<Models.Village> UpdateVillageAsync(Models.Village village);

        Task DeleteVillageAsync(int villageId);
        
        Task<List<Models.Village>> GetActiveVillagesAsync();
    }

    public class VillageService : ServiceBase, IVillageService
    {
        public VillageService(HttpClient http, SiteState siteState) : base(http, siteState) { }

        private string Apiurl => CreateApiUrl("Village");

        public async Task<List<Models.Village>> GetVillagesAsync()
        {
            List<Models.Village> villages = await GetJsonAsync<List<Models.Village>>($"{Apiurl}", Enumerable.Empty<Models.Village>().ToList());
            return villages.OrderBy(item => item.VillageName).ToList();
        }

        public async Task<Models.Village> GetVillageAsync(int villageId)
        {
            return await GetJsonAsync<Models.Village>($"{Apiurl}/{villageId}");
        }

        public async Task<Models.Village> AddVillageAsync(Models.Village village)
        {
            return await PostJsonAsync<Models.Village>($"{Apiurl}", village);
        }

        public async Task<Models.Village> UpdateVillageAsync(Models.Village village)
        {
            return await PutJsonAsync<Models.Village>($"{Apiurl}/{village.VillageId}", village);
        }

        public async Task DeleteVillageAsync(int villageId)
        {
            await DeleteAsync($"{Apiurl}/{villageId}");
        }
        
        public async Task<List<Models.Village>> GetActiveVillagesAsync()
        {
            return await GetJsonAsync<List<Models.Village>>($"{Apiurl}/active");
        }
    }
}
