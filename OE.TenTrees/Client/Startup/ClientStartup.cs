using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Oqtane.Services;
using OE.TenTrees.Services;

namespace OE.TenTrees.Startup
{
    public class ClientStartup : IClientStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            if (!services.Any(s => s.ServiceType == typeof(IMyModuleService)))
            {
                services.AddScoped<IMyModuleService, MyModuleService>();
            }
            
            if (!services.Any(s => s.ServiceType == typeof(IApplicationService)))
            {
                services.AddScoped<IApplicationService, ApplicationService>();
            }
            
            if (!services.Any(s => s.ServiceType == typeof(IMonitoringService)))
            {
                services.AddScoped<IMonitoringService, MonitoringService>();
            }
        }
    }
}
