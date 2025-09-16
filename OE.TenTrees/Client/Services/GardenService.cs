using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Oqtane.Services;
using Oqtane.Shared;
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

    public class GardenService : ServiceBase, IGardenService
    {
        public GardenService(HttpClient http, SiteState siteState) : base(http, siteState) { }

        private string ApiUrl => CreateApiUrl("Garden");

        public async Task<List<GardenSite>> GetGardensAsync()
        {
            var gardens = await GetJsonAsync<List<GardenSite>>($"{ApiUrl}", new List<GardenSite>());
            return gardens.OrderByDescending(item => item.CreatedOn).ToList();
        }

        public async Task<GardenSite> GetGardenAsync(int GardenSiteId)
        {
            return await GetJsonAsync<GardenSite>($"{ApiUrl}/{GardenSiteId}");
        }

        public async Task<GardenSite> GetGardenByApplicationIdAsync(int ApplicationId)
        {
            return await GetJsonAsync<GardenSite>($"{ApiUrl}/by-application/{ApplicationId}");
        }

        public async Task<GardenSite> AddGardenAsync(GardenSite gardenSite)
        {
            return await PostJsonAsync<GardenSite>($"{ApiUrl}", gardenSite);
        }

        public async Task<GardenSite> UpdateGardenAsync(GardenSite gardenSite)
        {
            return await PutJsonAsync<GardenSite>($"{ApiUrl}/{gardenSite.GardenSiteId}", gardenSite);
        }

        public async Task DeleteGardenAsync(int GardenSiteId)
        {
            await DeleteAsync($"{ApiUrl}/{GardenSiteId}");
        }

        public async Task<List<GardenListItemVm>> GetGardenListItemsAsync()
        {
            var gardens = await GetJsonAsync<List<GardenListItemVm>>($"{ApiUrl}/listitems", new List<GardenListItemVm>());
            return gardens.OrderByDescending(item => item.PlantingDate ?? DateTime.MinValue).ToList();
        }

        public async Task<GardenDetailVm> GetGardenDetailAsync(int GardenSiteId)
        {
            return await GetJsonAsync<GardenDetailVm>($"{ApiUrl}/details/{GardenSiteId}");
        }

        public async Task<GardenStatistics> GetGardenStatisticsAsync(int GardenSiteId)
        {
            return await GetJsonAsync<GardenStatistics>($"{ApiUrl}/statistics/{GardenSiteId}");
        }

        public async Task<GardenSite> CreateGardenFromApplicationAsync(CreateGardenFromApplicationRequest request)
        {
            return await PostJsonAsync<CreateGardenFromApplicationRequest, GardenSite>($"{ApiUrl}/from-application", request);
        }

        // Tree Planting methods
        public async Task<List<TreePlanting>> GetTreePlantingsAsync(int GardenSiteId)
        {
            return await GetJsonAsync<List<TreePlanting>>($"{ApiUrl}/{GardenSiteId}/plantings", new List<TreePlanting>());
        }

        public async Task<TreePlanting> AddTreePlantingAsync(TreePlanting treePlanting)
        {
            return await PostJsonAsync<TreePlanting>($"{ApiUrl}/plantings", treePlanting);
        }

        public async Task<TreePlanting> UpdateTreePlantingAsync(TreePlanting treePlanting)
        {
            return await PutJsonAsync<TreePlanting>($"{ApiUrl}/plantings/{treePlanting.TreePlantingId}", treePlanting);
        }

        public async Task DeleteTreePlantingAsync(int TreePlantingId)
        {
            await DeleteAsync($"{ApiUrl}/plantings/{TreePlantingId}");
        }

        // Photo methods
        public async Task<List<GardenPhoto>> GetGardenPhotosAsync(int GardenSiteId)
        {
            return await GetJsonAsync<List<GardenPhoto>>($"{ApiUrl}/{GardenSiteId}/photos", new List<GardenPhoto>());
        }

        public async Task<GardenPhoto> AddGardenPhotoAsync(GardenPhoto photo)
        {
            return await PostJsonAsync<GardenPhoto>($"{ApiUrl}/photos", photo);
        }

        public async Task DeleteGardenPhotoAsync(int GardenPhotoId)
        {
            await DeleteAsync($"{ApiUrl}/photos/{GardenPhotoId}");
        }
    }
}