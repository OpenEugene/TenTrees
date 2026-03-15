using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Oqtane.Infrastructure;
using OpenEug.TenTrees.Module.Cohort.Repository;
using OpenEug.TenTrees.Module.Cohort.Services;
using OpenEug.TenTrees.Module.Village.Repository;

namespace OpenEug.TenTrees.Module.Cohort.Startup
{
    public class CohortServerStartup : IServerStartup
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) { }

        public void ConfigureMvc(IMvcBuilder mvcBuilder) { }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ICohortService, ServerCohortService>();
            services.AddTransient<ICohortRepository, CohortRepository>();
            services.AddTransient<IVillageRepository, VillageRepository>();
        }
    }
}
