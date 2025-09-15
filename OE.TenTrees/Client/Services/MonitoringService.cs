using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Oqtane.Services;
using Oqtane.Shared;
using OE.TenTrees.Models;

namespace OE.TenTrees.Services
{
    public interface IMonitoringService
    {
        Task<List<MonitoringSession>> GetMonitoringSessionsAsync();
        Task<List<MonitoringSession>> GetMonitoringSessionsByApplicationAsync(int ApplicationId);
        Task<MonitoringSession> GetMonitoringSessionAsync(int MonitoringSessionId);
        Task<MonitoringSession> AddMonitoringSessionAsync(MonitoringSession Session);
        Task<MonitoringSession> UpdateMonitoringSessionAsync(MonitoringSession Session);
        Task DeleteMonitoringSessionAsync(int MonitoringSessionId);
        Task<List<MonitoringPhoto>> GetMonitoringPhotosAsync(int MonitoringSessionId);
        Task<MonitoringPhoto> AddMonitoringPhotoAsync(MonitoringPhoto Photo);
        Task DeleteMonitoringPhotoAsync(int PhotoId);
        Task<List<TreePlantingApplication>> GetApplicationsForMonitoringAsync();
        Task<MonitoringSessionVm> GetMonitoringSessionWithDetailsAsync(int MonitoringSessionId);
        Task<List<MonitoringListItemVm>> GetMonitoringSessionListItemsAsync();
    }

    public class MonitoringService : ServiceBase, IMonitoringService
    {
        public MonitoringService(HttpClient http, SiteState siteState) : base(http, siteState) { }

        private string ApiUrl => CreateApiUrl("Monitoring");

        public async Task<List<MonitoringSession>> GetMonitoringSessionsAsync()
        {
            var sessions = await GetJsonAsync<List<MonitoringSession>>(
                $"{ApiUrl}", 
                new List<MonitoringSession>());
            return sessions.OrderByDescending(item => item.SessionDate).ToList();
        }

        public async Task<List<MonitoringListItemVm>> GetMonitoringSessionListItemsAsync()
        {
            var sessions = await GetJsonAsync<List<MonitoringListItemVm>>(
                $"{ApiUrl}/listitems", 
                new List<MonitoringListItemVm>());
            return sessions.OrderByDescending(item => item.SessionDate).ToList();
        }

        public async Task<List<MonitoringSession>> GetMonitoringSessionsByApplicationAsync(int ApplicationId)
        {
            var sessions = await GetJsonAsync<List<MonitoringSession>>(
                $"{ApiUrl}/application/{ApplicationId}", 
                new List<MonitoringSession>());
            return sessions.OrderByDescending(item => item.SessionDate).ToList();
        }

        public async Task<MonitoringSession> GetMonitoringSessionAsync(int MonitoringSessionId)
        {
            return await GetJsonAsync<MonitoringSession>($"{ApiUrl}/{MonitoringSessionId}");
        }

        public async Task<MonitoringSession> AddMonitoringSessionAsync(MonitoringSession Session)
        {
            return await PostJsonAsync<MonitoringSession>($"{ApiUrl}", Session);
        }

        public async Task<MonitoringSession> UpdateMonitoringSessionAsync(MonitoringSession Session)
        {
            return await PutJsonAsync<MonitoringSession>($"{ApiUrl}/{Session.MonitoringSessionId}", Session);
        }

        public async Task DeleteMonitoringSessionAsync(int MonitoringSessionId)
        {
            await DeleteAsync($"{ApiUrl}/{MonitoringSessionId}");
        }

        public async Task<List<MonitoringPhoto>> GetMonitoringPhotosAsync(int MonitoringSessionId)
        {
            return await GetJsonAsync<List<MonitoringPhoto>>(
                $"{ApiUrl}/photos/{MonitoringSessionId}", 
                new List<MonitoringPhoto>());
        }

        public async Task<MonitoringPhoto> AddMonitoringPhotoAsync(MonitoringPhoto Photo)
        {
            return await PostJsonAsync<MonitoringPhoto>($"{ApiUrl}/photos", Photo);
        }

        public async Task DeleteMonitoringPhotoAsync(int PhotoId)
        {
            await DeleteAsync($"{ApiUrl}/photos/{PhotoId}");
        }

        public async Task<List<TreePlantingApplication>> GetApplicationsForMonitoringAsync()
        {
            return await GetJsonAsync<List<TreePlantingApplication>>(
                $"{ApiUrl}/applications", 
                new List<TreePlantingApplication>());
        }

        public async Task<MonitoringSessionVm> GetMonitoringSessionWithDetailsAsync(int MonitoringSessionId)
        {
            return await GetJsonAsync<MonitoringSessionVm>($"{ApiUrl}/details/{MonitoringSessionId}");
        }
    }
}