---
name: oqtane-module-development
description: |
  Architecture, conventions, and PWApps-specific constraints for Oqtane module
  development. Covers the client/server split, file layout, base class roles,
  and rules that differ from stock Oqtane defaults. API signatures, method
  names, and code patterns must always be read directly from the local
  framework reference clone at .oqtane-ref/ rather than guessed or assumed.
  Use this any time you create, edit, or review any Oqtane module component,
  service, controller, or repository in the PWApps workspace.
author: Skiller
version: 1.2.0
date: 2025-07-14
---

# Oqtane Module Development

## Overview

Oqtane is a modular Blazor CMS/app framework. PWApps modules follow a strict
client/server split that maps to the Oqtane hosting model:

| Layer | Project | Base Class | Purpose |
|---|---|---|---|
| Client UI | `Client` | `ModuleBase` | Razor components rendered in the browser |
| Client HTTP | `Client/Services` | `ServiceBase` | Typed HTTP clients that call server endpoints |
| Server API | `Server/Controllers` | `ModuleControllerBase` | MVC controllers exposing REST endpoints |
| Server Data | `Server/Repository` | *(interface + class)* | EF/ADO.NET data access, injected via DI |

---

## Local Framework Reference

A shallow clone of the Oqtane framework lives at `.oqtane-ref/` in the
repository root. It is git-ignored and never committed. **All API contracts,
method signatures, and base class members must be read from this reference —
never assumed.**

**One-time setup:**
```powershell
git clone --depth 1 https://github.com/oqtane/oqtane.framework.git .oqtane-ref
```

**Refresh:**
```powershell
git -C .oqtane-ref pull
```

### Key Reference Paths

| What | Path in reference |
|---|---|
| Client component base class | `.oqtane-ref\Oqtane.Client\Modules\ModuleBase.cs` |
| Client HTTP service base class | `.oqtane-ref\Oqtane.Client\Services\ServiceBase.cs` |
| Existing controllers (real examples) | `.oqtane-ref\Oqtane.Server\Controllers\` |
| Existing repositories (real examples) | `.oqtane-ref\Oqtane.Server\Repository\` |
| Shared models | `.oqtane-ref\Oqtane.Shared\Models\` |
| Built-in service interfaces | `.oqtane-ref\Oqtane.Shared\Interfaces\` |
| Client service registrations | `.oqtane-ref\Oqtane.Client\Services\` |

### Research Workflow

Before writing any Oqtane-specific code:

1. Use `get_file` to read the relevant reference file and confirm actual
   method signatures, constructor parameters, and return types.
2. Use `file_search` scoped to `.oqtane-ref` when the exact file path is
   unknown (search by class or interface name).
3. Look at a real existing controller or repository in the reference as a
   structural model before writing a new one.
4. Never infer API shape from memory — the reference is authoritative.

---

## PWApps File Layout Conventions

Each user-facing action is a separate Razor file. `Index.razor` is the
default action Oqtane loads for a module.

```
Client/Modules/<ModuleName>/
  Index.razor          <- default action
  Edit.razor           <- edit action
  Settings.razor       <- module settings (loaded by Oqtane admin panel)
  Services/
    <ModuleName>Service.cs   <- one service class per module, if needed

Server/Controllers/
  <ModuleName>Controller.cs  <- one controller per module

Server/Repository/
  I<ModuleName>Repository.cs
  <ModuleName>Repository.cs
```

---

## Client Components

- Inherit from `ModuleBase` — read `.oqtane-ref\Oqtane.Client\Modules\ModuleBase.cs`
  for all available properties, fields, and methods before using them.
- Use the built-in `logger` field and `AddModuleMessage` / `ClearModuleMessage`
  methods from `ModuleBase` for logging and user feedback.
- Do not inject `ILogger` directly into components.
- When registering a child component as a module control, pass the
  `RenderModeBoundary` parameter to correctly scope the render mode.

### Reading Action Parameters (QueryString)

Oqtane's module host (`RenderModeBoundary`) renders module components
dynamically. It does **not** map URL querystring values to Blazor `[Parameter]`
attributes. A `[Parameter] public string Id { get; set; }` on a module
component will **always be null** at runtime.

**Read action parameters from `PageState.QueryString` instead:**

```csharp
// WRONG — Id is never populated by the Oqtane module host
[Parameter]
public string Id { get; set; }

protected override async Task OnParametersSetAsync()
{
    if (!string.IsNullOrEmpty(Id)) { ... }   // always skipped
}

// CORRECT
protected override async Task OnParametersSetAsync()
{
    if (PageState.QueryString.ContainsKey("id"))
    {
        int id = int.Parse(PageState.QueryString["id"]);
        ...
    }
}
```

`PageState` is a `CascadingParameter` from `ModuleBase` and is always
populated before `OnParametersSetAsync` runs.

For safe access in catch/log blocks use `GetValueOrDefault`:

```csharp
PageState.QueryString.GetValueOrDefault("id")
```

**How `ActionLink` passes parameters:**
`ActionLink` with `Parameters="@($"id={someId}")"` calls
`Utilities.ParseParameters`, which treats `id=123` as a querystring value
(it prepends `?` if no leading `/`, `?`, or `#`). The generated URL is
`/page/!/moduleId/ActionName?id=123`.

