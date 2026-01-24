using Microsoft.AspNetCore.Builder; 
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Oqtane.Infrastructure;
using OpenEug.TenTrees.Module.Village.Repository;
using OpenEug.TenTrees.Module.Village.Services;

namespace OpenEug.TenTrees.Module.Village.Startup
{
    public class VillageServerStartup : IServerStartup
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // not implemented
        }

        public void ConfigureMvc(IMvcBuilder mvcBuilder)
        {
            // not implemented
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IVillageService, ServerVillageService>();
            services.AddDbContextFactory<OpenEug.TenTrees.Repository.TenTreesContext>(opt => { }, ServiceLifetime.Transient);
        }
    }
}
