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
        Task<List<Models.Enrollment>> GetEnrollmentsAsync();

        Task<List<Models.EnrollmentListViewModel>> GetEnrollmentListViewModelsAsync();

        Task<Models.Enrollment> GetEnrollmentAsync(int enrollmentId);

        Task<Models.Enrollment> AddEnrollmentAsync(Models.Enrollment enrollment);

        Task<Models.Enrollment> UpdateEnrollmentAsync(Models.Enrollment enrollment);

        Task DeleteEnrollmentAsync(int enrollmentId, int moduleId);

        // BDD-Specified Actions
        Task<ValidationResult> ValidateRequiredAsync(Models.Enrollment Enrollment);

        Task<bool> CaptureSignatureAsync(int EnrollmentId, string SignatureData);

        Task<MentorInfo> AutoFillMentorAsync(int UserId);

        Task<List<Models.Enrollment>> GetByStatusAsync(Models.EnrollmentStatus Status);

        Task<List<Models.Enrollment>> GetByVillageAsync(int villageId);

        Task<int> BackfillGrowersFromEnrollmentsAsync(int moduleId);

        Task<List<UserInfo>> GetSiteUsersAsync();

        Task<bool> CapturePhotoConsentAsync(int enrollmentId, int moduleId, Models.PhotoConsentLevel consentLevel, string signatureData);
    }

    public class EnrollmentService : ServiceBase, IEnrollmentService
    {
        public EnrollmentService(HttpClient http, SiteState siteState) : base(http, siteState) { }

        private string Apiurl => CreateApiUrl("Enrollment");

        public async Task<List<Models.Enrollment>> GetEnrollmentsAsync()
        {
            List<Models.Enrollment> enrollments = await GetJsonAsync<List<Models.Enrollment>>($"{Apiurl}", Enumerable.Empty<Models.Enrollment>().ToList());
            return enrollments.OrderBy(item => item.GrowerName).ToList();
        }

        public async Task<List<Models.EnrollmentListViewModel>> GetEnrollmentListViewModelsAsync()
        {
            List<Models.EnrollmentListViewModel> viewModels = await GetJsonAsync<List<Models.EnrollmentListViewModel>>($"{Apiurl}/listviewmodels", Enumerable.Empty<Models.EnrollmentListViewModel>().ToList());
            return viewModels.OrderBy(item => item.GrowerName).ToList();
        }

        public async Task<Models.Enrollment> GetEnrollmentAsync(int enrollmentId)
        {
            return await GetJsonAsync<Models.Enrollment>($"{Apiurl}/{enrollmentId}");
        }

        public async Task<Models.Enrollment> AddEnrollmentAsync(Models.Enrollment enrollment)
        {
            return await PostJsonAsync<Models.Enrollment>($"{Apiurl}", enrollment);
        }

        public async Task<Models.Enrollment> UpdateEnrollmentAsync(Models.Enrollment enrollment)
        {
            return await PutJsonAsync<Models.Enrollment>($"{Apiurl}/{enrollment.EnrollmentId}", enrollment);
        }

        public async Task DeleteEnrollmentAsync(int enrollmentId, int moduleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{Apiurl}/{enrollmentId}/{moduleId}", EntityNames.Module, moduleId));
        }
        
        
        public async Task<ValidationResult> ValidateRequiredAsync(Models.Enrollment enrollment)
        {
            return await PostJsonAsync<Models.Enrollment, ValidationResult>($"{Apiurl}/validate", enrollment);
        }
        
        public async Task<bool> CaptureSignatureAsync(int enrollmentId, string signatureData)
        {
            return await PostJsonAsync<SignatureRequest, bool>($"{Apiurl}/{enrollmentId}/signature", new SignatureRequest { ModuleId = 0, SignatureData = signatureData });
        }
        
        public async Task<MentorInfo> AutoFillMentorAsync(int userId)
        {
            return await GetJsonAsync<MentorInfo>($"{Apiurl}/mentor/{userId}");
        }
        
        public async Task<List<Models.Enrollment>> GetByStatusAsync(Models.EnrollmentStatus status)
        {
            return await GetJsonAsync<List<Models.Enrollment>>($"{Apiurl}/status/{status}");
        }
        
        public async Task<List<Models.Enrollment>> GetByVillageAsync(int villageId)
        {
            return await GetJsonAsync<List<Models.Enrollment>>($"{Apiurl}/village/{villageId}");
        }

        public async Task<int> BackfillGrowersFromEnrollmentsAsync(int moduleId)
        {
            return await PostJsonAsync<object, int>(CreateAuthorizationPolicyUrl($"{Apiurl}/backfill-growers", EntityNames.Module, moduleId), null);
        }

        public async Task<List<UserInfo>> GetSiteUsersAsync()
        {
            return await GetJsonAsync<List<UserInfo>>($"{Apiurl}/users");
        }

        public async Task<bool> CapturePhotoConsentAsync(int enrollmentId, int moduleId, Models.PhotoConsentLevel consentLevel, string signatureData)
        {
            return await PostJsonAsync<PhotoConsentRequest, bool>(
                CreateAuthorizationPolicyUrl($"{Apiurl}/{enrollmentId}/photoconsent", EntityNames.Module, moduleId),
                new PhotoConsentRequest { ModuleId = moduleId, ConsentLevel = consentLevel, SignatureData = signatureData });
        }
    }

    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; }
    }
    
    public class MentorInfo
    {
        public string MentorUsername { get; set; }
        public string MentorName { get; set; }
        public int VillageId { get; set; }
    }

    public class UserInfo
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
    }
    
    public class SignatureRequest
    {
        public int ModuleId { get; set; }
        public string SignatureData { get; set; }
    }

    public class PhotoConsentRequest
    {
        public int ModuleId { get; set; }
        public Models.PhotoConsentLevel ConsentLevel { get; set; }
        public string SignatureData { get; set; }
    }
}
