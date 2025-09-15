using Microsoft.EntityFrameworkCore;
using Oqtane.Modules;
using Oqtane.Repository;
using Oqtane.Repository.Databases.Interfaces;

namespace OE.TenTrees.Repository
{
    public class Context : DBContextBase, ITransientService, IMultiDatabase
    {
        public virtual DbSet<Models.MyModule> MyModule { get; set; }
        
        // Application entities
        public virtual DbSet<Models.TreePlantingApplication> TreePlantingApplication { get; set; }
        public virtual DbSet<Models.ApplicationDocument> ApplicationDocument { get; set; }
        public virtual DbSet<Models.ApplicationStatusHistory> ApplicationStatusHistory { get; set; }
        public virtual DbSet<Models.ApplicationReview> ApplicationReview { get; set; }
        public virtual DbSet<Models.ReviewChecklistItem> ReviewChecklistItem { get; set; }
        
        // Assessment entities
        public virtual DbSet<Models.SiteAssessment> SiteAssessment { get; set; }
        public virtual DbSet<Models.AssessmentPhoto> AssessmentPhoto { get; set; }
        
        // Monitoring entities
        public virtual DbSet<Models.MonitoringSession> MonitoringSession { get; set; }
        public virtual DbSet<Models.MonitoringMetric> MonitoringMetric { get; set; }
        public virtual DbSet<Models.MonitoringPhoto> MonitoringPhoto { get; set; }

        public Context(IDBContextDependencies DBContextDependencies) : base(DBContextDependencies)
        {
            // ContextBase handles multi-tenant database connections
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Models.MyModule>().ToTable(ActiveDatabase.RewriteName("MyModule"));
            
            // Application tables
            builder.Entity<Models.TreePlantingApplication>().ToTable(ActiveDatabase.RewriteName("TreePlantingApplication"));
            builder.Entity<Models.ApplicationDocument>().ToTable(ActiveDatabase.RewriteName("ApplicationDocument"));
            builder.Entity<Models.ApplicationStatusHistory>().ToTable(ActiveDatabase.RewriteName("ApplicationStatusHistory"));
            builder.Entity<Models.ApplicationReview>().ToTable(ActiveDatabase.RewriteName("ApplicationReview"));
            builder.Entity<Models.ReviewChecklistItem>().ToTable(ActiveDatabase.RewriteName("ReviewChecklistItem"));
            
            // Assessment tables
            builder.Entity<Models.SiteAssessment>().ToTable(ActiveDatabase.RewriteName("SiteAssessment"));
            builder.Entity<Models.AssessmentPhoto>().ToTable(ActiveDatabase.RewriteName("AssessmentPhoto"));
            
            // Monitoring tables
            builder.Entity<Models.MonitoringSession>().ToTable(ActiveDatabase.RewriteName("MonitoringSession"));
            builder.Entity<Models.MonitoringMetric>().ToTable(ActiveDatabase.RewriteName("MonitoringMetric"));
            builder.Entity<Models.MonitoringPhoto>().ToTable(ActiveDatabase.RewriteName("MonitoringPhoto"));
            
            // Configure relationships
            builder.Entity<Models.ApplicationDocument>()
                .HasOne<Models.TreePlantingApplication>()
                .WithMany(a => a.Documents)
                .HasForeignKey(d => d.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.Entity<Models.ApplicationStatusHistory>()
                .HasOne<Models.TreePlantingApplication>()
                .WithMany(a => a.History)
                .HasForeignKey(h => h.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.Entity<Models.ApplicationReview>()
                .HasOne<Models.TreePlantingApplication>()
                .WithMany()
                .HasForeignKey(r => r.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.Entity<Models.ReviewChecklistItem>()
                .HasOne<Models.ApplicationReview>()
                .WithMany(r => r.Checklist)
                .HasForeignKey(c => c.ReviewId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.Entity<Models.SiteAssessment>()
                .HasOne<Models.TreePlantingApplication>()
                .WithMany()
                .HasForeignKey(s => s.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.Entity<Models.AssessmentPhoto>()
                .HasOne<Models.SiteAssessment>()
                .WithMany(a => a.Photos)
                .HasForeignKey(p => p.AssessmentId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // Monitoring tables - no EF relationship configuration, just foreign key constraints in database
        }
    }
}
