using Oqtane.Models;
using Oqtane.Modules;

namespace OpenEug.TenTrees.Module.Enrollment
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "Enrollment",
            Description = "Grower Enrollment Submission",
            Version = "1.3.0",
            ServerManagerType = "",
            ReleaseVersions = "1.0.0,1.1.0,1.1.1,1.2.0,1.2.1,1.3.0",
            Dependencies = ""
        };
    }
}
