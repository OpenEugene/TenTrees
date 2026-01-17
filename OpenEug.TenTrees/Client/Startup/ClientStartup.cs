using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Oqtane.Services;
using OpenEug.TenTrees.Module.Enrollment.Services;

namespace OpenEug.TenTrees.Module.Enrollment.Startup
{
    public class ClientStartup : IClientStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            if (!services.Any(s => s.ServiceType == typeof(IEnrollmentService)))
            {
                services.AddScoped<IEnrollmentService, EnrollmentService>();
            }
        }
    }
}
