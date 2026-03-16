using Microsoft.Extensions.DependencyInjection;
using Oqtane.Services;
using OpenEug.TenTrees.Module.Mentor.Services;
using System.Linq;

namespace OpenEug.TenTrees.Module.Mentor.Startup
{
    public class MentorClientStartup : IClientStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            if (!services.Any(s => s.ServiceType == typeof(IMentorService)))
            {
                services.AddScoped<IMentorService, MentorService>();
            }
        }
    }
}
