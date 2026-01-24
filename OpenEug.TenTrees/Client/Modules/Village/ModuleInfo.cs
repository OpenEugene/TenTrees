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
            Version = "1.0.0",
            ServerManagerType = "OpenEug.TenTrees.Module.Village.Manager.VillageManager, OpenEug.TenTrees.Server.Oqtane",
            ReleaseVersions = "1.0.0",
            Dependencies = ""
        };
    }
}
