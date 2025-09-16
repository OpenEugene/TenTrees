using Oqtane.Models;
using Oqtane.Modules;

namespace OE.TenTrees.Garden
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "Garden Management",
            Description = "Module for managing garden sites, tree plantings, and garden monitoring following site assessments from approved applications",
            Version = "1.0.0",
            ReleaseVersions = "1.0.0",
            Dependencies = "OE.TenTrees.Shared.Oqtane",
            PackageName = "OE.TenTrees"
        };
    }
}