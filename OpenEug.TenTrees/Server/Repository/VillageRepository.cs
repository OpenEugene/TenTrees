using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Oqtane.Modules;
using Models = OpenEug.TenTrees.Models;

namespace OpenEug.TenTrees.Module.Village.Repository
{
    public interface IVillageRepository
    {
        IEnumerable<Models.Village> GetVillages();
        Models.Village GetVillage(int villageId);
        Models.Village GetVillage(int villageId, bool tracking);
        Models.Village AddVillage(Models.Village village);
        Models.Village UpdateVillage(Models.Village village);
        void DeleteVillage(int villageId);
    }

    public class VillageRepository : IVillageRepository, ITransientService
    {
        private readonly IDbContextFactory<OpenEug.TenTrees.Repository.TenTreesContext> _factory;

        public VillageRepository(IDbContextFactory<OpenEug.TenTrees.Repository.TenTreesContext> factory)
        {
            _factory = factory;
        }

        public IEnumerable<Models.Village> GetVillages()
        {
            using var db = _factory.CreateDbContext();
            return db.Village.ToList();
        }

        public Models.Village GetVillage(int villageId)
        {
            return GetVillage(villageId, true);
        }

        public Models.Village GetVillage(int villageId, bool tracking)
        {
            using var db = _factory.CreateDbContext();
            if (tracking)
            {
                return db.Village.Find(villageId);
            }
            else
            {
                return db.Village.AsNoTracking().FirstOrDefault(item => item.VillageId == villageId);
            }
        }

        public Models.Village AddVillage(Models.Village village)
        {
            using var db = _factory.CreateDbContext();
            db.Village.Add(village);
            db.SaveChanges();
            return village;
        }

        public Models.Village UpdateVillage(Models.Village village)
        {
            using var db = _factory.CreateDbContext();
            db.Entry(village).State = EntityState.Modified;
            db.SaveChanges();
            return village;
        }

        public void DeleteVillage(int villageId)
        {
            using var db = _factory.CreateDbContext();
            Models.Village village = db.Village.Find(villageId);
            if (village == null)
            {
                return;
            }
            db.Village.Remove(village);
            db.SaveChanges();
        }
    }
}
