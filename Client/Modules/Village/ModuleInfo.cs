using Oqtane.Models;
using Oqtane.Modules;

namespace OpenEug.TenTrees.Module.Village
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "Village",
            Description = "Village Management",
            Version = "1.1.4",
            ServerManagerType = "",
            ReleaseVersions = "1.0.0,1.1.0,1.1.1,1.1.2,1.1.3,1.1.4",
            Dependencies = ""
        };
    }
}
