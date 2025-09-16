using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using OE.TenTrees.Models;
using OE.TenTrees.Repository.Extensions;
using Oqtane.Modules;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;

namespace OE.TenTrees.Repository
{
    public interface IMonitoringRepository
    {
        IEnumerable<MonitoringSession> GetMonitoringSessions();
        IEnumerable<MonitoringSession> GetMonitoringSessionsByApplication(int ApplicationId);
        MonitoringSession GetMonitoringSession(int MonitoringSessionId);
        MonitoringSession GetMonitoringSession(int MonitoringSessionId, bool tracking);
        MonitoringSession AddMonitoringSession(MonitoringSession Session);
        MonitoringSession UpdateMonitoringSession(MonitoringSession Session);
        void DeleteMonitoringSession(int MonitoringSessionId);
        
        IEnumerable<MonitoringPhoto> GetMonitoringPhotos(int MonitoringSessionId);
        MonitoringPhoto AddMonitoringPhoto(MonitoringPhoto Photo);
        void DeleteMonitoringPhoto(int PhotoId);
        
        IEnumerable<MonitoringMetric> GetMonitoringMetrics(int MonitoringSessionId);
        MonitoringMetric AddMonitoringMetric(MonitoringMetric Metric);
        void DeleteMonitoringMetric(int MetricId);
        
        IEnumerable<TreePlantingApplication> GetApplicationsForMonitoring();
        MonitoringSessionVm GetMonitoringSessionWithDetails(int MonitoringSessionId);
        IEnumerable<MonitoringListItemVm> GetMonitoringSessionListItems();
    }

    public class MonitoringRepository : IMonitoringRepository, ITransientService
    {
        private readonly IDbContextFactory<Context> _factory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MonitoringRepository(IDbContextFactory<Context> factory, IHttpContextAccessor httpContextAccessor)
        {
            _factory = factory;
            _httpContextAccessor = httpContextAccessor;
        }

        public IEnumerable<MonitoringSession> GetMonitoringSessions()
        {
            try
            {
                using var db = _factory.CreateDbContext();
                return db.MonitoringSession
                    .OrderByDescending(item => item.SessionDate)
                    .ToList();
            }
            catch (SqlException ex) when (ex.Message.Contains("Invalid object name 'MonitoringSession'"))
            {
                // Migration hasn't run yet, return empty list
                return new List<MonitoringSession>();
            }
        }

        public IEnumerable<MonitoringListItemVm> GetMonitoringSessionListItems()
        {
            try
            {
                using var db = _factory.CreateDbContext();
                var query = from session in db.MonitoringSession
                           join application in db.TreePlantingApplication 
                               on session.ApplicationId equals application.ApplicationId
                           orderby session.SessionDate descending
                           select new MonitoringListItemVm
                           {
                               MonitoringSessionId = session.MonitoringSessionId,
                               ApplicationId = session.ApplicationId,
                               GardenSiteId = session.GardenSiteId,
                               BeneficiaryName = session.BeneficiaryName,
                               EvaluatorName = session.EvaluatorName,
                               SessionDate = session.SessionDate,
                               TreesAlive = session.TreesAlive,
                               TreesPlanted = session.TreesPlanted,
                               RequiresAttention = (session.TreesPlanted.HasValue && session.TreesPlanted > 0 && session.TreesAlive.HasValue
                                    ? (double)(session.TreesPlanted.Value - session.TreesAlive.Value) / session.TreesPlanted.Value * 100
                                    : 0) > 30 || 
                                    session.TreesLookingHealthy == false || 
                                    !string.IsNullOrEmpty(session.ProblemsIdentified)
                            };

                return query.ToList();
            }
            catch (SqlException ex) when (ex.Message.Contains("Invalid object name 'MonitoringSession'"))
            {
                // Migration hasn't run yet, return empty list
                return new List<MonitoringListItemVm>();
            }
        }

        public IEnumerable<MonitoringSession> GetMonitoringSessionsByApplication(int ApplicationId)
        {
            try
            {
                using var db = _factory.CreateDbContext();
                return db.MonitoringSession
                    .Where(item => item.ApplicationId == ApplicationId)
                    .OrderByDescending(item => item.SessionDate)
                    .ToList();
            }
            catch (SqlException ex) when (ex.Message.Contains("Invalid object name 'MonitoringSession'"))
            {
                // Migration hasn't run yet, return empty list
                return new List<MonitoringSession>();
            }
        }

        public MonitoringSession GetMonitoringSession(int MonitoringSessionId)
        {
            return GetMonitoringSession(MonitoringSessionId, true);
        }

        public MonitoringSession GetMonitoringSession(int MonitoringSessionId, bool tracking)
        {
            try
            {
                using var db = _factory.CreateDbContext();
                if (tracking)
                {
                    return db.MonitoringSession.Find(MonitoringSessionId);
                }
                else
                {
                    return db.MonitoringSession.AsNoTracking().FirstOrDefault(item => item.MonitoringSessionId == MonitoringSessionId);
                }
            }
            catch (SqlException ex) when (ex.Message.Contains("Invalid object name 'MonitoringSession'"))
            {
                // Migration hasn't run yet, return null
                return null;
            }
        }

        public MonitoringSession AddMonitoringSession(MonitoringSession Session)
        {
            try
            {
                using var db = _factory.CreateDbContext();
                db.MonitoringSession.Add(Session);
                db.SaveChangesWithAudit(_httpContextAccessor);
                return Session;
            }
            catch (SqlException ex) when (ex.Message.Contains("Invalid object name 'MonitoringSession'"))
            {
                throw new InvalidOperationException("MonitoringSession table does not exist. Please ensure database migrations have been run.", ex);
            }
        }

