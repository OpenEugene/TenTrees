using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Oqtane.Modules;
using OpenEug.TenTrees.Models;

namespace OpenEug.TenTrees.Module.Training.Repository
{
    public interface ITrainingRepository
    {
        // Training Classes
        IEnumerable<TrainingClass> GetTrainingClasses(int moduleId, int? villageId = null);
        TrainingClass GetTrainingClass(int classId);
        TrainingClass AddTrainingClass(TrainingClass trainingClass);
        TrainingClass UpdateTrainingClass(TrainingClass trainingClass);
        void DeleteTrainingClass(int classId);

        // Attendance
        IEnumerable<ClassAttendance> GetAttendanceForClass(int classId);
        void MarkAttendance(List<ClassAttendance> attendanceRecords, int trainingClassId);

        // Summaries
        IEnumerable<AttendanceSummaryViewModel> GetAttendanceSummaries(int moduleId, int? villageId = null);
        AttendanceSummaryViewModel GetGrowerAttendanceSummary(int growerId, int moduleId);
        TrainingStatusSummary GetTrainingStatusSummary(int moduleId, int? villageId = null);
    }

    public class TrainingRepository : ITrainingRepository, ITransientService
    {
        private const int RequiredClasses = 5;
        private readonly IDbContextFactory<OpenEug.TenTrees.Repository.TenTreesContext> _factory;

        public TrainingRepository(IDbContextFactory<OpenEug.TenTrees.Repository.TenTreesContext> factory)
        {
            _factory = factory;
        }

        public IEnumerable<TrainingClass> GetTrainingClasses(int moduleId, int? villageId = null)
        {
            using var db = _factory.CreateDbContext();
            var query = db.TrainingClass.Where(tc => tc.ModuleId == moduleId);

            if (villageId.HasValue)
            {
                query = query.Where(tc => tc.VillageId == villageId.Value);
            }

            return query.OrderByDescending(tc => tc.ClassDate).ToList();
        }

        public TrainingClass GetTrainingClass(int classId)
        {
            using var db = _factory.CreateDbContext();
            return db.TrainingClass.Find(classId);
        }

        public TrainingClass AddTrainingClass(TrainingClass trainingClass)
        {
            using var db = _factory.CreateDbContext();
            db.TrainingClass.Add(trainingClass);
            db.SaveChanges();
            return trainingClass;
        }

        public TrainingClass UpdateTrainingClass(TrainingClass trainingClass)
        {
            using var db = _factory.CreateDbContext();
            db.Entry(trainingClass).State = EntityState.Modified;
            db.SaveChanges();
            return trainingClass;
        }

        public void DeleteTrainingClass(int classId)
        {
            using var db = _factory.CreateDbContext();
            var trainingClass = db.TrainingClass.Find(classId);
            if (trainingClass != null)
            {
                // Remove related attendance records first
                var attendanceRecords = db.ClassAttendance.Where(ca => ca.TrainingClassId == classId);
                db.ClassAttendance.RemoveRange(attendanceRecords);
                db.TrainingClass.Remove(trainingClass);
                db.SaveChanges();
            }
        }

        public IEnumerable<ClassAttendance> GetAttendanceForClass(int classId)
        {
            using var db = _factory.CreateDbContext();
            return db.ClassAttendance.Where(ca => ca.TrainingClassId == classId).ToList();
        }

        public void MarkAttendance(List<ClassAttendance> attendanceRecords, int trainingClassId)
        {
            using var db = _factory.CreateDbContext();

            // Remove existing attendance for this class
            var existing = db.ClassAttendance.Where(ca => ca.TrainingClassId == trainingClassId);
            db.ClassAttendance.RemoveRange(existing);

            // Add new attendance records
            foreach (var record in attendanceRecords)
            {
                db.ClassAttendance.Add(record);
            }

            db.SaveChanges();
        }

        public IEnumerable<AttendanceSummaryViewModel> GetAttendanceSummaries(int moduleId, int? villageId = null)
        {
            using var db = _factory.CreateDbContext();

            var growerQuery = db.Grower.Where(g => g.Status == GrowerStatus.Active);
            if (villageId.HasValue)
            {
                growerQuery = growerQuery.Where(g => g.VillageId == villageId.Value);
            }

            var moduleClassIds = db.TrainingClass
                .Where(tc => tc.ModuleId == moduleId)
                .Select(tc => tc.TrainingClassId)
                .ToList();

            var summaries = from g in growerQuery
                            join v in db.Village on g.VillageId equals v.VillageId into villageJoin
                            from v in villageJoin.DefaultIfEmpty()
                            select new AttendanceSummaryViewModel
                            {
                                GrowerId = g.GrowerId,
                                GrowerName = g.GrowerName,
                                VillageId = g.VillageId,
                                VillageName = v != null ? v.VillageName : null,
                                ClassesAttended = db.ClassAttendance
                                    .Count(ca => ca.GrowerId == g.GrowerId
                                        && ca.IsPresent
                                        && moduleClassIds.Contains(ca.TrainingClassId)),
                                TotalRequired = RequiredClasses
                            };

            var result = summaries.ToList();

            foreach (var s in result)
            {
                s.IsEligible = s.ClassesAttended >= RequiredClasses;
            }

            return result;
        }

        public AttendanceSummaryViewModel GetGrowerAttendanceSummary(int growerId, int moduleId)
        {
            using var db = _factory.CreateDbContext();

            var grower = db.Grower.Find(growerId);
            if (grower == null) return null;

            var village = db.Village.Find(grower.VillageId);

            var moduleClassIds = db.TrainingClass
                .Where(tc => tc.ModuleId == moduleId)
                .Select(tc => tc.TrainingClassId)
                .ToList();

            var classesAttended = db.ClassAttendance
                .Count(ca => ca.GrowerId == growerId
                    && ca.IsPresent
                    && moduleClassIds.Contains(ca.TrainingClassId));

            return new AttendanceSummaryViewModel
            {
                GrowerId = grower.GrowerId,
                GrowerName = grower.GrowerName,
                VillageId = grower.VillageId,
                VillageName = village?.VillageName,
                ClassesAttended = classesAttended,
                TotalRequired = RequiredClasses,
                IsEligible = classesAttended >= RequiredClasses,
                StatusDisplay = classesAttended >= RequiredClasses
                    ? "Eligible for trees"
                    : $"{RequiredClasses - classesAttended} classes remaining"
            };
        }

        public TrainingStatusSummary GetTrainingStatusSummary(int moduleId, int? villageId = null)
        {
            var summaries = GetAttendanceSummaries(moduleId, villageId).ToList();
            return new TrainingStatusSummary
            {
                Eligible = summaries.Count(s => s.IsEligible),
                InProgress = summaries.Count(s => s.ClassesAttended > 0 && !s.IsEligible),
                NotStarted = summaries.Count(s => s.ClassesAttended == 0),
                Total = summaries.Count
            };
        }
    }
}
