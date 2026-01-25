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
            Version = "1.1.0",
            ServerManagerType = "",
            ReleaseVersions = "1.0.0,1.1.0",
            Dependencies = ""
        };
    }
}
