using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Oqtane.Modules;
using OpenEug.TenTrees.Models;

namespace OpenEug.TenTrees.Module.Grower.Repository
{
    public interface IGrowerRepository
    {
        Models.Grower GetGrower(int growerId);
        Models.Grower GetGrower(int growerId, bool tracking);
        IEnumerable<Models.Grower> GetAllGrowers(int? villageId = null);
        IEnumerable<Models.Grower> GetGrowersByVillage(int villageId);
        IEnumerable<Models.Grower> GetGrowersByStatus(GrowerStatus status, int? villageId = null);
        IEnumerable<Models.Grower> GetActiveGrowers(int? villageId = null);
        Models.Grower AddGrower(Models.Grower grower);
        Models.Grower UpdateGrower(Models.Grower grower);
        void DeleteGrower(int growerId);
        GrowerStatusSummary GetStatusSummary(int? villageId = null);
    }

    public class GrowerRepository : IGrowerRepository, ITransientService
    {
        private readonly IDbContextFactory<OpenEug.TenTrees.Repository.TenTreesContext> _factory;

        public GrowerRepository(IDbContextFactory<OpenEug.TenTrees.Repository.TenTreesContext> factory)
        {
            _factory = factory;
        }

        public Models.Grower GetGrower(int growerId)
        {
            return GetGrower(growerId, true);
        }

        public Models.Grower GetGrower(int growerId, bool tracking)
        {
            using var db = _factory.CreateDbContext();
            if (tracking)
            {
                return db.Grower.Find(growerId);
            }
            else
            {
                return db.Grower.AsNoTracking().FirstOrDefault(item => item.GrowerId == growerId);
            }
        }

        public IEnumerable<Models.Grower> GetAllGrowers(int? villageId = null)
        {
            using var db = _factory.CreateDbContext();
            var query = db.Grower.AsQueryable();

            if (villageId.HasValue)
            {
                query = query.Where(g => g.VillageId == villageId.Value);
            }

            return query.ToList();
        }

        public IEnumerable<Models.Grower> GetGrowersByVillage(int villageId)
        {
            using var db = _factory.CreateDbContext();
            return db.Grower.Where(g => g.VillageId == villageId).ToList();
        }

        public IEnumerable<Models.Grower> GetGrowersByStatus(GrowerStatus status, int? villageId = null)
        {
            using var db = _factory.CreateDbContext();
            var query = db.Grower.Where(g => g.Status == status);
            
            if (villageId.HasValue)
            {
                query = query.Where(g => g.VillageId == villageId.Value);
            }
            
            return query.ToList();
        }

        public IEnumerable<Models.Grower> GetActiveGrowers(int? villageId = null)
        {
            return GetGrowersByStatus(GrowerStatus.Active, villageId);
        }

        public Models.Grower AddGrower(Models.Grower grower)
        {
            using var db = _factory.CreateDbContext();
            db.Grower.Add(grower);
            db.SaveChanges();
            return grower;
        }

        public Models.Grower UpdateGrower(Models.Grower grower)
        {
            using var db = _factory.CreateDbContext();
            db.Entry(grower).State = EntityState.Modified;
            db.SaveChanges();
            return grower;
        }

        public void DeleteGrower(int growerId)
        {
            using var db = _factory.CreateDbContext();
            var grower = db.Grower.Find(growerId);
            if (grower != null)
            {
                db.Grower.Remove(grower);
                db.SaveChanges();
            }
        }

        public GrowerStatusSummary GetStatusSummary(int? villageId = null)
        {
            using var db = _factory.CreateDbContext();
            var query = db.Grower.AsQueryable();
            
            if (villageId.HasValue)
            {
                query = query.Where(g => g.VillageId == villageId.Value);
            }

            var growers = query.ToList();
            
            return new GrowerStatusSummary
            {
                Active = growers.Count(g => g.Status == GrowerStatus.Active),
                Inactive = growers.Count(g => g.Status == GrowerStatus.Inactive),
                Exited = growers.Count(g => g.Status == GrowerStatus.Exited),
                Total = growers.Count
            };
        }
    }
}
