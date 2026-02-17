using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Oqtane.Infrastructure;
using OpenEug.TenTrees.Module.Training.Repository;
using OpenEug.TenTrees.Module.Training.Services;

namespace OpenEug.TenTrees.Module.Training.Startup
{
    public class TrainingServerStartup : IServerStartup
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
            services.AddTransient<ITrainingService, ServerTrainingService>();
            services.AddTransient<ITrainingRepository, TrainingRepository>();
        }
    }
}
