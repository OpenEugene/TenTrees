using Microsoft.AspNetCore.Builder; 
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Oqtane.Infrastructure;
using OpenEug.TenTrees.Module.Enrollment.Repository;
using OpenEug.TenTrees.Module.Enrollment.Services;
using OpenEug.TenTrees.Module.Grower.Repository;
using OpenEug.TenTrees.Module.Grower.Services;

namespace OpenEug.TenTrees.Module.Enrollment.Startup
{
    public class EnrollmentServerStartup : IServerStartup
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
            services.AddTransient<IEnrollmentService, ServerEnrollmentService>();
            services.AddTransient<IGrowerService, ServerGrowerService>();
            services.AddTransient<IGrowerRepository, GrowerRepository>();
        }
    }
}
