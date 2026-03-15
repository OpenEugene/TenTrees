using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Oqtane.Infrastructure;
using OpenEug.TenTrees.Module.Mentor.Services;

namespace OpenEug.TenTrees.Module.Mentor.Startup
{
    public class MentorServerStartup : IServerStartup
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
            // IMentorRepository and IVillageRepository are auto-registered by Oqtane
            // via the ITransientService marker on their implementation classes.
            services.AddTransient<IMentorService, ServerMentorService>();
        }
    }
}
