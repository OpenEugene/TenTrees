using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Oqtane.Modules;
using OpenEug.TenTrees.Models;

namespace OpenEug.TenTrees.Module.Enrollment.Repository
{
    public interface IEnrollmentRepository
    {
        IEnumerable<Models.Enrollment> GetEnrollments(int moduleId);
        IEnumerable<Models.Enrollment> GetEnrollments();
        IEnumerable<EnrollmentListViewModel> GetEnrollmentListViewModels(int moduleId);
        Models.Enrollment GetEnrollment(int enrollmentId);
        Models.Enrollment GetEnrollment(int enrollmentId, bool tracking);
        Models.Enrollment AddEnrollment(Models.Enrollment enrollment);
        Models.Enrollment UpdateEnrollment(Models.Enrollment enrollment);
        void DeleteEnrollment(int enrollmentId);
        int BackfillGrowersFromEnrollments();
    }

    public class EnrollmentRepository : IEnrollmentRepository, ITransientService
    {
        private readonly IDbContextFactory<OpenEug.TenTrees.Repository.TenTreesContext> _factory;

        public EnrollmentRepository(IDbContextFactory<OpenEug.TenTrees.Repository.TenTreesContext> factory)
        {
            _factory = factory;
        }

        public IEnumerable<Models.Enrollment> GetEnrollments(int moduleId)
        {
            using var db = _factory.CreateDbContext();
            return db.Enrollment.Where(item => item.ModuleId == moduleId).ToList();
        }

        public IEnumerable<Models.Enrollment> GetEnrollments()
        {
            using var db = _factory.CreateDbContext();
            return db.Enrollment.ToList();
        }

        public IEnumerable<EnrollmentListViewModel> GetEnrollmentListViewModels(int moduleId)
        {
            using var db = _factory.CreateDbContext();
            var viewModels = from e in db.Enrollment
                           join v in db.Village on e.VillageId equals v.VillageId into villageJoin
                           from v in villageJoin.DefaultIfEmpty()
                           join g in db.Grower on e.GrowerId equals g.GrowerId into growerJoin
                           from g in growerJoin.DefaultIfEmpty()
                           where e.ModuleId == moduleId
                           select new EnrollmentListViewModel
                           {
                               EnrollmentId = e.EnrollmentId,
                               GrowerId = e.GrowerId,
                               GrowerName = e.GrowerName,
                               TreeMentorName = e.TreeMentorName,
                               VillageName = v != null ? v.VillageName : null,
                               EnrollmentStatus = e.Status,
                               GrowerStatus = g != null ? g.Status : GrowerStatus.Active
                           };
            return viewModels.ToList();
        }

        public Models.Enrollment GetEnrollment(int enrollmentId)
        {
            return GetEnrollment(enrollmentId, true);
        }

        public Models.Enrollment GetEnrollment(int enrollmentId, bool tracking)
        {
            using var db = _factory.CreateDbContext();
            if (tracking)
            {
                return db.Enrollment.Find(enrollmentId);
            }
            else
            {
                return db.Enrollment.AsNoTracking().FirstOrDefault(item => item.EnrollmentId == enrollmentId);
            }
        }

        public Models.Enrollment AddEnrollment(Models.Enrollment enrollment)
        {
            using var db = _factory.CreateDbContext();

            // Set default status to Pending for new enrollments
            enrollment.Status = EnrollmentStatus.Pending;

            // Check if grower already exists (by name and village)
            var existingGrower = db.Grower
                .FirstOrDefault(g => g.GrowerName == enrollment.GrowerName && g.VillageId == enrollment.VillageId);

            if (existingGrower != null)
            {
                // Link to existing grower
                enrollment.GrowerId = existingGrower.GrowerId;
            }
            else
            {
                // Create new Grower record from enrollment data
                var grower = new Models.Grower
                {
                    GrowerName = enrollment.GrowerName,
                    VillageId = enrollment.VillageId,
                    MentorId = enrollment.MentorId,
                    HouseNumber = enrollment.HouseNumber,
                    IdNumber = enrollment.IdNumber,
                    BirthDate = enrollment.BirthDate,
                    HouseholdSize = enrollment.HouseholdSize,
                    OwnsHome = enrollment.OwnsHome,
                    Status = GrowerStatus.Active, // New growers are active by default
                    CreatedBy = enrollment.CreatedBy,
                    CreatedOn = enrollment.CreatedOn,
                    ModifiedBy = enrollment.ModifiedBy,
                    ModifiedOn = enrollment.ModifiedOn
                };

                db.Grower.Add(grower);
                db.SaveChanges(); // Save to get GrowerId

                // Link enrollment to new grower
                enrollment.GrowerId = grower.GrowerId;
            }

            db.Enrollment.Add(enrollment);
            db.SaveChanges();
            return enrollment;
        }

