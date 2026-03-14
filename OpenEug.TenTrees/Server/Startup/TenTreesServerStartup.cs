using Microsoft.AspNetCore.Builder; 
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Oqtane.Infrastructure;

namespace OpenEug.TenTrees.Startup
{
    public class TenTreesServerStartup : IServerStartup
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
            // Register shared TenTreesContext DbContextFactory once for all modules
            services.AddDbContextFactory<OpenEug.TenTrees.Repository.TenTreesContext>(opt => { }, ServiceLifetime.Transient);
        }
    }
}
