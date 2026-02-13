using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Oqtane.Services;
using Oqtane.Shared;

namespace OpenEug.TenTrees.Module.Enrollment.Services
{
    public interface IEnrollmentService 
    {
        Task<List<Models.Enrollment>> GetEnrollmentsAsync(int moduleId);

        Task<Models.Enrollment> GetEnrollmentAsync(int enrollmentId, int moduleId);

        Task<Models.Enrollment> AddEnrollmentAsync(Models.Enrollment enrollment);

        Task<Models.Enrollment> UpdateEnrollmentAsync(Models.Enrollment enrollment);

        Task DeleteEnrollmentAsync(int enrollmentId, int moduleId);
        
        // BDD-Specified Actions
        Task<ValidationResult> ValidateRequiredAsync(Models.Enrollment Enrollment);
        
        Task<bool> CaptureSignatureAsync(int EnrollmentId, string SignatureData);
        
        Task<MentorInfo> AutoFillMentorAsync(int UserId);
        
        Task<List<Models.Enrollment>> GetByStatusAsync(int ModuleId, Models.EnrollmentStatus Status);
        
        
        Task<List<Models.Enrollment>> GetByVillageAsync(int villageId);
    }

    public class EnrollmentService : ServiceBase, IEnrollmentService
    {
        public EnrollmentService(HttpClient http, SiteState siteState) : base(http, siteState) { }

        private string Apiurl => CreateApiUrl("Enrollment");

        public async Task<List<Models.Enrollment>> GetEnrollmentsAsync(int moduleId)
        {
            List<Models.Enrollment> enrollments = await GetJsonAsync<List<Models.Enrollment>>(CreateAuthorizationPolicyUrl($"{Apiurl}?moduleid={moduleId}", EntityNames.Module, moduleId), Enumerable.Empty<Models.Enrollment>().ToList());
            return enrollments.OrderBy(item => item.GrowerName).ToList();
        }

        public async Task<Models.Enrollment> GetEnrollmentAsync(int enrollmentId, int moduleId)
        {
            return await GetJsonAsync<Models.Enrollment>(CreateAuthorizationPolicyUrl($"{Apiurl}/{enrollmentId}/{moduleId}", EntityNames.Module, moduleId));
        }

        public async Task<Models.Enrollment> AddEnrollmentAsync(Models.Enrollment enrollment)
        {
            return await PostJsonAsync<Models.Enrollment>(CreateAuthorizationPolicyUrl($"{Apiurl}", EntityNames.Module, enrollment.ModuleId), enrollment);
        }

        public async Task<Models.Enrollment> UpdateEnrollmentAsync(Models.Enrollment enrollment)
        {
            return await PutJsonAsync<Models.Enrollment>(CreateAuthorizationPolicyUrl($"{Apiurl}/{enrollment.EnrollmentId}", EntityNames.Module, enrollment.ModuleId), enrollment);
        }

        public async Task DeleteEnrollmentAsync(int enrollmentId, int moduleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{Apiurl}/{enrollmentId}/{moduleId}", EntityNames.Module, moduleId));
        }
        
        
        public async Task<ValidationResult> ValidateRequiredAsync(Models.Enrollment enrollment)
        {
            return await PostJsonAsync<Models.Enrollment, ValidationResult>(CreateAuthorizationPolicyUrl($"{Apiurl}/validate", EntityNames.Module, enrollment.ModuleId), enrollment);
        }
        
        public async Task<bool> CaptureSignatureAsync(int enrollmentId, string signatureData)
        {
            return await PostJsonAsync<SignatureRequest, bool>($"{Apiurl}/{enrollmentId}/signature", new SignatureRequest { ModuleId = 0, SignatureData = signatureData });
        }
        
        public async Task<MentorInfo> AutoFillMentorAsync(int userId)
        {
            return await GetJsonAsync<MentorInfo>($"{Apiurl}/mentor/{userId}");
        }
        
        public async Task<List<Models.Enrollment>> GetByStatusAsync(int moduleId, Models.EnrollmentStatus status)
        {
            return await GetJsonAsync<List<Models.Enrollment>>(CreateAuthorizationPolicyUrl($"{Apiurl}/status/{status}?moduleid={moduleId}", EntityNames.Module, moduleId));
        }
        
        public async Task<List<Models.Enrollment>> GetByVillageAsync(int villageId)
        {
            return await GetJsonAsync<List<Models.Enrollment>>($"{Apiurl}/village/{villageId}");
        }
    }
    
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; }
    }
    
    public class MentorInfo
    {
        public int MentorId { get; set; }
        public string TreeMentorName { get; set; }
        public int VillageId { get; set; }
    }
    
    public class SignatureRequest
    {
        public int ModuleId { get; set; }
        public string SignatureData { get; set; }
    }
}
