using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Oqtane.Services;
using OpenEug.TenTrees.Module.Village.Services;

namespace OpenEug.TenTrees.Module.Village.Startup
{
    public class VillageClientStartup : IClientStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            if (!services.Any(s => s.ServiceType == typeof(IVillageService)))
            {
                services.AddScoped<IVillageService, VillageService>();
            }
        }
    }
}
