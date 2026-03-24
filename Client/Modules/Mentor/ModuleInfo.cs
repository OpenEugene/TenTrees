using Oqtane.Models;
using Oqtane.Modules;

namespace OpenEug.TenTrees.Module.Mentor
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "Mentor",
            Description = "Mentor user management — create, edit, activate/deactivate mentors and assign growers",
            Version = "1.0.4",
            ServerManagerType = "",
            ReleaseVersions = "1.0.0,1.0.1,1.0.2,1.0.3,1.0.4",
            Dependencies = "OpenEug.TenTrees.Module.Village.1.0.0",
            PackageName = "OpenEug.TenTrees.Module.Mentor"
        };
    }
}