**Confirmed in:** `Client/Modules/Enrollment/Edit.razor`

```csharp
_id = Int32.Parse(PageState.QueryString["id"]);
```

**Do not use `NavigateUrl(ModuleState.ModuleId, ...)` for page-level querystring navigation.**
Oqtane's `EditUrl` (called by all `NavigateUrl(moduleId, ...)` overloads) appends
`/!/moduleId` to the path. The `!` is Oqtane's **module admin delimiter** — any URL
with that pattern opens the module inside the admin container modal. Use it only when
intentionally navigating to a named module action (e.g. `Status`, `Edit`).

**`NavigateUrl(string path, string querystring)`** calls `Utilities.NavigateUrl` directly —
no `EditUrl`, no module ID. This is the correct helper for querystring-only navigation:

```csharp
// CORRECT — page URL + querystring, no module admin mode
NavigationManager.NavigateTo(NavigateUrl(PageState.Page.Path, "status=Active&villageId=2"));

// CORRECT — back to index, no querystring
NavigationManager.NavigateTo(NavigateUrl());

// WRONG — never build page URLs manually
NavigationManager.NavigateTo($"/{PageState.Page.Path}?status=Active");  // misses alias handling
```

| Pattern | Produces | Use for |
|---|---|---|
| `NavigateUrl()` | `/grower` | Back to index |
| `NavigateUrl(PageState.Page.Path, qs)` | `/grower?status=Active` | Filter navigation |
| `EditUrl("Status", qs)` | `/grower/!/31/Status?id=5` | Module action links |
attribute (.NET 7+, `[Parameter]` no longer required alongside it in .NET 8+)
that binds a property from the URL querystring. It works when a *routing
component* (one with `@page`) supplies the value through the Blazor router.
Oqtane module components are **not** routed by Blazor — they are rendered
dynamically by `RenderModeBoundary`, which does not participate in the Blazor
routing cascade. `[SupplyParameterFromQuery]` will silently produce `null`
in Oqtane module components for the same reason `[Parameter]` does.

### OnParametersSetAsync vs OnInitializedAsync

Use `OnParametersSetAsync` for data loading in module components. In Oqtane,
`ModuleState` (a cascading parameter) is only guaranteed to be available by
the time `OnParametersSetAsync` runs — not during `OnInitializedAsync`.

When the service returns `null` (e.g. permissions not yet resolved on first
render), default list fields to an empty collection rather than leaving them
null so the template shows the empty-state message instead of a loading
spinner:

```csharp
_growers = await GrowerService.GetAllGrowersAsync(ModuleState.ModuleId) 
           ?? new List<Models.Grower>();
```

---

## Client Services

- Create one service class per module when client logic exceeds simple inline
  `@code`. Place it in the module's `Services/` folder.
- Inherit from `ServiceBase` — read `.oqtane-ref\Oqtane.Client\Services\ServiceBase.cs`
  for `CreateApiUrl`, HTTP helper methods, and constructor signature before use.

---

## Server Controllers

- One controller per module, inheriting from `ModuleControllerBase`.
- Read existing controllers in `.oqtane-ref\Oqtane.Server\Controllers\` as
  structural models for route attributes, authorization policies, and action
  method signatures.

---

## Server Repositories

- One interface + one implementation per module.
- Register both in the module's server-side DI startup file.
- Data access uses raw SQL or Dapper only — no EF migrations, no navigation
  properties, no EF relationships.
- Table definitions go in the SQL projects under `dbo/tables/`.
- Read existing repositories in `.oqtane-ref\Oqtane.Server\Repository\` as
  structural models.

---

## Logging

- In components (`ModuleBase` subclasses): use the `logger` field inherited
  from `ModuleBase`. Read `ModuleBase.cs` for the exact logger type and
  available methods.
- In repositories and plain service classes: inject and use `ILogger<T>`
  directly from the standard .NET logging abstraction.

---

## Context / Trigger Conditions

Apply this skill whenever:

- Creating a new Oqtane module or adding actions to an existing module
- Adding or modifying a client service, server controller, or repository
- Using any `ModuleBase` or `ServiceBase` member not recently verified
- Wiring up DI registration for a new repository or service
- Deciding whether an Oqtane built-in already covers the needed functionality

---

## PWApps Constraints

- No EF migrations or navigation properties.
- No Radzen controls unless explicitly requested — default to Bootstrap and
  Oqtane/Blazor built-ins.
- No Localizer — use plain text strings directly.
- Settings go in `Settings.razor`; Oqtane loads it automatically from the
  module admin panel.

## References

- Framework source (local): `.oqtane-ref\`
- Framework source (live): https://github.com/oqtane/oqtane.framework
- Oqtane docs: https://docs.oqtane.org
