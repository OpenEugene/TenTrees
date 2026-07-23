using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Oqtane.Services;
using OpenEug.TenTrees.Module.Enrollment.Services;
using OpenEug.TenTrees.Module.Grower.Services;

namespace OpenEug.TenTrees.Module.Enrollment.Startup
{
    public class EnrollmentClientStartup : IClientStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Guard against double-registration when running on Blazor Server, where Oqtane.Server
            // also invokes IClientStartup and IServerStartup already registered these services first.
            // See https://github.com/oqtane/oqtane.framework/discussions/5541
            if (!services.Any(s => s.ServiceType == typeof(IEnrollmentService)))
            {
                services.AddScoped<IEnrollmentService, EnrollmentService>();
            }
            if (!services.Any(s => s.ServiceType == typeof(IEnrollmentStateService)))
            {
                services.AddScoped<IEnrollmentStateService, EnrollmentStateService>();
            }
            if (!services.Any(s => s.ServiceType == typeof(IGrowerService)))
            {
                services.AddScoped<IGrowerService, GrowerService>();
            }
        }
    }
}