        public Models.Enrollment UpdateEnrollment(Models.Enrollment enrollment)
        {
            using var db = _factory.CreateDbContext();

            // If enrollment is linked to a grower, update grower details too
            if (enrollment.GrowerId.HasValue)
            {
                var grower = db.Grower.Find(enrollment.GrowerId.Value);
                if (grower != null)
                {
                    // Update grower details from enrollment
                    grower.GrowerName = enrollment.GrowerName;
                    grower.VillageId = enrollment.VillageId;
                    grower.MentorId = enrollment.MentorId;
                    grower.HouseNumber = enrollment.HouseNumber;
                    grower.IdNumber = enrollment.IdNumber;
                    grower.BirthDate = enrollment.BirthDate;
                    grower.HouseholdSize = enrollment.HouseholdSize;
                    grower.OwnsHome = enrollment.OwnsHome;
                    grower.ModifiedBy = enrollment.ModifiedBy;
                    grower.ModifiedOn = enrollment.ModifiedOn;

                    db.Entry(grower).State = EntityState.Modified;
                }
            }

            db.Entry(enrollment).State = EntityState.Modified;
            db.SaveChanges();
            return enrollment;
        }

        public void DeleteEnrollment(int enrollmentId)
        {
            using var db = _factory.CreateDbContext();
            Models.Enrollment enrollment = db.Enrollment.Find(enrollmentId);
            db.Enrollment.Remove(enrollment);
            db.SaveChanges();
        }

        /// <summary>
        /// Backfill Grower records for existing Enrollments that don't have a linked Grower.
        /// Use this for data migration after adding Grower table.
        /// </summary>
        public int BackfillGrowersFromEnrollments()
        {
            using var db = _factory.CreateDbContext();

            // Find all enrollments without a linked grower
            var enrollmentsWithoutGrower = db.Enrollment
                .Where(e => e.GrowerId == null)
                .ToList();

            int created = 0;

            foreach (var enrollment in enrollmentsWithoutGrower)
            {
                // Check if grower already exists (by name and village)
                var existingGrower = db.Grower
                    .FirstOrDefault(g => g.GrowerName == enrollment.GrowerName && g.VillageId == enrollment.VillageId);

                if (existingGrower != null)
                {
                    // Link to existing grower
                    enrollment.GrowerId = existingGrower.GrowerId;
                }
                else
                {
                    // Create new Grower from enrollment data
                    var grower = new Models.Grower
                    {
                        GrowerName = enrollment.GrowerName,
                        VillageId = enrollment.VillageId,
                        MentorId = enrollment.MentorId,
                        HouseNumber = enrollment.HouseNumber,
                        IdNumber = enrollment.IdNumber,
                        BirthDate = enrollment.BirthDate,
                        HouseholdSize = enrollment.HouseholdSize,
                        OwnsHome = enrollment.OwnsHome,
                        Status = GrowerStatus.Active,
                        CreatedBy = enrollment.CreatedBy,
                        CreatedOn = enrollment.CreatedOn,
                        ModifiedBy = enrollment.ModifiedBy,
                        ModifiedOn = enrollment.ModifiedOn
                    };

                    db.Grower.Add(grower);
                    db.SaveChanges(); // Save to get GrowerId

                    enrollment.GrowerId = grower.GrowerId;
                    created++;
                }
            }

            db.SaveChanges();
            return created;
        }
    }
}
