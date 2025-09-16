using Oqtane.Models;
using Oqtane.Modules;

namespace OE.TenTrees.Application
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "Tree Planting Application",
            Description = "Module for managing tree planting applications including submission, review, approval and rejection",
            Version = "1.0.0",
            ReleaseVersions = "1.0.0",
            Dependencies = "OE.TenTrees.Shared.Oqtane",
            PackageName = "OE.TenTrees"
        };
    }
}