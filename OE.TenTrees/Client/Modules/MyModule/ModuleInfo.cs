using Oqtane.Models;
using Oqtane.Modules;

namespace OE.TenTrees.MyModule
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "MyModule",
            Description = "Example module",
            Version = "1.0.0",
            ServerManagerType = "OE.TenTrees.Manager.MyModuleManager, OE.TenTrees.Server.Oqtane",
            ReleaseVersions = "1.0.0",
            Dependencies = "OE.TenTrees.Shared.Oqtane",
            PackageName = "OE.TenTrees" 
        };
    }
}
