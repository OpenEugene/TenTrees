using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Oqtane.Modules;
using Oqtane.Repository;
using Oqtane.Infrastructure;
using Oqtane.Repository.Databases.Interfaces;
using Models = OpenEug.TenTrees.Models;

namespace OpenEug.TenTrees.Repository
{
    public class TenTreesContext : DBContextBase, ITransientService, IMultiDatabase
    {
        public virtual DbSet<Models.Village> Village { get; set; }
        public virtual DbSet<Models.Enrollment> Enrollment { get; set; }
        public virtual DbSet<Models.Participant> Participant { get; set; }

        public TenTreesContext(IDBContextDependencies dependencies) : base(dependencies)
        {
            // ContextBase handles multi-tenant database connections
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Models.Village>().ToTable(ActiveDatabase.RewriteName("Village"));
            modelBuilder.Entity<Models.Enrollment>().ToTable(ActiveDatabase.RewriteName("Enrollment"));
            modelBuilder.Entity<Models.Participant>().ToTable(ActiveDatabase.RewriteName("Participant"));
        }
    }
}
