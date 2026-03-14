using System.Collections.Generic;
using Oqtane.Models;
using Oqtane.Modules;
using Oqtane.Shared;

namespace OpenEug.TenTrees.Module.Enrollment
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "Enrollment",
            Description = "Grower Enrollment Submission",
            Version = "2.0.0",
            ServerManagerType = "",
            ReleaseVersions = "1.0.0,1.1.0,1.1.1,1.2.0,1.2.1,1.3.0,2.0.0",
            Dependencies = "",
            Resources = new List<Resource>
            {
                new Resource { ResourceType = ResourceType.Script, Url = "~/Modules/OpenEug.TenTrees.Module.Enrollment/Module.js" }
            }
        };
    }
}
