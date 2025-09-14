using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Oqtane.Modules;
using OE.TenTrees.Models;

namespace OE.TenTrees.Repository
{
    public class TreeRepository : ITreeRepository, ITransientService
    {
        private readonly IDbContextFactory<TenTreesContext> _factory;

        public TreeRepository(IDbContextFactory<TenTreesContext> factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<Tree>> GetTreesAsync(int ModuleId)
        {
            using var db = _factory.CreateDbContext();
            return await db.Tree.Where(item => item.ModuleId == ModuleId).ToListAsync();
        }

        public async Task<Tree?> GetTreeAsync(int TreeId, int ModuleId)
        {
            using var db = _factory.CreateDbContext();
            return await db.Tree.Where(item => item.TreeId == TreeId && item.ModuleId == ModuleId).FirstOrDefaultAsync();
        }

        public async Task<Tree> AddTreeAsync(Tree tree)
        {
            using var db = _factory.CreateDbContext();
            db.Tree.Add(tree);
            await db.SaveChangesAsync();
            return tree;
        }

        public async Task<Tree> UpdateTreeAsync(Tree tree)
        {
            using var db = _factory.CreateDbContext();
            db.Entry(tree).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return tree;
        }

        public async Task DeleteTreeAsync(int TreeId, int ModuleId)
        {
            using var db = _factory.CreateDbContext();
            var tree = await db.Tree.Where(item => item.TreeId == TreeId && item.ModuleId == ModuleId).FirstOrDefaultAsync();
            if (tree != null)
            {
                db.Tree.Remove(tree);
                await db.SaveChangesAsync();
            }
        }
    }
}