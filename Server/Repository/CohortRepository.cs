using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Oqtane.Modules;
using Models = OpenEug.TenTrees.Models;

namespace OpenEug.TenTrees.Module.Cohort.Repository
{
    public interface ICohortRepository
    {
        // Cohort CRUD
        IEnumerable<Models.Cohort> GetCohorts();
        IEnumerable<Models.Cohort> GetCohortsByVillage(int villageId);
        Models.Cohort GetCohort(int cohortId);
        Models.Cohort AddCohort(Models.Cohort cohort);
        Models.Cohort UpdateCohort(Models.Cohort cohort);
        void DeleteCohort(int cohortId);

        // Cohort name suggestion support
        int CountCohortsForVillageYear(int villageId, int year);

        // Grower membership
        IEnumerable<Models.GrowerCohort> GetGrowerCohorts(int cohortId);
        IEnumerable<Models.Cohort> GetCohortsByGrower(int growerId);
        Models.GrowerCohort AddGrowerCohort(int growerId, int cohortId);
        void DeleteGrowerCohort(int growerId, int cohortId);

        // Mentor assignment
        IEnumerable<Models.MentorCohort> GetMentorCohorts(int cohortId);
        IEnumerable<Models.Cohort> GetCohortsByMentor(string mentorId);
        Models.MentorCohort AddMentorCohort(string mentorId, int cohortId);
        void DeleteMentorCohort(string mentorId, int cohortId);

        // Class association
        IEnumerable<Models.CohortClass> GetClassesForCohort(int cohortId);
        IEnumerable<Models.CohortClass> GetCohortsForClass(int trainingClassId);
        Models.CohortClass AddCohortClass(int cohortId, int trainingClassId);
        void DeleteCohortClass(int cohortId, int trainingClassId);
    }

    public class CohortRepository : ICohortRepository, ITransientService
    {
        private readonly IDbContextFactory<OpenEug.TenTrees.Repository.TenTreesContext> _factory;

        public CohortRepository(IDbContextFactory<OpenEug.TenTrees.Repository.TenTreesContext> factory)
        {
            _factory = factory;
        }

        public IEnumerable<Models.Cohort> GetCohorts()
        {
            using var db = _factory.CreateDbContext();
            return db.Cohort.OrderBy(c => c.Name).ToList();
        }

        public IEnumerable<Models.Cohort> GetCohortsByVillage(int villageId)
        {
            using var db = _factory.CreateDbContext();
            return db.Cohort.Where(c => c.VillageId == villageId).OrderBy(c => c.Name).ToList();
        }

        public Models.Cohort GetCohort(int cohortId)
        {
            using var db = _factory.CreateDbContext();
            return db.Cohort.Find(cohortId);
        }

        public Models.Cohort AddCohort(Models.Cohort cohort)
        {
            using var db = _factory.CreateDbContext();
            db.Cohort.Add(cohort);
            db.SaveChanges();
            return cohort;
        }

        public Models.Cohort UpdateCohort(Models.Cohort cohort)
        {
            using var db = _factory.CreateDbContext();
            db.Entry(cohort).State = EntityState.Modified;
            db.SaveChanges();
            return cohort;
        }

        public void DeleteCohort(int cohortId)
        {
            using var db = _factory.CreateDbContext();
            var cohort = db.Cohort.Find(cohortId);
            if (cohort == null) return;
            db.Cohort.Remove(cohort);
            db.SaveChanges();
        }

        public int CountCohortsForVillageYear(int villageId, int year)
        {
            using var db = _factory.CreateDbContext();
            // Match cohorts whose program year (based on activation date) is the given year for this village
            return db.Cohort.Count(c =>
                c.VillageId == villageId &&
                c.ActivatedOn.HasValue &&
                c.ActivatedOn.Value.Year == year);
        }

        // ── Grower membership ──────────────────────────────────────────────────

        public IEnumerable<Models.GrowerCohort> GetGrowerCohorts(int cohortId)
        {
            using var db = _factory.CreateDbContext();
            return db.GrowerCohort.Where(gc => gc.CohortId == cohortId).ToList();
        }

