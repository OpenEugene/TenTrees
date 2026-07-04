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
            Version = "2.5.0",
            ServerManagerType = "",
            ReleaseVersions = "1.0.0,1.0.1,1.0.2,1.0.3,1.0.4,2.1.0,2.2.0,2.3.0,2.4.0,2.5.0",
            Dependencies = ""
        };
    }
}
