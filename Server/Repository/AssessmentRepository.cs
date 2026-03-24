using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Oqtane.Modules;
using OpenEug.TenTrees.Models;

namespace OpenEug.TenTrees.Module.Assessment.Repository
{
    public interface IAssessmentRepository
    {
        IEnumerable<Models.Assessment> GetAssessments();
        IEnumerable<Models.Assessment> GetAssessmentsByGrower(int growerId);
        Models.Assessment GetAssessment(int assessmentId);
        Models.Assessment GetAssessment(int assessmentId, bool tracking);
        Models.Assessment AddAssessment(Models.Assessment assessment);
        Models.Assessment UpdateAssessment(Models.Assessment assessment);
        void DeleteAssessment(int assessmentId);
    }

    public class AssessmentRepository : IAssessmentRepository, ITransientService
    {
        private readonly IDbContextFactory<OpenEug.TenTrees.Repository.TenTreesContext> _factory;

        public AssessmentRepository(IDbContextFactory<OpenEug.TenTrees.Repository.TenTreesContext> factory)
        {
            _factory = factory;
        }

        public IEnumerable<Models.Assessment> GetAssessments()
        {
            using var db = _factory.CreateDbContext();
            var list = db.Assessment.ToList();
            return list;
        }

        public IEnumerable<Models.Assessment> GetAssessmentsByGrower(int growerId)
        {
            using var db = _factory.CreateDbContext();
            return db.Assessment.Where(item => item.GrowerId == growerId).ToList();
        }

        public Models.Assessment GetAssessment(int assessmentId)
        {
            return GetAssessment(assessmentId, true);
        }

        public Models.Assessment GetAssessment(int assessmentId, bool tracking)
        {
            using var db = _factory.CreateDbContext();
            if (tracking)
            {
                return db.Assessment.Find(assessmentId);
            }
            else
            {
                return db.Assessment.AsNoTracking().FirstOrDefault(item => item.AssessmentId == assessmentId);
            }
        }

        public Models.Assessment AddAssessment(Models.Assessment assessment)
        {
            using var db = _factory.CreateDbContext();
            db.Assessment.Add(assessment);
            db.SaveChanges();
            return assessment;
        }

        public Models.Assessment UpdateAssessment(Models.Assessment assessment)
        {
            using var db = _factory.CreateDbContext();
            db.Entry(assessment).State = EntityState.Modified;
            db.SaveChanges();
            return assessment;
        }

        public void DeleteAssessment(int assessmentId)
        {
            using var db = _factory.CreateDbContext();
            var assessment = db.Assessment.Find(assessmentId);
            if (assessment != null)
            {
                db.Assessment.Remove(assessment);
                db.SaveChanges();
            }
        }
    }
}
