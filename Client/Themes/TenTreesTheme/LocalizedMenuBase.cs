using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Oqtane.Models;
using Oqtane.Themes.Controls;

namespace OpenEug.TenTrees.Theme.TenTreesTheme
{
    public abstract class LocalizedMenuBase : MenuItemsBase
    {
        [Inject]
        protected IStringLocalizer<LocalizedMenu> Localizer { get; set; }

        // IStringLocalizer returns the key itself when no resource exists,
        // so unmapped pages fall back to their raw database name.
        protected string LocalizeName(Page page)
        {
            return Localizer[page.Name];
        }
    }
}
