using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Models;
using Oqtane.Security;
using Oqtane.Shared;
using OE.TenTrees.Repository;
using OE.TenTrees.Models;

namespace OE.TenTrees.Services
{
    public interface IGardenService
    {
        Task<List<GardenSite>> GetGardensAsync();
        Task<GardenSite> GetGardenAsync(int GardenSiteId);
        Task<GardenSite> GetGardenByApplicationIdAsync(int ApplicationId);
        Task<GardenSite> AddGardenAsync(GardenSite gardenSite);
        Task<GardenSite> UpdateGardenAsync(GardenSite gardenSite);
        Task DeleteGardenAsync(int GardenSiteId);
        
        Task<List<TreePlanting>> GetTreePlantingsAsync(int GardenSiteId);
        Task<TreePlanting> AddTreePlantingAsync(TreePlanting treePlanting);
        Task<TreePlanting> UpdateTreePlantingAsync(TreePlanting treePlanting);
        Task DeleteTreePlantingAsync(int TreePlantingId);
        
        Task<List<GardenPhoto>> GetGardenPhotosAsync(int GardenSiteId);
        Task<GardenPhoto> AddGardenPhotoAsync(GardenPhoto photo);
        Task DeleteGardenPhotoAsync(int GardenPhotoId);
        
        Task<GardenStatistics> GetGardenStatisticsAsync(int GardenSiteId);
        Task<List<GardenListItemVm>> GetGardenListItemsAsync();
        Task<GardenDetailVm> GetGardenDetailAsync(int GardenSiteId);
        Task<GardenSite> CreateGardenFromApplicationAsync(CreateGardenFromApplicationRequest request);
    }

    public class ServerGardenService : IGardenService
    {
        private readonly IGardenRepository _gardenRepository;
        private readonly ILogManager _logger;
        private readonly IHttpContextAccessor _accessor;

        public ServerGardenService(
            IGardenRepository gardenRepository, 
            ILogManager logger, 
            IHttpContextAccessor accessor)
        {
            _gardenRepository = gardenRepository;
            _logger = logger;
            _accessor = accessor;
        }

        public Task<List<GardenSite>> GetGardensAsync()
        {
            return Task.FromResult(_gardenRepository.GetGardens().ToList());
        }

        public Task<GardenSite> GetGardenAsync(int GardenSiteId)
        {
            return Task.FromResult(_gardenRepository.GetGarden(GardenSiteId));
        }

        public Task<GardenSite> GetGardenByApplicationIdAsync(int ApplicationId)
        {
            return Task.FromResult(_gardenRepository.GetGardenByApplicationId(ApplicationId));
        }

        public Task<GardenSite> AddGardenAsync(GardenSite gardenSite)
        {
            gardenSite = _gardenRepository.AddGarden(gardenSite);
            _logger.Log(LogLevel.Information, this, LogFunction.Create, "Garden Added {Garden}", gardenSite);
            return Task.FromResult(gardenSite);
        }

        public Task<GardenSite> UpdateGardenAsync(GardenSite gardenSite)
        {
            gardenSite = _gardenRepository.UpdateGarden(gardenSite);
            _logger.Log(LogLevel.Information, this, LogFunction.Update, "Garden Updated {Garden}", gardenSite);
            return Task.FromResult(gardenSite);
        }

        public Task DeleteGardenAsync(int GardenSiteId)
        {
            _gardenRepository.DeleteGarden(GardenSiteId);
            _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Garden Deleted {GardenSiteId}", GardenSiteId);
            return Task.CompletedTask;
        }

        public Task<List<TreePlanting>> GetTreePlantingsAsync(int GardenSiteId)
        {
            return Task.FromResult(_gardenRepository.GetTreePlantings(GardenSiteId).ToList());
        }

        public Task<TreePlanting> AddTreePlantingAsync(TreePlanting treePlanting)
        {
            treePlanting = _gardenRepository.AddTreePlanting(treePlanting);
            _logger.Log(LogLevel.Information, this, LogFunction.Create, "Tree Planting Added {TreePlanting}", treePlanting);
            return Task.FromResult(treePlanting);
        }

        public Task<TreePlanting> UpdateTreePlantingAsync(TreePlanting treePlanting)
        {
            treePlanting = _gardenRepository.UpdateTreePlanting(treePlanting);
            _logger.Log(LogLevel.Information, this, LogFunction.Update, "Tree Planting Updated {TreePlanting}", treePlanting);
            return Task.FromResult(treePlanting);
        }

        public Task DeleteTreePlantingAsync(int TreePlantingId)
        {
            _gardenRepository.DeleteTreePlanting(TreePlantingId);
            _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Tree Planting Deleted {TreePlantingId}", TreePlantingId);
            return Task.CompletedTask;
        }

        public Task<List<GardenPhoto>> GetGardenPhotosAsync(int GardenSiteId)
        {
            return Task.FromResult(_gardenRepository.GetGardenPhotos(GardenSiteId).ToList());
        }

        public Task<GardenPhoto> AddGardenPhotoAsync(GardenPhoto photo)
        {
            photo = _gardenRepository.AddGardenPhoto(photo);
            _logger.Log(LogLevel.Information, this, LogFunction.Create, "Garden Photo Added {Photo}", photo);
            return Task.FromResult(photo);
        }

        public Task DeleteGardenPhotoAsync(int GardenPhotoId)
        {
            _gardenRepository.DeleteGardenPhoto(GardenPhotoId);
            _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Garden Photo Deleted {PhotoId}", GardenPhotoId);
            return Task.CompletedTask;
        }

        public Task<GardenStatistics> GetGardenStatisticsAsync(int GardenSiteId)
        {
            return Task.FromResult(_gardenRepository.GetGardenStatistics(GardenSiteId));
        }

        public Task<List<GardenListItemVm>> GetGardenListItemsAsync()
        {
            return Task.FromResult(_gardenRepository.GetGardenListItems().ToList());
        }

        public async Task<GardenDetailVm> GetGardenDetailAsync(int GardenSiteId)
        {
            var gardenSite = _gardenRepository.GetGarden(GardenSiteId);
            if (gardenSite == null) return null;

            return new GardenDetailVm
            {
                GardenSite = gardenSite,
                Application = gardenSite.Application,
                TreePlantings = _gardenRepository.GetTreePlantings(GardenSiteId),
                MonitoringSessions = gardenSite.MonitoringSessions ?? new List<MonitoringSession>(),
                Photos = _gardenRepository.GetGardenPhotos(GardenSiteId),
                Statistics = _gardenRepository.GetGardenStatistics(GardenSiteId)
            };
        }

        public Task<GardenSite> CreateGardenFromApplicationAsync(CreateGardenFromApplicationRequest request)
        {
            var gardenSite = _gardenRepository.CreateGardenFromApplication(request);
            _logger.Log(LogLevel.Information, this, LogFunction.Create, "Garden Created from Application {ApplicationId}", request.ApplicationId);
            return Task.FromResult(gardenSite);
        }
    }
}