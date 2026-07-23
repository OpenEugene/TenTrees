using System.Collections.Generic;
using System.Threading.Tasks;
using OpenEug.TenTrees.Models;

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
        Task<bool> HasAssociatedDataAsync(int villageId);
    }
}
