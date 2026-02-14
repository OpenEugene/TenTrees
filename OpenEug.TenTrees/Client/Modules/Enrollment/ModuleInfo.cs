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
            Version = "1.2.1",
            ServerManagerType = "",
            ReleaseVersions = "1.0.0,1.1.0,1.1.1,1.2.0,1.2.1",
            Dependencies = ""
        };
    }
}
