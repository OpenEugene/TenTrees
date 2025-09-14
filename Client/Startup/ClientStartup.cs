using Microsoft.Extensions.DependencyInjection;
using Oqtane.Services;
using OE.TenTrees.Services;

namespace OE.TenTrees.Startup
{
    public class ClientStartup : IClientStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ITreeService, TreeService>();
            services.AddScoped<IPlantingEventService, PlantingEventService>();
        }
    }
}