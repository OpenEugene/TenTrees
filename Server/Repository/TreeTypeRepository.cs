using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Oqtane.Modules;
using Models = OpenEug.TenTrees.Models;

namespace OpenEug.TenTrees.Module.TreeType.Repository
{
    public interface ITreeTypeRepository
    {
        IEnumerable<Models.TreeType> GetTreeTypes();
        Models.TreeType GetTreeType(int treeTypeId);
        Models.TreeType GetTreeType(int treeTypeId, bool tracking);
        Models.TreeType AddTreeType(Models.TreeType treeType);
        Models.TreeType UpdateTreeType(Models.TreeType treeType);
        void DeleteTreeType(int treeTypeId);
    }

    public class TreeTypeRepository : ITreeTypeRepository, ITransientService
    {
        private readonly IDbContextFactory<OpenEug.TenTrees.Repository.TenTreesContext> _factory;

        public TreeTypeRepository(IDbContextFactory<OpenEug.TenTrees.Repository.TenTreesContext> factory)
        {
            _factory = factory;
        }

        public IEnumerable<Models.TreeType> GetTreeTypes()
        {
            using var db = _factory.CreateDbContext();
            return db.TreeType.ToList();
        }

        public Models.TreeType GetTreeType(int treeTypeId)
        {
            return GetTreeType(treeTypeId, true);
        }

        public Models.TreeType GetTreeType(int treeTypeId, bool tracking)
        {
            using var db = _factory.CreateDbContext();
            if (tracking)
            {
                return db.TreeType.Find(treeTypeId);
            }
            else
            {
                return db.TreeType.AsNoTracking().FirstOrDefault(item => item.TreeTypeId == treeTypeId);
            }
        }

        public Models.TreeType AddTreeType(Models.TreeType treeType)
        {
            using var db = _factory.CreateDbContext();
            db.TreeType.Add(treeType);
            db.SaveChanges();
            return treeType;
        }

        public Models.TreeType UpdateTreeType(Models.TreeType treeType)
        {
            using var db = _factory.CreateDbContext();
            db.Entry(treeType).State = EntityState.Modified;
            db.SaveChanges();
            return treeType;
        }

        public void DeleteTreeType(int treeTypeId)
        {
            using var db = _factory.CreateDbContext();
            Models.TreeType treeType = db.TreeType.Find(treeTypeId);
            if (treeType == null)
            {
                return;
            }
            db.TreeType.Remove(treeType);
            db.SaveChanges();
        }
    }
}
