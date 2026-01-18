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
            Version = "1.0.0",
            ServerManagerType = "OpenEug.TenTrees.Module.Enrollment.Manager.EnrollmentManager, OpenEug.TenTrees.Server.Oqtane",
            ReleaseVersions = "1.0.0",
            Dependencies = ""
        };
    }
}
