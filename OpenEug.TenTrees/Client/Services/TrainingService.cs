using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Oqtane.Services;
using Oqtane.Shared;
using OpenEug.TenTrees.Models;

namespace OpenEug.TenTrees.Module.Training.Services
{
    public class TrainingService : ServiceBase, ITrainingService
    {
        public TrainingService(HttpClient http, SiteState siteState) : base(http, siteState) { }

        private string ApiUrl => CreateApiUrl("Training");

        public async Task<List<TrainingClass>> GetTrainingClassesAsync(int moduleId, int? villageId = null)
        {
            string url = $"{ApiUrl}?moduleid={moduleId}";
            if (villageId.HasValue)
            {
                url += $"&villageid={villageId.Value}";
            }
            List<TrainingClass> classes = await GetJsonAsync<List<TrainingClass>>(CreateAuthorizationPolicyUrl(url, EntityNames.Module, moduleId), Enumerable.Empty<TrainingClass>().ToList());
            return classes.OrderByDescending(c => c.ClassDate).ToList();
        }

        public async Task<TrainingClass> GetTrainingClassAsync(int classId, int moduleId)
        {
            return await GetJsonAsync<TrainingClass>(CreateAuthorizationPolicyUrl($"{ApiUrl}/{classId}?moduleid={moduleId}", EntityNames.Module, moduleId));
        }

        public async Task<TrainingClass> AddTrainingClassAsync(TrainingClass trainingClass)
        {
            return await PostJsonAsync<TrainingClass>(CreateAuthorizationPolicyUrl($"{ApiUrl}", EntityNames.Module, trainingClass.ModuleId), trainingClass);
        }

        public async Task<TrainingClass> UpdateTrainingClassAsync(TrainingClass trainingClass)
        {
            return await PutJsonAsync<TrainingClass>(CreateAuthorizationPolicyUrl($"{ApiUrl}/{trainingClass.TrainingClassId}", EntityNames.Module, trainingClass.ModuleId), trainingClass);
        }

        public async Task DeleteTrainingClassAsync(int classId, int moduleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{ApiUrl}/{classId}/{moduleId}", EntityNames.Module, moduleId));
        }

        public async Task<List<ClassAttendance>> GetAttendanceForClassAsync(int classId, int moduleId)
        {
            return await GetJsonAsync<List<ClassAttendance>>(CreateAuthorizationPolicyUrl($"{ApiUrl}/attendance/{classId}?moduleid={moduleId}", EntityNames.Module, moduleId), Enumerable.Empty<ClassAttendance>().ToList());
        }

        public async Task MarkAttendanceAsync(MarkAttendanceRequest request)
        {
            await PostJsonAsync(CreateAuthorizationPolicyUrl($"{ApiUrl}/attendance", EntityNames.Module, request.ModuleId), request);
        }

        public async Task<List<AttendanceSummaryViewModel>> GetAttendanceSummariesAsync(int moduleId, int? villageId = null)
        {
            string url = $"{ApiUrl}/summaries?moduleid={moduleId}";
            if (villageId.HasValue)
            {
                url += $"&villageid={villageId.Value}";
            }
            return await GetJsonAsync<List<AttendanceSummaryViewModel>>(CreateAuthorizationPolicyUrl(url, EntityNames.Module, moduleId), Enumerable.Empty<AttendanceSummaryViewModel>().ToList());
        }

        public async Task<AttendanceSummaryViewModel> GetGrowerAttendanceSummaryAsync(int growerId, int moduleId)
        {
            return await GetJsonAsync<AttendanceSummaryViewModel>(CreateAuthorizationPolicyUrl($"{ApiUrl}/grower-summary/{growerId}?moduleid={moduleId}", EntityNames.Module, moduleId));
        }

        public async Task<TrainingStatusSummary> GetTrainingStatusSummaryAsync(int moduleId, int? villageId = null)
        {
            string url = $"{ApiUrl}/status-summary?moduleid={moduleId}";
            if (villageId.HasValue)
            {
                url += $"&villageid={villageId.Value}";
            }
            return await GetJsonAsync<TrainingStatusSummary>(CreateAuthorizationPolicyUrl(url, EntityNames.Module, moduleId));
        }
    }
}
