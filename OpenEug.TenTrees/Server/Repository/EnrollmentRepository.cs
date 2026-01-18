using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Oqtane.Modules;

namespace OpenEug.TenTrees.Module.Enrollment.Repository
{
    public interface IEnrollmentRepository
    {
        IEnumerable<Models.Enrollment> GetEnrollments(int ModuleId);
        IEnumerable<Models.Enrollment> GetEnrollments();
        Models.Enrollment GetEnrollment(int EnrollmentId);
        Models.Enrollment GetEnrollment(int EnrollmentId, bool tracking);
        Models.Enrollment AddEnrollment(Models.Enrollment Enrollment);
        Models.Enrollment UpdateEnrollment(Models.Enrollment Enrollment);
        void DeleteEnrollment(int EnrollmentId);
    }

    public class EnrollmentRepository : IEnrollmentRepository, ITransientService
    {
        private readonly IDbContextFactory<EnrollmentContext> _factory;

        public EnrollmentRepository(IDbContextFactory<EnrollmentContext> factory)
        {
            _factory = factory;
        }

        public IEnumerable<Models.Enrollment> GetEnrollments(int ModuleId)
        {
            using var db = _factory.CreateDbContext();
            return db.Enrollment.Where(item => item.ModuleId == ModuleId).ToList();
        }
        
        public IEnumerable<Models.Enrollment> GetEnrollments()
        {
            using var db = _factory.CreateDbContext();
            return db.Enrollment.ToList();
        }

        public Models.Enrollment GetEnrollment(int EnrollmentId)
        {
            return GetEnrollment(EnrollmentId, true);
        }

        public Models.Enrollment GetEnrollment(int EnrollmentId, bool tracking)
        {
            using var db = _factory.CreateDbContext();
            if (tracking)
            {
                return db.Enrollment.Find(EnrollmentId);
            }
            else
            {
                return db.Enrollment.AsNoTracking().FirstOrDefault(item => item.EnrollmentId == EnrollmentId);
            }
        }

        public Models.Enrollment AddEnrollment(Models.Enrollment Enrollment)
        {
            using var db = _factory.CreateDbContext();
            db.Enrollment.Add(Enrollment);
            db.SaveChanges();
            return Enrollment;
        }

        public Models.Enrollment UpdateEnrollment(Models.Enrollment Enrollment)
        {
            using var db = _factory.CreateDbContext();
            db.Entry(Enrollment).State = EntityState.Modified;
            db.SaveChanges();
            return Enrollment;
        }

        public void DeleteEnrollment(int EnrollmentId)
        {
            using var db = _factory.CreateDbContext();
            Models.Enrollment Enrollment = db.Enrollment.Find(EnrollmentId);
            db.Enrollment.Remove(Enrollment);
            db.SaveChanges();
        }
    }
}
