using System.Collections.Generic;
using Oqtane.Models;
using Oqtane.Themes;
using Oqtane.Shared;

namespace OE.TenTrees.MyTheme
{
    public class ThemeInfo : ITheme
    {
        public Oqtane.Models.Theme Theme => new Oqtane.Models.Theme
        {
            Name = "MyTheme",
            Version = "1.0.0",
            PackageName = "OE.TenTrees",
            ThemeSettingsType = "OE.TenTrees.MyTheme.ThemeSettings, OE.TenTrees.Client.Oqtane",
            ContainerSettingsType = "OE.TenTrees.MyTheme.ContainerSettings, OE.TenTrees.Client.Oqtane",
            Resources = new List<Resource>()
            {
                new Stylesheet(Constants.BootstrapStylesheetUrl, Constants.BootstrapStylesheetIntegrity, "anonymous"),
                new Stylesheet("~/Theme.css"),
                new Script(Constants.BootstrapScriptUrl, Constants.BootstrapScriptIntegrity, "anonymous")
            }
        };
    }
}
