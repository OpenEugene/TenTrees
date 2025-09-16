using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oqtane.Modules;
using Oqtane.Repository;
using Oqtane.Infrastructure;
using OE.TenTrees.Models;

namespace OE.TenTrees.Repository
{
    public class Context : DBContextBase, ITransientService
    {
        public virtual DbSet<Models.MyModule> MyModule { get; set; }
        public virtual DbSet<TreePlantingApplication> TreePlantingApplication { get; set; }
        public virtual DbSet<ApplicationDocument> ApplicationDocument { get; set; }
        public virtual DbSet<ApplicationStatusHistory> ApplicationStatusHistory { get; set; }
        public virtual DbSet<ApplicationReview> ApplicationReview { get; set; }
        public virtual DbSet<ReviewChecklistItem> ReviewChecklistItem { get; set; }
        public virtual DbSet<SiteAssessment> SiteAssessment { get; set; }
        public virtual DbSet<AssessmentPhoto> AssessmentPhoto { get; set; }
        public virtual DbSet<MonitoringSession> MonitoringSession { get; set; }
        public virtual DbSet<MonitoringMetric> MonitoringMetric { get; set; }
        public virtual DbSet<MonitoringPhoto> MonitoringPhoto { get; set; }
        
        // Garden-related entities
        public virtual DbSet<GardenSite> GardenSite { get; set; }
        public virtual DbSet<TreePlanting> TreePlanting { get; set; }
        public virtual DbSet<GardenPhoto> GardenPhoto { get; set; }

        public Context(IDBContextDependencies DBContextDependencies) : base(DBContextDependencies)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure Application relationships - foreign keys only, no navigation properties
            builder.Entity<ApplicationDocument>()
                .HasOne<TreePlantingApplication>()
                .WithMany()
                .HasForeignKey(d => d.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationStatusHistory>()
                .HasOne<TreePlantingApplication>()
                .WithMany()
                .HasForeignKey(h => h.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Review relationships
            builder.Entity<ApplicationReview>()
                .HasOne<TreePlantingApplication>()
                .WithMany()
                .HasForeignKey(r => r.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ReviewChecklistItem>()
                .HasOne<ApplicationReview>()
                .WithMany()
                .HasForeignKey(c => c.ReviewId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Assessment relationships
            builder.Entity<SiteAssessment>()
                .HasOne<TreePlantingApplication>()
                .WithMany()
                .HasForeignKey(a => a.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<AssessmentPhoto>()
                .HasOne<SiteAssessment>()
                .WithMany()
                .HasForeignKey(p => p.AssessmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Monitoring relationships
            builder.Entity<MonitoringSession>()
                .HasOne<TreePlantingApplication>()
                .WithMany()
                .HasForeignKey(ms => ms.ApplicationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<MonitoringSession>()
                .HasOne<GardenSite>()
                .WithMany()
                .HasForeignKey(ms => ms.GardenSiteId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<MonitoringMetric>()
                .HasOne<MonitoringSession>()
                .WithMany()
                .HasForeignKey(m => m.MonitoringSessionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MonitoringPhoto>()
                .HasOne<MonitoringSession>()
                .WithMany()
                .HasForeignKey(p => p.MonitoringSessionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure GardenSite relationships
            builder.Entity<GardenSite>()
                .HasOne<TreePlantingApplication>()
                .WithMany()
                .HasForeignKey(g => g.ApplicationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TreePlanting>()
                .HasOne<GardenSite>()
                .WithMany()
                .HasForeignKey(tp => tp.GardenSiteId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<GardenPhoto>()
                .HasOne<GardenSite>()
                .WithMany()
                .HasForeignKey(p => p.GardenSiteId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure decimal precision for coordinates
            builder.Entity<GardenSite>()
                .Property(g => g.Latitude)
                .HasPrecision(18, 6);

            builder.Entity<GardenSite>()
                .Property(g => g.Longitude)
                .HasPrecision(18, 6);

            // Configure enum storage
            builder.Entity<GardenSite>()
                .Property(g => g.Status)
                .HasConversion<int>();

            builder.Entity<TreePlanting>()
                .Property(tp => tp.Status)
                .HasConversion<int>();

            builder.Entity<GardenPhoto>()
                .Property(gp => gp.PhotoType)
                .HasConversion<int>();

            // Ensure unique garden site per application
            builder.Entity<GardenSite>()
                .HasIndex(g => g.ApplicationId)
                .IsUnique();
        }
    }
}