        public MonitoringSession UpdateMonitoringSession(MonitoringSession Session)
        {
            try
            {
                using var db = _factory.CreateDbContext();
                db.Entry(Session).State = EntityState.Modified;
                db.SaveChangesWithAudit(_httpContextAccessor);
                return Session;
            }
            catch (SqlException ex) when (ex.Message.Contains("Invalid object name 'MonitoringSession'"))
            {
                throw new InvalidOperationException("MonitoringSession table does not exist. Please ensure database migrations have been run.", ex);
            }
        }

        public void DeleteMonitoringSession(int MonitoringSessionId)
        {
            try
            {
                using var db = _factory.CreateDbContext();
                MonitoringSession Session = db.MonitoringSession.Find(MonitoringSessionId);
                if (Session != null)
                {
                    db.MonitoringSession.Remove(Session);
                    db.SaveChanges(); // No audit needed for deletion
                }
            }
            catch (SqlException ex) when (ex.Message.Contains("Invalid object name 'MonitoringSession'"))
            {
                throw new InvalidOperationException("MonitoringSession table does not exist. Please ensure database migrations have been run.", ex);
            }
        }

        public IEnumerable<MonitoringPhoto> GetMonitoringPhotos(int MonitoringSessionId)
        {
            try
            {
                using var db = _factory.CreateDbContext();
                return db.MonitoringPhoto
                    .Where(p => p.MonitoringSessionId == MonitoringSessionId)
                    .OrderBy(p => p.CreatedOn)
                    .ToList();
            }
            catch (SqlException ex) when (ex.Message.Contains("Invalid object name 'MonitoringPhoto'"))
            {
                // Migration hasn't run yet, return empty list
                return new List<MonitoringPhoto>();
            }
        }

        public MonitoringPhoto AddMonitoringPhoto(MonitoringPhoto Photo)
        {
            try
            {
                using var db = _factory.CreateDbContext();
                db.MonitoringPhoto.Add(Photo);
                db.SaveChangesWithAudit(_httpContextAccessor);
                return Photo;
            }
            catch (SqlException ex) when (ex.Message.Contains("Invalid object name 'MonitoringPhoto'"))
            {
                throw new InvalidOperationException("MonitoringPhoto table does not exist. Please ensure database migrations have been run.", ex);
            }
        }

        public void DeleteMonitoringPhoto(int PhotoId)
        {
            try
            {
                using var db = _factory.CreateDbContext();
                MonitoringPhoto Photo = db.MonitoringPhoto.Find(PhotoId);
                if (Photo != null)
                {
                    db.MonitoringPhoto.Remove(Photo);
                    db.SaveChanges(); // No audit needed for deletion
                }
            }
            catch (SqlException ex) when (ex.Message.Contains("Invalid object name 'MonitoringPhoto'"))
            {
                throw new InvalidOperationException("MonitoringPhoto table does not exist. Please ensure database migrations have been run.", ex);
            }
        }

        public IEnumerable<MonitoringMetric> GetMonitoringMetrics(int MonitoringSessionId)
        {
            try
            {
                using var db = _factory.CreateDbContext();
                return db.MonitoringMetric
                    .Where(m => m.MonitoringSessionId == MonitoringSessionId)
                    .OrderBy(m => m.MetricType)
                    .ToList();
            }
            catch (SqlException ex) when (ex.Message.Contains("Invalid object name 'MonitoringMetric'"))
            {
                // Migration hasn't run yet, return empty list
                return new List<MonitoringMetric>();
            }
        }

        public MonitoringMetric AddMonitoringMetric(MonitoringMetric Metric)
        {
            try
            {
                using var db = _factory.CreateDbContext();
                db.MonitoringMetric.Add(Metric);
                db.SaveChangesWithAudit(_httpContextAccessor);
                return Metric;
            }
            catch (SqlException ex) when (ex.Message.Contains("Invalid object name 'MonitoringMetric'"))
            {
                throw new InvalidOperationException("MonitoringMetric table does not exist. Please ensure database migrations have been run.", ex);
            }
        }

        public void DeleteMonitoringMetric(int MetricId)
        {
            try
            {
                using var db = _factory.CreateDbContext();
                MonitoringMetric Metric = db.MonitoringMetric.Find(MetricId);
                if (Metric != null)
                {
                    db.MonitoringMetric.Remove(Metric);
                    db.SaveChanges(); // No audit needed for deletion
                }
            }
            catch (SqlException ex) when (ex.Message.Contains("Invalid object name 'MonitoringMetric'"))
            {
                throw new InvalidOperationException("MonitoringMetric table does not exist. Please ensure database migrations have been run.", ex);
            }
        }

        public IEnumerable<TreePlantingApplication> GetApplicationsForMonitoring()
        {
            using var db = _factory.CreateDbContext();
            return db.TreePlantingApplication
                .Where(a => a.Status == ApplicationStatus.Approved)
                .OrderBy(a => a.BeneficiaryName)
                .ToList();
        }

        public MonitoringSessionVm GetMonitoringSessionWithDetails(int MonitoringSessionId)
        {
            try
            {
                var session = GetMonitoringSession(MonitoringSessionId);
                if (session == null) return null;

                // Get application using LINQ join
                using var db = _factory.CreateDbContext();
                var application = db.TreePlantingApplication
                    .Where(a => a.ApplicationId == session.ApplicationId)
                    .FirstOrDefault();

                return new MonitoringSessionVm
                {
                    Session = session,
                    Metrics = GetMonitoringMetrics(MonitoringSessionId),
                    Photos = GetMonitoringPhotos(MonitoringSessionId),
                    Application = application
                };
            }
            catch (SqlException ex) when (ex.Message.Contains("Invalid object name"))
            {
                // Migration hasn't run yet, return null
                return null;
            }
        }
    }
}