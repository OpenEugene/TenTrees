using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Oqtane.Infrastructure;
using OpenEug.TenTrees.Module.TreeType.Repository;
using OpenEug.TenTrees.Module.TreeType.Services;

namespace OpenEug.TenTrees.Module.TreeType.Startup
{
    public class TreeTypeServerStartup : IServerStartup
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
            services.AddTransient<ITreeTypeRepository, TreeTypeRepository>();
            services.AddTransient<ITreeTypeService, ServerTreeTypeService>();
        }
    }
}
