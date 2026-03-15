using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Oqtane.Services;
using OpenEug.TenTrees.Module.Cohort.Services;

namespace OpenEug.TenTrees.Module.Cohort.Startup
{
    public class CohortClientStartup : IClientStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            if (!services.Any(s => s.ServiceType == typeof(ICohortService)))
            {
                services.AddScoped<ICohortService, CohortService>();
            }
        }
    }
}
