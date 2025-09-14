using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Oqtane.Modules;
using OE.TenTrees.Models;

namespace OE.TenTrees.Repository
{
    public class TenTreesRepository : ITenTreesRepository, ITransientService
    {
        private readonly IDbContextFactory<TenTreesContext> _factory;

        public TenTreesRepository(IDbContextFactory<TenTreesContext> factory)
        {
            _factory = factory;
        }

        public IEnumerable<Tree> GetTrees(int ModuleId)
        {
            using var db = _factory.CreateDbContext();
            return db.Tree.Where(item => item.ModuleId == ModuleId).ToList();
        }

        public Tree AddTree(Tree tree)
        {
            using var db = _factory.CreateDbContext();
            db.Tree.Add(tree);
            db.SaveChanges();
            return tree;
        }
    }
}