using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using OE.TenTrees.Models;
using OE.TenTrees.Repository.Extensions;
using Oqtane.Modules;
using Microsoft.AspNetCore.Http;

namespace OE.TenTrees.Repository
{
    public interface IGardenRepository
    {
        IEnumerable<GardenSite> GetGardens();
        GardenSite GetGarden(int GardenSiteId);
        GardenSite GetGarden(int GardenSiteId, bool tracking);
        GardenSite GetGardenByApplicationId(int ApplicationId);
        GardenSite AddGarden(GardenSite gardenSite);
        GardenSite UpdateGarden(GardenSite gardenSite);
        void DeleteGarden(int GardenSiteId);
        
        IEnumerable<TreePlanting> GetTreePlantings(int GardenSiteId);
        TreePlanting AddTreePlanting(TreePlanting treePlanting);
        TreePlanting UpdateTreePlanting(TreePlanting treePlanting);
        void DeleteTreePlanting(int TreePlantingId);
        
        IEnumerable<GardenPhoto> GetGardenPhotos(int GardenSiteId);
        GardenPhoto AddGardenPhoto(GardenPhoto photo);
        void DeleteGardenPhoto(int GardenPhotoId);
        
        GardenStatistics GetGardenStatistics(int GardenSiteId);
        IEnumerable<GardenListItemVm> GetGardenListItems();
        GardenDetailVm GetGardenDetailView(int GardenSiteId);
        GardenSite CreateGardenFromApplication(CreateGardenFromApplicationRequest request);
    }

    public class GardenRepository : IGardenRepository, ITransientService
    {
        private readonly IDbContextFactory<Context> _factory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GardenRepository(IDbContextFactory<Context> factory, IHttpContextAccessor httpContextAccessor)
        {
            _factory = factory;
            _httpContextAccessor = httpContextAccessor;
        }

        public IEnumerable<GardenSite> GetGardens()
        {
            using var db = _factory.CreateDbContext();
            return db.GardenSite
                .OrderByDescending(item => item.CreatedOn)
                .ToList();
        }

        public GardenSite GetGarden(int GardenSiteId)
        {
            return GetGarden(GardenSiteId, true);
        }

        public GardenSite GetGarden(int GardenSiteId, bool tracking)
        {
            using var db = _factory.CreateDbContext();
            if (tracking)
            {
                return db.GardenSite
                    .SingleOrDefault(item => item.GardenSiteId == GardenSiteId);
            }
            else
            {
                return db.GardenSite
                    .AsNoTracking()
                    .SingleOrDefault(item => item.GardenSiteId == GardenSiteId);
            }
        }

        public GardenSite GetGardenByApplicationId(int ApplicationId)
        {
            using var db = _factory.CreateDbContext();
            return db.GardenSite
                .FirstOrDefault(g => g.ApplicationId == ApplicationId);
        }

        public GardenDetailVm GetGardenDetailView(int GardenSiteId)
        {
            using var db = _factory.CreateDbContext();
            
            var gardenQuery = from garden in db.GardenSite
                             join application in db.TreePlantingApplication on garden.ApplicationId equals application.ApplicationId
                             where garden.GardenSiteId == GardenSiteId
                             select new
                             {
                                 Garden = garden,
                                 Application = application
                             };

            var gardenData = gardenQuery.FirstOrDefault();
            if (gardenData == null) return null;

            var treePlantings = db.TreePlanting
                .Where(tp => tp.GardenSiteId == GardenSiteId)
                .ToList();

            var monitoringSessions = db.MonitoringSession
                .Where(ms => ms.GardenSiteId == GardenSiteId)
                .OrderByDescending(ms => ms.SessionDate)
                .ToList();

            var photos = db.GardenPhoto
                .Where(p => p.GardenSiteId == GardenSiteId)
                .OrderBy(p => p.PhotoDate)
                .ToList();

            var statistics = GetGardenStatistics(GardenSiteId);

            return new GardenDetailVm
            {
                GardenSite = gardenData.Garden,
                Application = gardenData.Application,
                TreePlantings = treePlantings,
                MonitoringSessions = monitoringSessions,
                Photos = photos,
                Statistics = statistics
            };
        }

        public GardenSite AddGarden(GardenSite gardenSite)
        {
            using var db = _factory.CreateDbContext();
            db.GardenSite.Add(gardenSite);
            db.SaveChangesWithAudit(_httpContextAccessor);
            return gardenSite;
        }

        public GardenSite UpdateGarden(GardenSite gardenSite)
        {
            using var db = _factory.CreateDbContext();
            db.Entry(gardenSite).State = EntityState.Modified;
            db.SaveChangesWithAudit(_httpContextAccessor);
            return gardenSite;
        }

