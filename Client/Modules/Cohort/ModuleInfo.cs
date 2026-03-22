using Oqtane.Models;
using Oqtane.Modules;

namespace OpenEug.TenTrees.Module.Cohort
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "Cohort",
            Description = "Cohort Management",
            Version = "1.0.2",
            ServerManagerType = "",
            ReleaseVersions = "1.0.0,1.0.1,1.0.2",
            Dependencies = ""
        };
    }
}
