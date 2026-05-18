using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Oqtane.Services;
using OpenEug.TenTrees.Module.Grower.Services;

namespace OpenEug.TenTrees.Module.Grower.Startup
{
    public class OrchardClientStartup : IClientStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            if (!services.Any(s => s.ServiceType == typeof(IOrchardService)))
            {
                services.AddScoped<IOrchardService, OrchardService>();
            }
        }
    }
}
