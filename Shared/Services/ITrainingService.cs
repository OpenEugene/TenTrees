using System.Collections.Generic;
using System.Threading.Tasks;
using OpenEug.TenTrees.Models;

namespace OpenEug.TenTrees.Module.Training.Services
{
    public interface ITrainingService
    {
        // Training Class CRUD
        Task<List<TrainingClass>> GetTrainingClassesAsync(int moduleId, int? villageId = null);
        Task<TrainingClass> GetTrainingClassAsync(int classId, int moduleId);
        Task<TrainingClass> AddTrainingClassAsync(TrainingClass trainingClass);
        Task<TrainingClass> UpdateTrainingClassAsync(TrainingClass trainingClass);
        Task DeleteTrainingClassAsync(int classId, int moduleId);

        // Attendance
        Task<List<ClassAttendance>> GetAttendanceForClassAsync(int classId, int moduleId);
        Task MarkAttendanceAsync(MarkAttendanceRequest request);

        // Summaries
        Task<List<AttendanceSummaryViewModel>> GetAttendanceSummariesAsync(int moduleId, int? villageId = null);
        Task<AttendanceSummaryViewModel> GetGrowerAttendanceSummaryAsync(int growerId, int moduleId);
        Task<TrainingStatusSummary> GetTrainingStatusSummaryAsync(int moduleId, int? villageId = null);
    }
}
