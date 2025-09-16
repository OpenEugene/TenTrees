using Oqtane.Models;
using Oqtane.Modules;

namespace OE.TenTrees.Monitoring
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "Garden Monitoring",
            Description = "Module for monitoring gardens",
            Version = "1.0.0",
            ServerManagerType = "OE.TenTrees.Manager.MonitoringManager, OE.TenTrees.Server.Oqtane",
            ReleaseVersions = "1.0.0",
            Dependencies = "OE.TenTrees.Shared.Oqtane",
            PackageName = "OE.TenTrees"
        };
    }
}