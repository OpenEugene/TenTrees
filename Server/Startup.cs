using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Oqtane.Infrastructure;

namespace OE.TenTrees
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOqtane();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseOqtane(env);
        }
    }
}