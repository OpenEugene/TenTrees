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
        Task<List<Models.Enrollment>> GetEnrollmentsAsync(int ModuleId);

        Task<Models.Enrollment> GetEnrollmentAsync(int EnrollmentId, int ModuleId);

        Task<Models.Enrollment> AddEnrollmentAsync(Models.Enrollment Enrollment);

        Task<Models.Enrollment> UpdateEnrollmentAsync(Models.Enrollment Enrollment);

        Task DeleteEnrollmentAsync(int EnrollmentId, int ModuleId);
        
        // BDD-Specified Actions
        Task<ValidationResult> ValidateRequiredAsync(Models.Enrollment Enrollment);
        
        Task<bool> CaptureSignatureAsync(int EnrollmentId, string SignatureData);
        
        Task<MentorInfo> AutoFillMentorAsync(int UserId);
        
        Task<List<Models.Enrollment>> GetByStatusAsync(int ModuleId, Models.EnrollmentStatus Status);
        
        Task<List<Models.Enrollment>> GetByVillageAsync(int VillageId);
    }

    public class EnrollmentService : ServiceBase, IEnrollmentService
    {
        public EnrollmentService(HttpClient http, SiteState siteState) : base(http, siteState) { }

        private string Apiurl => CreateApiUrl("Enrollment");

        public async Task<List<Models.Enrollment>> GetEnrollmentsAsync(int ModuleId)
        {
            List<Models.Enrollment> Enrollments = await GetJsonAsync<List<Models.Enrollment>>(CreateAuthorizationPolicyUrl($"{Apiurl}?moduleid={ModuleId}", EntityNames.Module, ModuleId), Enumerable.Empty<Models.Enrollment>().ToList());
            return Enrollments.OrderBy(item => item.BeneficiaryName).ToList();
        }

        public async Task<Models.Enrollment> GetEnrollmentAsync(int EnrollmentId, int ModuleId)
        {
            return await GetJsonAsync<Models.Enrollment>(CreateAuthorizationPolicyUrl($"{Apiurl}/{EnrollmentId}/{ModuleId}", EntityNames.Module, ModuleId));
        }

        public async Task<Models.Enrollment> AddEnrollmentAsync(Models.Enrollment Enrollment)
        {
            return await PostJsonAsync<Models.Enrollment>(CreateAuthorizationPolicyUrl($"{Apiurl}", EntityNames.Module, Enrollment.ModuleId), Enrollment);
        }

        public async Task<Models.Enrollment> UpdateEnrollmentAsync(Models.Enrollment Enrollment)
        {
            return await PutJsonAsync<Models.Enrollment>(CreateAuthorizationPolicyUrl($"{Apiurl}/{Enrollment.EnrollmentId}", EntityNames.Module, Enrollment.ModuleId), Enrollment);
        }

        public async Task DeleteEnrollmentAsync(int EnrollmentId, int ModuleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{Apiurl}/{EnrollmentId}/{ModuleId}", EntityNames.Module, ModuleId));
        }
        
        
        public async Task<ValidationResult> ValidateRequiredAsync(Models.Enrollment Enrollment)
        {
            return await PostJsonAsync<Models.Enrollment, ValidationResult>(CreateAuthorizationPolicyUrl($"{Apiurl}/validate", EntityNames.Module, Enrollment.ModuleId), Enrollment);
        }
        
        public async Task<bool> CaptureSignatureAsync(int EnrollmentId, string SignatureData)
        {
            return await PostJsonAsync<SignatureRequest, bool>($"{Apiurl}/{EnrollmentId}/signature", new SignatureRequest { ModuleId = 0, SignatureData = SignatureData });
        }
        
        public async Task<MentorInfo> AutoFillMentorAsync(int UserId)
        {
            return await GetJsonAsync<MentorInfo>($"{Apiurl}/mentor/{UserId}");
        }
        
        public async Task<List<Models.Enrollment>> GetByStatusAsync(int ModuleId, Models.EnrollmentStatus Status)
        {
            return await GetJsonAsync<List<Models.Enrollment>>(CreateAuthorizationPolicyUrl($"{Apiurl}/status/{Status}?moduleid={ModuleId}", EntityNames.Module, ModuleId));
        }
        
        public async Task<List<Models.Enrollment>> GetByVillageAsync(int VillageId)
        {
            return await GetJsonAsync<List<Models.Enrollment>>($"{Apiurl}/village/{VillageId}");
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
        public string EvaluatorName { get; set; }
        public int VillageId { get; set; }
    }
    
    public class SignatureRequest
    {
        public int ModuleId { get; set; }
        public string SignatureData { get; set; }
    }
}
