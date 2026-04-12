using Oqtane.Models;
using Oqtane.Modules;

namespace OpenEug.TenTrees.Module.Training
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "Training",
            Description = "Permaculture training class management and attendance tracking for tree eligibility",
            Version = "2.2.0",
            ServerManagerType = "",
            ReleaseVersions = "1.0.0,1.0.1,1.0.2,1.0.3,1.0.4,1.0.5,2.1.0,2.2.0",
            Dependencies = "OpenEug.TenTrees.Module.Village.1.0.0,OpenEug.TenTrees.Module.Grower.1.0.0",
            PackageName = "OpenEug.TenTrees.Module.Training"
        };
    }
}
