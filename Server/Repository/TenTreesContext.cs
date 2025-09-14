using Microsoft.EntityFrameworkCore;
using OE.TenTrees.Models;
using Oqtane.Modules;
using Oqtane.Repository;

namespace OE.TenTrees.Repository
{
    public class TenTreesContext : DBContextBase, ITransientService, IMultiDatabase
    {
        public virtual DbSet<Tree> Tree { get; set; }
        public virtual DbSet<PlantingEvent> PlantingEvent { get; set; }
        public virtual DbSet<TreeMonitoring> TreeMonitoring { get; set; }

        public TenTreesContext(IDBContextDependencies DBContextDependencies) : base(DBContextDependencies)
        {
            // ContextBase handles multi-tenant database connections
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Tree>().ToTable(ActiveDatabase.RewriteName("OETenTreesTree"));
            builder.Entity<PlantingEvent>().ToTable(ActiveDatabase.RewriteName("OETenTreesPlantingEvent"));
            builder.Entity<TreeMonitoring>().ToTable(ActiveDatabase.RewriteName("OETenTreesTreeMonitoring"));
        }
    }
}