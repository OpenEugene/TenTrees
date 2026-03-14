using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Oqtane.Infrastructure;
using OpenEug.TenTrees.Module.Assessment.Repository;
using OpenEug.TenTrees.Module.Assessment.Services;

namespace OpenEug.TenTrees.Module.Assessment.Startup
{
    public class AssessmentServerStartup : IServerStartup
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // No specific configuration needed
        }

        public void ConfigureMvc(IMvcBuilder mvcBuilder)
        {
            // No specific configuration needed
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IAssessmentService, ServerAssessmentService>();
            services.AddTransient<IAssessmentRepository, AssessmentRepository>();
        }
    }
}
