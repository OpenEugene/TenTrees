using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Oqtane.Modules;
using Oqtane.Repository;
using Oqtane.Infrastructure;
using Oqtane.Repository.Databases.Interfaces;

namespace OpenEug.TenTrees.Module.Enrollment.Repository
{
    public class EnrollmentContext : DBContextBase, ITransientService, IMultiDatabase
    {
        public virtual DbSet<Models.Enrollment> Enrollment { get; set; }

        public EnrollmentContext(IDBContextDependencies DBContextDependencies) : base(DBContextDependencies)
        {
            // ContextBase handles multi-tenant database connections
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Models.Enrollment>().ToTable(ActiveDatabase.RewriteName("OpenEug.TenTreesEnrollment"));
        }
    }
}
