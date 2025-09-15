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

    public class ServerMonitoringService : IMonitoringService
    {
        private readonly IMonitoringRepository _monitoringRepository;
        private readonly ILogManager _logger;
        private readonly IHttpContextAccessor _accessor;

        public ServerMonitoringService(
            IMonitoringRepository monitoringRepository, 
            ILogManager logger, 
            IHttpContextAccessor accessor)
        {
            _monitoringRepository = monitoringRepository;
            _logger = logger;
            _accessor = accessor;
        }

        public Task<List<MonitoringSession>> GetMonitoringSessionsAsync()
        {
            return Task.FromResult(_monitoringRepository.GetMonitoringSessions().ToList());
        }

        public Task<List<MonitoringListItemVm>> GetMonitoringSessionListItemsAsync()
        {
            return Task.FromResult(_monitoringRepository.GetMonitoringSessionListItems().ToList());
        }

        public Task<List<MonitoringSession>> GetMonitoringSessionsByApplicationAsync(int ApplicationId)
        {
            return Task.FromResult(_monitoringRepository.GetMonitoringSessionsByApplication(ApplicationId).ToList());
        }

        public Task<MonitoringSession> GetMonitoringSessionAsync(int MonitoringSessionId)
        {
            return Task.FromResult(_monitoringRepository.GetMonitoringSession(MonitoringSessionId));
        }

        public Task<MonitoringSession> AddMonitoringSessionAsync(MonitoringSession Session)
        {
            Session.ObserverUserId = _accessor.HttpContext.User.Identity.Name;
            Session = _monitoringRepository.AddMonitoringSession(Session);
            
            // Check if intervention is needed and log appropriately
            if (Session.MortalityRate > 30 || Session.TreesLookingHealthy == false || !string.IsNullOrEmpty(Session.ProblemsIdentified))
            {
                _logger.Log(LogLevel.Warning, this, LogFunction.Create, "Monitoring Session requires intervention {SessionId}", Session.MonitoringSessionId);
            }
            
            _logger.Log(LogLevel.Information, this, LogFunction.Create, "Monitoring Session Added {Session}", Session);
            return Task.FromResult(Session);
        }

        public Task<MonitoringSession> UpdateMonitoringSessionAsync(MonitoringSession Session)
        {
            Session = _monitoringRepository.UpdateMonitoringSession(Session);
            
            // Check if intervention is needed and log appropriately
            if (Session.MortalityRate > 30 || Session.TreesLookingHealthy == false || !string.IsNullOrEmpty(Session.ProblemsIdentified))
            {
                _logger.Log(LogLevel.Warning, this, LogFunction.Update, "Monitoring Session requires intervention {SessionId}", Session.MonitoringSessionId);
            }
            
            _logger.Log(LogLevel.Information, this, LogFunction.Update, "Monitoring Session Updated {Session}", Session);
            return Task.FromResult(Session);
        }

        public Task DeleteMonitoringSessionAsync(int MonitoringSessionId)
        {
            _monitoringRepository.DeleteMonitoringSession(MonitoringSessionId);
            _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Monitoring Session Deleted {SessionId}", MonitoringSessionId);
            return Task.CompletedTask;
        }

        public Task<List<MonitoringPhoto>> GetMonitoringPhotosAsync(int MonitoringSessionId)
        {
            return Task.FromResult(_monitoringRepository.GetMonitoringPhotos(MonitoringSessionId).ToList());
        }

        public Task<MonitoringPhoto> AddMonitoringPhotoAsync(MonitoringPhoto Photo)
        {
            Photo = _monitoringRepository.AddMonitoringPhoto(Photo);
            _logger.Log(LogLevel.Information, this, LogFunction.Create, "Monitoring Photo Added {Photo}", Photo);
            return Task.FromResult(Photo);
        }

        public Task DeleteMonitoringPhotoAsync(int PhotoId)
        {
            _monitoringRepository.DeleteMonitoringPhoto(PhotoId);
            _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Monitoring Photo Deleted {PhotoId}", PhotoId);
            return Task.CompletedTask;
        }

        public Task<List<TreePlantingApplication>> GetApplicationsForMonitoringAsync()
        {
            return Task.FromResult(_monitoringRepository.GetApplicationsForMonitoring().ToList());
        }

        public Task<MonitoringSessionVm> GetMonitoringSessionWithDetailsAsync(int MonitoringSessionId)
        {
            return Task.FromResult(_monitoringRepository.GetMonitoringSessionWithDetails(MonitoringSessionId));
        }
    }
}