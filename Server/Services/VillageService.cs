using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Models;
using Oqtane.Security;
using Oqtane.Shared;
using Models = OpenEug.TenTrees.Models;
using OpenEug.TenTrees.Module.Village.Repository;

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

    public class ServerVillageService : IVillageService
    {
        private readonly IVillageRepository _villageRepository;
        private readonly ILogManager _logger;

        public ServerVillageService(IVillageRepository villageRepository, ILogManager logger)
        {
            _villageRepository = villageRepository;
            _logger = logger;
        }

        public Task<List<Models.Village>> GetVillagesAsync()
        {
            return Task.FromResult(_villageRepository.GetVillages().ToList());
        }

        public Task<Models.Village> GetVillageAsync(int villageId)
        {
            return Task.FromResult(_villageRepository.GetVillage(villageId));
        }

        public Task<Models.Village> AddVillageAsync(Models.Village village)
        {
            village = _villageRepository.AddVillage(village);
            _logger.Log(LogLevel.Information, this, LogFunction.Create, "Village Added {Village}", village);
            return Task.FromResult(village);
        }

        public Task<Models.Village> UpdateVillageAsync(Models.Village village)
        {
            village = _villageRepository.UpdateVillage(village);
            _logger.Log(LogLevel.Information, this, LogFunction.Update, "Village Updated {Village}", village);
            return Task.FromResult(village);
        }

        public Task DeleteVillageAsync(int villageId)
        {
            _villageRepository.DeleteVillage(villageId);
            _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Village Deleted {VillageId}", villageId);
            return Task.CompletedTask;
        }
        
        public Task<List<Models.Village>> GetActiveVillagesAsync()
        {
            return Task.FromResult(_villageRepository.GetVillages().Where(v => v.IsActive).ToList());
        }
    }
}
