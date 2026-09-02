using Oqtane.Models;
using Oqtane.Modules;

namespace OpenEug.TenTrees.Module.Assessment
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "Assessment",
            Description = "Garden Assessment and Tree Monitoring",
            Version = "2.9.0",
            ServerManagerType = "",
            ReleaseVersions = "1.0.0,1.0.1,1.0.2,1.0.3,1.0.4,1.0.5,2.1.0,2.2.0,2.3.0,2.4.0,2.5.0,2.6.0,2.7.0,2.8.0,2.9.0",
            Dependencies = "OpenEug.TenTrees.Module.Grower.1.0.0",
            PackageName = "OpenEug.TenTrees.Module.Assessment"
        };
    }
}
