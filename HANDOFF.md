# Hand-off: Localized Navigation Menu (feature/localized-menu)

Session hand-off for continuing work on another machine. Written 2026-07-15.
Delete this file before merging the branch.

## Goal

Localize the top navigation menu (page names come from the `Page` table in the
database, so Oqtane's built-in menu shows raw English names regardless of
culture). Approach: a copy of Oqtane's menu control chain that looks up each
`Page.Name` as a resx key via `IStringLocalizer`, falling back to the raw name
when no key exists. This mirrors the framework's own precedent
(`Oqtane.Client\Modules\Admin\Dashboard\Index.razor` localizes admin page names
against `SharedResources` using the name as key).

Stretch goal (deferred): upstream to oqtane/oqtane.framework — inject
`IStringLocalizer<SharedResources>` into `MenuItemsBase` and render
`SharedLocalizer[childPage.Name]`. Zero behavior change when no resource
exists. Process: issue first, fork, branch from `dev`, PR to `dev` per
CONTRIBUTING.md.

## What's on this branch (commit 709fa17)

All new files in `Client\Themes\TenTreesTheme\`, namespace
`OpenEug.TenTrees.Theme.TenTreesTheme`:

| File | Role |
|---|---|
| `LocalizedMenuBase.cs` | Inherits Oqtane's `MenuItemsBase`; injects `IStringLocalizer<LocalizedMenu>`; `LocalizeName(page)` returns `Localizer[page.Name]` (missing key ⇒ raw name) |
| `LocalizedMenu.razor` | Orientation dispatcher (`Horizontal`/`Vertical`), compile-time switch |
| `LocalizedMenuHorizontal.razor` / `LocalizedMenuVertical.razor` | Verbatim copies of stock `MenuHorizontal/Vertical` (inherit Oqtane `MenuBase`), pointing at the Localized items components |
| `LocalizedMenuItemsHorizontal.razor` / `LocalizedMenuItemsVertical.razor` | Verbatim copies of stock items components; only change: `@childPage.Name` → `@LocalizeName(childPage)` |

Resources (resolve via full-namespace folder because the Client project's root
namespace doesn't prefix the theme namespace):

- `Client\Resources\OpenEug.TenTrees.Theme.TenTreesTheme\LocalizedMenu.resx`
- `Client\Resources\OpenEug.TenTrees.Theme.TenTreesTheme\LocalizedMenu.ts-ZA.resx`

Keys: `Home, Enrollment, Classes, Training, Grower, Assessment, Admin,
Village, Cohort, Mentor, TreeType`. ts-ZA values reuse the reviewed Home-tile
strings (Nkandziyiso, Tiklasi, Ku Leha, Mulimi, Xikambelo, Vulawuri, Rixaka,
Muleriseri, Muxaka wa Murhi). **"Kaya" (Home) is a new, unreviewed translation
— flag for translation review.**

Wire-up: `Client\Themes\TenTreesTheme\Theme.razor` line 7 —
`<LocalizedMenu Orientation="Horizontal" />` (was `<Menu Orientation="Horizontal" />`).

## Test status

Done:
- `dotnet build` clean (0 errors).
- Menu structure/fallback verified — but only under the **default Oqtane
  theme** (the dev site wasn't set to TenTreesTheme at the time), so the
  localized control itself has NOT yet rendered successfully end-to-end.

Not done (the actual test plan):
1. Site set to TenTreesTheme, English: menu identical to before (Admin
   dropdown, active-page `(current)` marker, mobile toggler).
2. Switch to Xitsonga via the theme's LanguagePicker (full reload): navbar
   shows Kaya / Nkandziyiso / Tiklasi / Mulimi / Xikambelo / Vulawuri etc.
3. Fallback: any page without a matching key shows its raw DB name.
4. Keyboard nav + aria attributes survived the copy.

## Known issues / gotchas

1. **Key = exact `Page.Name`, case-sensitive.** The Surf7 dev DB has a page
   literally named `training` (lowercase) — it falls back to raw "training"
   because the key is `Training`. Fix by renaming the page or adding a
   lowercase key. Check the target DB's names first:
   `SELECT Name, Path FROM [Page] WHERE IsNavigation = 1`.
2. **White screen on Surf7 when TenTreesTheme is selected — NOT caused by this
   branch.** Server prerender dies with `NullReferenceException` at
   `OqtaneLocalizationExtensions.Create` ← `LocalizableComponent.OnParametersSet`
   ← `Oqtane.Modules.Controls.Label.OnParametersSet` — i.e. some component
   passes a `ResourceType` string that `Type.GetType()` can't resolve. No
   TenTrees menu frames in the stack; none of the new files use `Label`.
   Suspected stale type registration in that machine's ancient `Oqtane-TenTrees`
   LocalDB. Response is HTTP 200 with a zero-length body; the real stack trace
   is in the Oqtane `Log` table (`SELECT TOP 5 * FROM [Log] ORDER BY LogId DESC`).
   If the theme renders fine on this PC's DB, that diagnosis is confirmed.
3. **Connection strings are per-machine.** `appsettings.json` keeps named
   variants (`DefaultConnectionSurf7`, `DefaultConnectionFold`); the active
   `DefaultConnection` swap is intentionally NOT committed. Set it locally.
4. This dev DB had no `Classes` or `Admin` navigation page (production does,
   per the Discord screenshot) — those resx keys are dormant until tested
   against a site that has them.

## Reference notes (saves re-research)

- Framework reference clone: `.oqtane-ref\` (git-ignored). Menu chain:
  `.oqtane-ref\Oqtane.Client\Themes\Controls\Theme\Menu*.razor|cs`.
- `MenuBase.MenuPages` already applies `IsNavigation` + view-permission
  filtering; `GetUrl`/`GetTarget` handle external URLs and non-clickable pages.
- Culture switching (theme `LanguagePicker.razor`) persists `User.CultureCode`
  for logged-in users / sets the `.AspNetCore.Culture` cookie for anonymous,
  then full-reloads — so the menu re-renders in the new culture with no cache
  concerns.
- `Page` model has no localization support at all (verified `Page.cs` incl.
  `Clone()`); menu text is always `Page.Name`, browser tab is `Page.Title`.
