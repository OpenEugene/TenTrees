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
        public virtual DbSet<Models.Grower> Grower { get; set; }
        public virtual DbSet<Models.TrainingClass> TrainingClass { get; set; }
        public virtual DbSet<Models.ClassAttendance> ClassAttendance { get; set; }
        public virtual DbSet<Models.Assessment> Assessment { get; set; }
        public virtual DbSet<Models.AssessmentNote> AssessmentNote { get; set; }
        public virtual DbSet<Models.Cohort> Cohort { get; set; }
        public virtual DbSet<Models.GrowerCohort> GrowerCohort { get; set; }
        public virtual DbSet<Models.MentorCohort> MentorCohort { get; set; }
        public virtual DbSet<Models.CohortClass> CohortClass { get; set; }
        public virtual DbSet<Models.TreeType> TreeType { get; set; }
        public virtual DbSet<Models.Orchard> Orchard { get; set; }
        public virtual DbSet<Models.Tree> Tree { get; set; }
        public virtual DbSet<Models.AssessmentProblem> AssessmentProblem { get; set; }
        public virtual DbSet<Models.AssessmentPhoto> AssessmentPhoto { get; set; }

        public TenTreesContext(IDBContextDependencies dependencies) : base(dependencies)
        {
            // ContextBase handles multi-tenant database connections
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Models.Village>().ToTable(ActiveDatabase.RewriteName("Village"));
            modelBuilder.Entity<Models.Enrollment>().ToTable(ActiveDatabase.RewriteName("Enrollment"));
            modelBuilder.Entity<Models.Grower>().ToTable(ActiveDatabase.RewriteName("Grower"));
            modelBuilder.Entity<Models.TrainingClass>().ToTable(ActiveDatabase.RewriteName("TrainingClass"));
            modelBuilder.Entity<Models.ClassAttendance>().ToTable(ActiveDatabase.RewriteName("ClassAttendance"));
            modelBuilder.Entity<Models.Assessment>().ToTable(ActiveDatabase.RewriteName("Assessment"));
            modelBuilder.Entity<Models.AssessmentNote>().ToTable(ActiveDatabase.RewriteName("AssessmentNote"));
            modelBuilder.Entity<Models.Cohort>().ToTable(ActiveDatabase.RewriteName("Cohort"));
            modelBuilder.Entity<Models.GrowerCohort>().ToTable(ActiveDatabase.RewriteName("GrowerCohort"));
            modelBuilder.Entity<Models.MentorCohort>().ToTable(ActiveDatabase.RewriteName("MentorCohort"));
            modelBuilder.Entity<Models.CohortClass>().ToTable(ActiveDatabase.RewriteName("CohortClass"));
            modelBuilder.Entity<Models.TreeType>().ToTable(ActiveDatabase.RewriteName("TreeType"));
            modelBuilder.Entity<Models.Orchard>().ToTable(ActiveDatabase.RewriteName("Orchard"));
            modelBuilder.Entity<Models.Tree>().ToTable(ActiveDatabase.RewriteName("Tree"));
            modelBuilder.Entity<Models.AssessmentProblem>().ToTable(ActiveDatabase.RewriteName("AssessmentProblem"));
            modelBuilder.Entity<Models.AssessmentPhoto>().ToTable(ActiveDatabase.RewriteName("AssessmentPhoto"));
        }
    }
}
