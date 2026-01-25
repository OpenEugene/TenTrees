using Oqtane.Models;
using Oqtane.Modules;

namespace OpenEug.TenTrees.Module.Enrollment
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "Enrollment",
            Description = "Beneficiary Enrollment Submission",
            Version = "1.1.0",
            ServerManagerType = "",
            ReleaseVersions = "1.0.0,1.1.0",
            Dependencies = ""
        };
    }
}