        public void DeleteGarden(int GardenSiteId)
        {
            using var db = _factory.CreateDbContext();
            GardenSite gardenSite = db.GardenSite.Find(GardenSiteId);
            if (gardenSite != null)
            {
                db.GardenSite.Remove(gardenSite);
                db.SaveChanges(); // No audit needed for deletion
            }
        }

        public IEnumerable<TreePlanting> GetTreePlantings(int GardenSiteId)
        {
            using var db = _factory.CreateDbContext();
            return db.TreePlanting
                .Where(tp => tp.GardenSiteId == GardenSiteId)
                .OrderBy(tp => tp.PlantingDate)
                .ToList();
        }

        public TreePlanting AddTreePlanting(TreePlanting treePlanting)
        {
            using var db = _factory.CreateDbContext();
            db.TreePlanting.Add(treePlanting);
            db.SaveChangesWithAudit(_httpContextAccessor);
            return treePlanting;
        }

        public TreePlanting UpdateTreePlanting(TreePlanting treePlanting)
        {
            using var db = _factory.CreateDbContext();
            db.Entry(treePlanting).State = EntityState.Modified;
            db.SaveChangesWithAudit(_httpContextAccessor);
            return treePlanting;
        }

        public void DeleteTreePlanting(int TreePlantingId)
        {
            using var db = _factory.CreateDbContext();
            TreePlanting treePlanting = db.TreePlanting.Find(TreePlantingId);
            if (treePlanting != null)
            {
                db.TreePlanting.Remove(treePlanting);
                db.SaveChanges(); // No audit needed for deletion
            }
        }

        public IEnumerable<GardenPhoto> GetGardenPhotos(int GardenSiteId)
        {
            using var db = _factory.CreateDbContext();
            return db.GardenPhoto
                .Where(p => p.GardenSiteId == GardenSiteId)
                .OrderBy(p => p.CreatedOn)
                .ToList();
        }

        public GardenPhoto AddGardenPhoto(GardenPhoto photo)
        {
            using var db = _factory.CreateDbContext();
            db.GardenPhoto.Add(photo);
            db.SaveChangesWithAudit(_httpContextAccessor);
            return photo;
        }

        public void DeleteGardenPhoto(int GardenPhotoId)
        {
            using var db = _factory.CreateDbContext();
            GardenPhoto photo = db.GardenPhoto.Find(GardenPhotoId);
            if (photo != null)
            {
                db.GardenPhoto.Remove(photo);
                db.SaveChanges(); // No audit needed for deletion
            }
        }

        public GardenStatistics GetGardenStatistics(int GardenSiteId)
        {
            using var db = _factory.CreateDbContext();
            
            // Get tree planting statistics using separate queries
            var totalTreesPlanted = db.TreePlanting
                .Where(tp => tp.GardenSiteId == GardenSiteId)
                .Sum(tp => (int?)tp.Quantity) ?? 0;

            var treeSpeciesCount = db.TreePlanting
                .Where(tp => tp.GardenSiteId == GardenSiteId)
                .Select(tp => tp.TreeSpecies)
                .Distinct()
                .Count();

            // Get monitoring session statistics
            var monitoringSessionCount = db.MonitoringSession
                .Where(ms => ms.GardenSiteId == GardenSiteId)
                .Count();

            var lastMonitoringDate = db.MonitoringSession
                .Where(ms => ms.GardenSiteId == GardenSiteId)
                .Max(ms => (DateTime?)ms.SessionDate);

            // Calculate average survival rate from monitoring sessions
            var sessionsWithSurvivalData = db.MonitoringSession
                .Where(ms => ms.GardenSiteId == GardenSiteId && 
                           ms.TreesPlanted.HasValue && ms.TreesPlanted > 0 && 
                           ms.TreesAlive.HasValue)
                .Select(ms => new { TreesPlanted = ms.TreesPlanted.Value, TreesAlive = ms.TreesAlive.Value })
                .ToList();

            double? averageSurvivalRate = null;
            if (sessionsWithSurvivalData.Any())
            {
                averageSurvivalRate = sessionsWithSurvivalData
                    .Average(ms => (double)ms.TreesAlive / ms.TreesPlanted * 100);
            }

            // Determine if intervention is required
            var requiresIntervention = false;
            var interventionReason = "";

            if (averageSurvivalRate.HasValue && averageSurvivalRate < 70)
            {
                requiresIntervention = true;
                interventionReason = "Low survival rate";
            }
            else if (lastMonitoringDate.HasValue && lastMonitoringDate < DateTime.Now.AddMonths(-3))
            {
                requiresIntervention = true;
                interventionReason = "No recent monitoring";
            }

            return new GardenStatistics
            {
                TotalTreesPlanted = totalTreesPlanted,
                TreeSpeciesCount = treeSpeciesCount,
                MonitoringSessionCount = monitoringSessionCount,
                LastMonitoringDate = lastMonitoringDate,
                AverageSurvivalRate = averageSurvivalRate,
                RequiresIntervention = requiresIntervention,
                InterventionReason = interventionReason
            };
        }

