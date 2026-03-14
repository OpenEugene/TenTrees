using Microsoft.Extensions.DependencyInjection;
using Oqtane.Services;
using OpenEug.TenTrees.Module.Assessment.Services;

namespace OpenEug.TenTrees.Module.Assessment.Startup
{
    public class AssessmentClientStartup : IClientStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IAssessmentService, AssessmentService>();
        }
    }
}
