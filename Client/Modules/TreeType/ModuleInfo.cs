using Oqtane.Models;
using Oqtane.Modules;

namespace OpenEug.TenTrees.Module.TreeType
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "TreeType",
            Description = "Tree Type Management",
            Version = "2.2.0",
            ServerManagerType = "",
            ReleaseVersions = "1.0.0,2.1.0,2.2.0",
            Dependencies = ""
        };
    }
}
