using Oqtane.Models;
using Oqtane.Modules;

namespace OpenEug.TenTrees.Module.Home
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "Home",
            Description = "Home page dashboard with navigation tiles for each program module",
            Version = "1.1.0",
            ServerManagerType = "",
            ReleaseVersions = "1.0.0,1.1.0",
            Dependencies = "",
            PackageName = "OpenEug.TenTrees.Module.Home"
        };
    }
}
