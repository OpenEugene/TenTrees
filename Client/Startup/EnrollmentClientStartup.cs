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
