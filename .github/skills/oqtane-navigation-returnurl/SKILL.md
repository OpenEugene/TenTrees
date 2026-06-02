---
name: oqtane-navigation-returnurl
description: |
  How to implement Back/Cancel navigation in Oqtane module Edit pages that
  correctly returns to the Index with filter and sort state preserved.
  Use when: adding a Back link to an Edit.razor, passing filter/sort state
  through to Edit and back, or debugging 404 errors caused by returnurl with
  module-action path segments (*/moduleId) in the URL.
author: Skiller
version: 1.0.0
date: 2026-06-10
---

# Oqtane Navigation & Return URLs

## Problem

Oqtane module-action URLs contain `*/moduleId` path segments (e.g.
`/grower/*/34/Edit`). If you try to navigate back using a stored `returnurl`
that contains this segment — via `NavigationManager.NavigateTo` or an `href`
— Blazor's client-side router sees `*` as a literal wildcard and 404s before
Oqtane's `SiteRouter` ever parses it.

## Context / Trigger Conditions

- Adding a Back/Cancel button to an Edit.razor that should return to Index
- Passing filter or sort state from Index → Edit → back to Index
- Seeing 404 when using `NavigationManager.NavigateTo(returnurl)` where
  `returnurl` was built from `EditUrl(...)` or `PageState.Route.PathAndQuery`
- URLs like `%2Fgrower%2F*%2F34%3Fstatus%3DActive` always 404 when navigated to

## Solution

### Back / Cancel navigation in Edit.razor

**Never store or navigate to a `returnurl` that contains `*/moduleId`.**

Use `NavigateUrl(PageState.Page.Path, PageState.QueryString)` instead:

```razor
<!-- Back link (href) -->
<a href="@NavigateUrl(PageState.Page.Path, PageState.QueryString)"
   class="btn btn-sm btn-outline-secondary">
    <span class="oi oi-arrow-left" aria-hidden="true"></span> Back
</a>
```

```csharp
// Cancel button / NavigateBack() method
private void NavigateBack()
{
    NavigationManager.NavigateTo(NavigateUrl(PageState.Page.Path, PageState.QueryString));
}
```

This produces a clean page URL like `/grower?status=Active&villageId=2` — no
action segment — which routes correctly every time. `PageState.QueryString` is
the dictionary Oqtane already decoded from the current URL, so all params
(filters, sort, etc.) come along for free.

### Passing filter/sort state from Index to Edit

Append the filter/sort query string to the `ActionLink` `Parameters`:

```razor
<ActionLink Action="Edit"
            Parameters="@($"id={grower.GrowerId}&{BuildFilterQueryString()}")"
            ... />
```

Where `BuildFilterQueryString()` emits only non-default params:

```csharp
private string BuildFilterQueryString()
{
    var parts = new List<string>();
    if (!string.IsNullOrEmpty(_statusFilter))  parts.Add($"status={Uri.EscapeDataString(_statusFilter)}");
    if (!string.IsNullOrEmpty(_villageFilter)) parts.Add($"villageId={Uri.EscapeDataString(_villageFilter)}");
    if (_sortBy != "lastname")                 parts.Add($"sortBy={Uri.EscapeDataString(_sortBy)}");
    if (!_sortAsc)                             parts.Add("sortDesc=1");
    return string.Join("&", parts);
}
```

Oqtane appends these to the Edit URL query string, so on the Edit page
`PageState.QueryString` contains all of them. `NavigateUrl(PageState.Page.Path,
PageState.QueryString)` then carries them back to Index automatically.

### Keeping the URL in sync with filter/sort state

Wire every filter and sort control to call `NavigateWithFilters()` via
`@bind:after` or a wrapper method, so the browser URL always reflects current
state. This means `PageState.QueryString` is always current when Edit is opened.

```razor
<select @bind="_sortBy" @bind:after="NavigateWithFilters"> ... </select>
<button @onclick="ToggleSortDirAndNavigate"> ... </button>
```

```csharp
private void NavigateWithFilters()
{
    NavigationManager.NavigateTo(EditUrl("", BuildFilterQueryString()));
}
```

> **Note:** `EditUrl("", qs)` is correct for navigating within the same module
> on the same page. It embeds the module ID: `/grower/*/34?status=Active`.

### Restoring state from the URL on load

```csharp
protected override async Task OnParametersSetAsync()
{
    _statusFilter = PageState.QueryString.GetValueOrDefault("status", "");
    _villageFilter = PageState.QueryString.GetValueOrDefault("villageId", "");
    _sortBy = PageState.QueryString.GetValueOrDefault("sortBy", "lastname");
    _sortAsc = !PageState.QueryString.ContainsKey("sortDesc");
    // ...load data...
}
```

## What Was Tried (and why it failed)

| Approach | Why it fails |
|---|---|
| `NavigationManager.NavigateTo(PageState.QueryString["returnurl"])` | `PageState.QueryString["returnurl"]` is decoded; `/grower/*/34` causes Blazor router 404 |
| `href="@PageState.QueryString["returnurl"]"` | Same — `*` in path is a Blazor wildcard |
| `PageState.Route.PathAndQuery` as returnurl | Captures `/grower/*/34/Edit?id=1` — contains the action segment, still 404s on back-nav |
| `EditUrl("")` as fallback | Goes to module index but loses all filter/sort state |
| `Uri.EscapeDataString` for encoding returnurl | Should use `WebUtility.UrlEncode` to match Oqtane's own convention |

## Verification

1. Apply filters and sort on Index — URL updates in address bar
2. Click Edit on a row — Edit URL contains all filter/sort params in query string
3. Click Back — lands on Index with same filters and sort active
4. No 404 at any step

## Notes

- `NavigateUrl()` (no args) = current page, no query string — loses filter state
- `NavigateUrl(PageState.Page.Path, PageState.QueryString)` = current page + all
  current query params — the correct pattern for Back/Cancel
- Oqtane's own admin modules (Users, Files, etc.) use `NavigateUrl()` with no
  state to preserve because they don't have filter state to restore
- `PageState.QueryString` is already decoded by `SiteRouter` via
  `WebUtility.UrlDecode` — never decode it again before use

## References

- `.oqtane-ref/Oqtane.Client/UI/SiteRouter.razor` — returnurl decode logic (line ~117)
- `.oqtane-ref/Oqtane.Shared/Models/Route.cs` — how `*/moduleId` is parsed from path
- `.oqtane-ref/Oqtane.Client/Modules/ModuleBase.cs` — `NavigateUrl` overloads
- `.oqtane-ref/Oqtane.Client/Themes/Controls/Theme/LoginBase.cs` — `WebUtility.UrlEncode` pattern

## Activation History

- 2026-06-10 Grower Index/Edit — filter+sort state preservation with Back link
