using Oqtane.Models;
using Oqtane.Modules;

namespace OE.TenTrees.Modules.TenTrees
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "TenTrees",
            Description = "A module for managing tree planting, monitoring and conservation efforts",
            Version = "1.0.0",
            ServerManagerType = "OE.TenTrees.Manager.TenTreesManager, OE.TenTrees.Server.Oqtane",
            ReleaseVersions = "1.0.0",
            Dependencies = "OE.TenTrees.Shared.Oqtane",
            PackageName = "OE.TenTrees"
        };
    }
}