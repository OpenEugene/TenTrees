using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Oqtane.Modules;
using Models = OpenEug.TenTrees.Models;

namespace OpenEug.TenTrees.Module.Grower.Repository
{
    public interface IOrchardRepository
    {
        IEnumerable<Models.Orchard> GetOrchardsByGrower(int growerId);
        Models.Orchard GetOrchard(int orchardId);
        Models.Orchard GetOrDefaultOrchard(int growerId);
        Models.Orchard AddOrchard(Models.Orchard orchard);
        IEnumerable<Models.Tree> GetTrees(int orchardId);
        Models.Tree AddTree(Models.Tree tree);
        void DeleteTree(int treeId);
    }

    public class OrchardRepository : IOrchardRepository, ITransientService
    {
        private readonly IDbContextFactory<OpenEug.TenTrees.Repository.TenTreesContext> _factory;

        public OrchardRepository(IDbContextFactory<OpenEug.TenTrees.Repository.TenTreesContext> factory)
        {
            _factory = factory;
        }

        public IEnumerable<Models.Orchard> GetOrchardsByGrower(int growerId)
        {
            using var db = _factory.CreateDbContext();
            return db.Orchard.AsNoTracking().Where(o => o.GrowerId == growerId).ToList();
        }

        public Models.Orchard GetOrchard(int orchardId)
        {
            using var db = _factory.CreateDbContext();
            return db.Orchard.Find(orchardId);
        }

        public Models.Orchard GetOrDefaultOrchard(int growerId)
        {
            using var db = _factory.CreateDbContext();
            return db.Orchard.AsNoTracking().FirstOrDefault(o => o.GrowerId == growerId);
        }

        public Models.Orchard AddOrchard(Models.Orchard orchard)
        {
            using var db = _factory.CreateDbContext();
            db.Orchard.Add(orchard);
            db.SaveChanges();
            return orchard;
        }

        public IEnumerable<Models.Tree> GetTrees(int orchardId)
        {
            using var db = _factory.CreateDbContext();
            return db.Tree.AsNoTracking().Where(t => t.OrchardId == orchardId).ToList();
        }

        public Models.Tree AddTree(Models.Tree tree)
        {
            using var db = _factory.CreateDbContext();
            db.Tree.Add(tree);
            db.SaveChanges();
            return tree;
        }

        public void DeleteTree(int treeId)
        {
            using var db = _factory.CreateDbContext();
            var item = db.Tree.Find(treeId);
            if (item != null)
            {
                db.Tree.Remove(item);
                db.SaveChanges();
            }
        }
    }
}
