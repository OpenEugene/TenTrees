using System.Collections.Generic;
using Oqtane.Models;
using Oqtane.Themes;
using Oqtane.Shared;

namespace OpenEug.TenTrees.Theme.TenTreesTheme
{
    public class ThemeInfo : ITheme
    {
        public Oqtane.Models.Theme Theme => new Oqtane.Models.Theme
        {
            Name = "TenTreesTheme",
            Version = "1.0.0",
            PackageName = "OpenEug.TenTrees.Theme.TenTreesTheme",
            ThemeSettingsType = "OpenEug.TenTrees.Theme.TenTreesTheme.ThemeSettings, OpenEug.TenTrees.Theme.TenTreesTheme.Client.Oqtane",
            ContainerSettingsType = "OpenEug.TenTrees.Theme.TenTreesTheme.ContainerSettings, OpenEug.TenTrees.Theme.TenTreesTheme.Client.Oqtane",
            Resources = new List<Resource>()
            {
		// obtained from https://cdnjs.com/libraries
                new Stylesheet(Constants.BootstrapStylesheetUrl, Constants.BootstrapStylesheetIntegrity, "anonymous"),
                new Stylesheet("~/Theme.css"),
                new Script(Constants.BootstrapScriptUrl, Constants.BootstrapScriptIntegrity, "anonymous")
            }
        };

    }
}
