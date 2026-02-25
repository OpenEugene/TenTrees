using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Oqtane.Services;
using OpenEug.TenTrees.Module.Training.Services;

namespace OpenEug.TenTrees.Module.Training.Startup
{
    public class TrainingClientStartup : IClientStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            if (!services.Any(s => s.ServiceType == typeof(ITrainingService)))
            {
                services.AddScoped<ITrainingService, TrainingService>();
            }
        }
    }
}
