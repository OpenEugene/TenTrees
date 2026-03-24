using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Oqtane.Services;
using OpenEug.TenTrees.Module.Assessment.Services;

namespace OpenEug.TenTrees.Module.Assessment.Startup
{
    public class AssessmentClientStartup : IClientStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            if (!services.Any(s => s.ServiceType == typeof(IAssessmentService)))
            {
                services.AddScoped<IAssessmentService, AssessmentService>();
            }
        }
    }
}
