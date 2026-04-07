using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Oqtane.Services;
using OpenEug.TenTrees.Module.TreeType.Services;

namespace OpenEug.TenTrees.Module.TreeType.Startup
{
    public class TreeTypeClientStartup : IClientStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            if (!services.Any(s => s.ServiceType == typeof(ITreeTypeService)))
            {
                services.AddScoped<ITreeTypeService, TreeTypeService>();
            }
        }
    }
}
