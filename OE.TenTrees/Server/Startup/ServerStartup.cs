using Microsoft.AspNetCore.Builder; 
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Oqtane.Infrastructure;
using OE.TenTrees.Repository;
using OE.TenTrees.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace OE.TenTrees.Startup
{
    public class ServerStartup : IServerStartup
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Ensure monitoring migrations are applied
            using (var scope = app.ApplicationServices.CreateScope())
            {
                try
                {
                    var contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<Context>>();
                    using var context = contextFactory.CreateDbContext();
                    
                    // Apply any pending migrations
                    context.Database.Migrate();
                }
                catch (System.Exception ex)
                {
                    // Log the exception but don't fail startup
                    var logger = scope.ServiceProvider.GetService<Microsoft.Extensions.Logging.ILogger<ServerStartup>>();
                    logger?.LogError(ex, "Error applying monitoring migrations during startup");
                }
            }
        }

        public void ConfigureMvc(IMvcBuilder mvcBuilder)
        {
            // not implemented
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IMyModuleService, ServerMyModuleService>();
            services.AddTransient<IApplicationService, ServerApplicationService>();
            services.AddTransient<IApplicationRepository, ApplicationRepository>();
            services.AddTransient<IMonitoringService, ServerMonitoringService>();
            services.AddTransient<IMonitoringRepository, MonitoringRepository>();
            services.AddDbContextFactory<Context>(opt => { }, ServiceLifetime.Transient);
            
            // Ensure HttpContextAccessor is registered for audit fields
            services.AddHttpContextAccessor();
        }
    }
}
