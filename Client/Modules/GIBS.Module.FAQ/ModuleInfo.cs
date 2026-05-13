using Oqtane.Models;
using Oqtane.Modules;

namespace GIBS.Module.FAQ
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "FAQ",
            Description = "FAQ Module for Oqtane",
            Version = "1.0.1",
            ServerManagerType = "GIBS.Module.FAQ.Manager.FAQManager, GIBS.Module.FAQ.Server.Oqtane",
            ReleaseVersions = "1.0.0,1.0.1",
            Dependencies = "GIBS.Module.FAQ.Shared.Oqtane",
            PackageName = "GIBS.Module.FAQ" 
        };
    }
}
