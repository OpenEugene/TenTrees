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
    }

    public class EnrollmentService : ServiceBase, IEnrollmentService
    {
        public EnrollmentService(HttpClient http, SiteState siteState) : base(http, siteState) { }

        private string Apiurl => CreateApiUrl("Enrollment");

        public async Task<List<Models.Enrollment>> GetEnrollmentsAsync(int ModuleId)
        {
            List<Models.Enrollment> Enrollments = await GetJsonAsync<List<Models.Enrollment>>(CreateAuthorizationPolicyUrl($"{Apiurl}?moduleid={ModuleId}", EntityNames.Module, ModuleId), Enumerable.Empty<Models.Enrollment>().ToList());
            return Enrollments.OrderBy(item => item.Name).ToList();
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
    }
}
