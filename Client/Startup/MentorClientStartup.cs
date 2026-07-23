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
            // Guard against double-registration when running on Blazor Server, where Oqtane.Server
            // also invokes IClientStartup and IServerStartup already registered this service first.
            // See https://github.com/oqtane/oqtane.framework/discussions/5541
            if (!services.Any(s => s.ServiceType == typeof(IMentorService)))
            {
                services.AddScoped<IMentorService, MentorService>();
            }
        }
    }
}