        public IEnumerable<Models.Cohort> GetCohortsByGrower(int growerId)
        {
            using var db = _factory.CreateDbContext();
            var cohortIds = db.GrowerCohort.Where(gc => gc.GrowerId == growerId).Select(gc => gc.CohortId);
            return db.Cohort.Where(c => cohortIds.Contains(c.CohortId)).OrderBy(c => c.Name).ToList();
        }

        public Models.GrowerCohort AddGrowerCohort(int growerId, int cohortId)
        {
            using var db = _factory.CreateDbContext();
            var existing = db.GrowerCohort.FirstOrDefault(gc => gc.GrowerId == growerId && gc.CohortId == cohortId);
            if (existing != null) return existing;
            var gc = new Models.GrowerCohort { GrowerId = growerId, CohortId = cohortId, JoinedOn = DateTime.UtcNow };
            db.GrowerCohort.Add(gc);
            db.SaveChanges();
            return gc;
        }

        public void DeleteGrowerCohort(int growerId, int cohortId)
        {
            using var db = _factory.CreateDbContext();
            var gc = db.GrowerCohort.FirstOrDefault(x => x.GrowerId == growerId && x.CohortId == cohortId);
            if (gc == null) return;
            db.GrowerCohort.Remove(gc);
            db.SaveChanges();
        }

        // ── Mentor assignment ──────────────────────────────────────────────────

        public IEnumerable<Models.MentorCohort> GetMentorCohorts(int cohortId)
        {
            using var db = _factory.CreateDbContext();
            return db.MentorCohort.Where(mc => mc.CohortId == cohortId).ToList();
        }

        public IEnumerable<Models.Cohort> GetCohortsByMentor(string mentorId)
        {
            using var db = _factory.CreateDbContext();
            var cohortIds = db.MentorCohort.Where(mc => mc.MentorId == mentorId).Select(mc => mc.CohortId);
            return db.Cohort.Where(c => cohortIds.Contains(c.CohortId)).OrderBy(c => c.Name).ToList();
        }

        public Models.MentorCohort AddMentorCohort(string mentorId, int cohortId)
        {
            using var db = _factory.CreateDbContext();
            var existing = db.MentorCohort.FirstOrDefault(mc => mc.MentorId == mentorId && mc.CohortId == cohortId);
            if (existing != null) return existing;
            var mc = new Models.MentorCohort { MentorId = mentorId, CohortId = cohortId, AssignedOn = DateTime.UtcNow };
            db.MentorCohort.Add(mc);
            db.SaveChanges();
            return mc;
        }

        public void DeleteMentorCohort(string mentorId, int cohortId)
        {
            using var db = _factory.CreateDbContext();
            var mc = db.MentorCohort.FirstOrDefault(x => x.MentorId == mentorId && x.CohortId == cohortId);
            if (mc == null) return;
            db.MentorCohort.Remove(mc);
            db.SaveChanges();
        }

        // ── Class association ──────────────────────────────────────────────────

        public IEnumerable<Models.CohortClass> GetClassesForCohort(int cohortId)
        {
            using var db = _factory.CreateDbContext();
            return db.CohortClass.Where(cc => cc.CohortId == cohortId).ToList();
        }

        public IEnumerable<Models.CohortClass> GetCohortsForClass(int trainingClassId)
        {
            using var db = _factory.CreateDbContext();
            return db.CohortClass.Where(cc => cc.TrainingClassId == trainingClassId).ToList();
        }

        public Models.CohortClass AddCohortClass(int cohortId, int trainingClassId)
        {
            using var db = _factory.CreateDbContext();
            var existing = db.CohortClass.FirstOrDefault(cc => cc.CohortId == cohortId && cc.TrainingClassId == trainingClassId);
            if (existing != null) return existing;
            var cc = new Models.CohortClass { CohortId = cohortId, TrainingClassId = trainingClassId };
            db.CohortClass.Add(cc);
            db.SaveChanges();
            return cc;
        }

        public void DeleteCohortClass(int cohortId, int trainingClassId)
        {
            using var db = _factory.CreateDbContext();
            var cc = db.CohortClass.FirstOrDefault(x => x.CohortId == cohortId && x.TrainingClassId == trainingClassId);
            if (cc == null) return;
            db.CohortClass.Remove(cc);
            db.SaveChanges();
        }
    }
}
