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
        IEnumerable<EnrollmentViewModel> GetEnrollmentViewModels(int moduleId);
        Models.Enrollment GetEnrollment(int enrollmentId);
        Models.Enrollment GetEnrollment(int enrollmentId, bool tracking);
        Models.Enrollment AddEnrollment(Models.Enrollment enrollment);
        Models.Enrollment UpdateEnrollment(Models.Enrollment enrollment);
        void DeleteEnrollment(int enrollmentId);
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

        public IEnumerable<EnrollmentViewModel> GetEnrollmentViewModels(int moduleId)
        {
            using var db = _factory.CreateDbContext();
            var viewModels = from e in db.Enrollment
                           join v in db.Village on e.VillageId equals v.VillageId into villageJoin
                           from v in villageJoin.DefaultIfEmpty()
                           where e.ModuleId == moduleId
                           select new EnrollmentViewModel
                           {
                               EnrollmentId = e.EnrollmentId,
                               ModuleId = e.ModuleId,
                               GrowerName = e.GrowerName,
                               VillageId = e.VillageId,
                               VillageName = v != null ? v.VillageName : "Unknown",
                               HouseNumber = e.HouseNumber,
                               IdNumber = e.IdNumber,
                               BirthDate = e.BirthDate,
                               OwnsHome = e.OwnsHome,
                               HouseholdSize = e.HouseholdSize,
                               EnrollmentDate = e.EnrollmentDate,
                               MentorId = e.MentorId,
                               TreeMentorName = e.TreeMentorName,
                               EnrolledInPE = e.EnrolledInPE,
                               PEGraduate = e.PEGraduate,
                               GardenPlantedAndTended = e.GardenPlantedAndTended,
                               ChildHeadedHousehold = e.ChildHeadedHousehold,
                               WomanHeadedHousehold = e.WomanHeadedHousehold,
                               EmptyYard = e.EmptyYard,
                               CommitNoChemicals = e.CommitNoChemicals,
                               CommitAttendClasses = e.CommitAttendClasses,
                               CommitNoCuttingTrees = e.CommitNoCuttingTrees,
                               CommitStandForWomenChildren = e.CommitStandForWomenChildren,
                               CommitCareWhileAway = e.CommitCareWhileAway,
                               CommitAllowYardAccess = e.CommitAllowYardAccess,
                               SignatureData = e.SignatureData,
                               SignatureCollected = e.SignatureCollected,
                               SignatureDate = e.SignatureDate,
                               Status = e.Status,
                               CreatedBy = e.CreatedBy,
                               CreatedOn = e.CreatedOn,
                               ModifiedBy = e.ModifiedBy,
                               ModifiedOn = e.ModifiedOn
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
            db.Enrollment.Add(enrollment);
            db.SaveChanges();
            return enrollment;
        }

        public Models.Enrollment UpdateEnrollment(Models.Enrollment enrollment)
        {
            using var db = _factory.CreateDbContext();
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
    }
}