        public IEnumerable<GardenListItemVm> GetGardenListItems()
        {
            using var db = _factory.CreateDbContext();
            
            // Main query with joins but no navigation properties
            var gardenQuery = from gardenSite in db.GardenSite
                             join application in db.TreePlantingApplication on gardenSite.ApplicationId equals application.ApplicationId
                             select new
                             {
                                 gardenSite.GardenSiteId,
                                 gardenSite.BeneficiaryName,
                                 gardenSite.EvaluatorName,
                                 application.Village,
                                 gardenSite.Status,
                                 gardenSite.PlantingDate,
                                 gardenSite.LastMonitoringDate
                             };

            var gardens = gardenQuery.ToList();

            // Get monitoring session counts separately
            var monitoringCounts = db.MonitoringSession
                .Where(ms => ms.GardenSiteId.HasValue)
                .GroupBy(ms => ms.GardenSiteId.Value)
                .Select(g => new { GardenSiteId = g.Key, Count = g.Count() })
                .ToDictionary(x => x.GardenSiteId, x => x.Count);

            // Get tree planting statistics separately
            var treePlantingStats = db.TreePlanting
                .GroupBy(tp => tp.GardenSiteId)
                .Select(g => new
                {
                    GardenSiteId = g.Key,
                    SpeciesCount = g.Select(tp => tp.TreeSpecies).Distinct().Count(),
                    TotalTrees = g.Sum(tp => tp.Quantity)
                })
                .ToDictionary(x => x.GardenSiteId, x => new { x.SpeciesCount, x.TotalTrees });

            return gardens.Select(g => new GardenListItemVm
            {
                GardenSiteId = g.GardenSiteId,
                BeneficiaryName = g.BeneficiaryName,
                EvaluatorName = g.EvaluatorName,
                Village = g.Village,
                Status = g.Status,
                PlantingDate = g.PlantingDate,
                LastMonitoringDate = g.LastMonitoringDate,
                MonitoringSessionCount = monitoringCounts.GetValueOrDefault(g.GardenSiteId, 0),
                TreeSpeciesCount = treePlantingStats.ContainsKey(g.GardenSiteId) ? treePlantingStats[g.GardenSiteId].SpeciesCount : 0,
                TotalTreesPlanted = treePlantingStats.ContainsKey(g.GardenSiteId) ? treePlantingStats[g.GardenSiteId].TotalTrees : 0,
                RequiresAttention = (g.LastMonitoringDate.HasValue && g.LastMonitoringDate < DateTime.Now.AddMonths(-2)) ||
                                   g.Status == GardenStatus.RequiresIntervention
            }).OrderByDescending(g => g.PlantingDate ?? g.LastMonitoringDate ?? DateTime.MinValue).ToList();
        }

        public GardenSite CreateGardenFromApplication(CreateGardenFromApplicationRequest request)
        {
            using var db = _factory.CreateDbContext();
            
            // Get the approved application without navigation properties
            var application = db.TreePlantingApplication.Find(request.ApplicationId);
            if (application == null || application.Status != ApplicationStatus.Approved)
            {
                throw new InvalidOperationException("Can only create gardens from approved applications");
            }

            // Check if garden already exists for this application
            var existingGarden = db.GardenSite.FirstOrDefault(g => g.ApplicationId == request.ApplicationId);
            if (existingGarden != null)
            {
                throw new InvalidOperationException("A garden already exists for this application");
            }

            var gardenSite = new GardenSite
            {
                ApplicationId = request.ApplicationId,
                ModuleId = application.ModuleId,
                EvaluatorName = request.EvaluatorName ?? application.EvaluatorName,
                BeneficiaryName = application.BeneficiaryName,
                HouseNumber = request.HouseNumber,
                IdOrBirthdate = request.IdOrBirthdate,
                HasWaterInPlot = request.HasWaterInPlot,
                HasWaterCatchmentSystem = request.HasWaterCatchmentSystem,
                NumberOfExistingTrees = request.NumberOfExistingTrees,
                NumberOfIndigenousTrees = request.NumberOfIndigenousTrees,
                NumberOfFruitNutTrees = request.NumberOfFruitNutTrees,
                HasSpaceForMoreTrees = request.HasSpaceForMoreTrees,
                IsPropertyFenced = request.IsPropertyFenced,
                HasCompostOrMulchResources = request.HasCompostOrMulchResources,
                Village = application.Village,
                Address = application.Address,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                Notes = request.Notes,
                Status = GardenStatus.Active
            };

            db.GardenSite.Add(gardenSite);
            db.SaveChangesWithAudit(_httpContextAccessor);
            
            return gardenSite;
        }
    }
}