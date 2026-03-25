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
        IEnumerable<AssessmentListDto> GetAssessmentList(int? villageId = null, int? cohortId = null, string mentorUsername = null, int? growerId = null);
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

        public IEnumerable<AssessmentListDto> GetAssessmentList(int? villageId = null, int? cohortId = null, string mentorUsername = null, int? growerId = null)
        {
            using var db = _factory.CreateDbContext();
            var query = from a in db.Assessment
                        join g in db.Grower on a.GrowerId equals g.GrowerId
                        join v in db.Village on g.VillageId equals v.VillageId
                        select new { a, g, v };

            if (villageId.HasValue)
                query = query.Where(x => x.g.VillageId == villageId.Value);

            if (cohortId.HasValue)
            {
                var cohortGrowerIds = db.GrowerCohort
                    .Where(gc => gc.CohortId == cohortId.Value)
                    .Select(gc => gc.GrowerId);
                query = query.Where(x => cohortGrowerIds.Contains(x.a.GrowerId));
            }

            if (!string.IsNullOrEmpty(mentorUsername))
                query = query.Where(x => x.g.MentorUsername == mentorUsername);

            if (growerId.HasValue)
                query = query.Where(x => x.a.GrowerId == growerId.Value);

            return query.OrderByDescending(x => x.a.AssessmentDate)
                .Select(x => new AssessmentListDto
                {
                    AssessmentId = x.a.AssessmentId,
                    AssessmentDate = x.a.AssessmentDate,
                    GrowerId = x.a.GrowerId,
                    GrowerName = x.g.GrowerName,
                    VillageId = x.v.VillageId,
                    VillageName = x.v.VillageName,
                    MentorUsername = x.g.MentorUsername,
                    TreesPlanted = x.a.TreesPlanted,
                    TreesAlive = x.a.TreesAlive,
                    PermaculturePrinciplesCount = x.a.PermaculturePrinciplesCount,
                    NeedsHelp = x.a.NeedsHelp
                })
                .ToList();
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
