using Oqtane.Models;
using Oqtane.Modules;
using System.Collections.Generic;

namespace OpenEug.TenTrees.Module.Grower
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "Grower",
            Description = "Grower management for 10 Trees program - track active status and program exits",
            Version = "1.0.0",
            ServerManagerType = "",
            ReleaseVersions = "1.0.0",
            Dependencies = "OpenEug.TenTrees.Module.Village.1.0.0",
            PackageName = "OpenEug.TenTrees.Module.Grower"
        };
    }
}
