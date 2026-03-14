using System.Collections.Generic;
using Oqtane.Models;
using Oqtane.Modules;
using Oqtane.Shared;

namespace OpenEug.TenTrees.Module.Assessment
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "Assessment",
            Description = "Garden Assessment and Tree Monitoring",
            Version = "1.0.0",
            ServerManagerType = "",
            ReleaseVersions = "1.0.0",
            Dependencies = "OpenEug.TenTrees.Module.Grower.1.0.0",
            PackageName = "OpenEug.TenTrees.Module.Assessment"
        };
    }
}
