using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oqtane.Modules;
using Oqtane.Repository;
using Oqtane.Infrastructure;
using OE.TenTrees.Models;
using Oqtane.Repository.Databases.Interfaces;

namespace OE.TenTrees.Repository
{
    public class Context : DBContextBase, ITransientService, IMultiDatabase
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

            // Configure Application relationships
            builder.Entity<TreePlantingApplication>()
                .HasMany(a => a.Documents)
                .WithOne()
                .HasForeignKey(d => d.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TreePlantingApplication>()
                .HasMany(a => a.History)
                .WithOne()
                .HasForeignKey(h => h.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Review relationships
            builder.Entity<ApplicationReview>()
                .HasMany(r => r.Checklist)
                .WithOne()
                .HasForeignKey(c => c.ReviewId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Assessment relationships
            builder.Entity<SiteAssessment>()
                .HasMany(a => a.Photos)
                .WithOne()
                .HasForeignKey(p => p.AssessmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Monitoring relationships
            builder.Entity<MonitoringSession>()
                .HasMany(ms => ms.Metrics)
                .WithOne()
                .HasForeignKey(m => m.MonitoringSessionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MonitoringSession>()
                .HasMany(ms => ms.Photos)
                .WithOne()
                .HasForeignKey(p => p.MonitoringSessionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure GardenSite relationships
            builder.Entity<GardenSite>()
                .HasOne(g => g.Application)
                .WithMany()
                .HasForeignKey(g => g.ApplicationId)
                .OnDelete(DeleteBehavior.Restrict); // Don't delete application when garden site is deleted

            builder.Entity<GardenSite>()
                .HasMany(g => g.TreePlantings)
                .WithOne(tp => tp.GardenSite)
                .HasForeignKey(tp => tp.GardenSiteId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<GardenSite>()
                .HasMany(g => g.Photos)
                .WithOne(p => p.GardenSite)
                .HasForeignKey(p => p.GardenSiteId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<GardenSite>()
                .HasMany(g => g.MonitoringSessions)
                .WithOne(ms => ms.GardenSite)
                .HasForeignKey(ms => ms.GardenSiteId)
                .OnDelete(DeleteBehavior.SetNull); // Keep monitoring sessions even if garden site is deleted

            // Configure MonitoringSession to support both Application and GardenSite relationships
            builder.Entity<MonitoringSession>()
                .HasOne<TreePlantingApplication>()
                .WithMany()
                .HasForeignKey(ms => ms.ApplicationId)
                .OnDelete(DeleteBehavior.Restrict);

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
