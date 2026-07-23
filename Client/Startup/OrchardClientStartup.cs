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
            // Guard against double-registration when running on Blazor Server, where Oqtane.Server
            // also invokes IClientStartup and IServerStartup already registered this service first.
            // See https://github.com/oqtane/oqtane.framework/discussions/5541
            if (!services.Any(s => s.ServiceType == typeof(IOrchardService)))
            {
                services.AddScoped<IOrchardService, OrchardService>();
            }
        }
    }
}
