---
name: blazor-oqtane-js-interop
description: |
  How to correctly implement and load JavaScript interop in Oqtane Blazor modules,
  avoiding "undefined" errors when calling JS functions from Blazor components.
author: Skiller
version: 1.0.0
date: 2026-02-25
---

# Oqtane Blazor JavaScript Interop

## Problem
When adding custom JavaScript to an Oqtane module, calling `JSRuntime.InvokeVoidAsync` from a Blazor component often results in `Could not find 'Namespace.Function' ('Namespace' was undefined)` errors, even if the script is registered in `ModuleInfo.cs`.

## Context / Trigger Conditions
- Building a custom Oqtane module (Blazor WebAssembly/Server).
- Adding a custom `.js` file to `Server/wwwroot/Modules/[Module.Name]/`.
- Attempting to call JS functions from `OnAfterRenderAsync` in a `.razor` component.

## Solution
Oqtane's dynamic module loading means scripts registered in `ModuleInfo.cs` might not be fully parsed and available in the DOM by the time `OnAfterRenderAsync` fires on the first render.

1. **Use the standard Oqtane JS namespace pattern:**
   Do not use ES6 `export` modules. Use the IIFE (Immediately Invoked Function Expression) pattern attached to a global namespace.
   ```javascript
   /* Server/wwwroot/Modules/Your.Module.Name/Module.js */
   var YourNamespace = YourNamespace || {};
   YourNamespace.YourModule = {
       YourFeature: (function () {
           function init(elementId) { /* ... */ }
           return { init: init };
       })()
   };
   ```

2. **Register the script in `ModuleInfo.cs`:**
   ```csharp
   public override List<Resource> Resources => new List<Resource>
   {
       new Resource { ResourceType = ResourceType.Script, Url = "~/Modules/Your.Module.Name/Module.js" }
   };
   ```

3. **Avoid Redundant Resource Registrations:**
   Do not register the same script in both `ModuleInfo.cs` and the component's `Resources` property. Rely on `ModuleInfo.cs` for module-wide scripts. Redundant registrations are unnecessary and can complicate debugging.

4. **Ensure the script is loaded before invoking (The Fix):**
   In `OnAfterRenderAsync`, retry the invocation until it succeeds. This avoids the race condition without `eval` or manually injecting script tags.
   ```csharp
   protected override async Task OnAfterRenderAsync(bool firstRender)
   {
       if (firstRender)
       {
           // Retry until the namespace is defined (Oqtane loads scripts asynchronously).
           // 10 attempts × 100 ms = up to 1 second total wait time.
           const int MaxAttempts = 10;
           const int DelayMs = 100;
           for (int i = 0; i < MaxAttempts; i++)
           {
               try
               {
                   await JSRuntime.InvokeVoidAsync("YourNamespace.YourModule.YourFeature.init", "elementId");
                   break; // success
               }
               catch (JSException) when (i < MaxAttempts - 1)
               {
                   // Script not yet available; wait before retrying.
                   // On the last attempt the exception is not caught here and propagates.
                   await Task.Delay(DelayMs);
               }
           }
       }
   }
   ```

   **Preferred alternative — use Oqtane's resource manager with the render cycle:**
   Register the script in `ModuleInfo.cs` (see step 2) and rely entirely on Oqtane's built-in resource pipeline. Guard the invocation with a flag and use `StateHasChanged` to re-enter the render cycle until the script is ready, instead of polling with delays. Include a retry counter to prevent an infinite loop if the script never loads.
   ```csharp
   private bool _scriptsReady = false;
   private int _scriptRetryCount = 0;
   private const int MaxScriptRetries = 10;

   protected override async Task OnAfterRenderAsync(bool firstRender)
   {
       if (!_scriptsReady && _scriptRetryCount < MaxScriptRetries)
       {
           try
           {
               await JSRuntime.InvokeVoidAsync("YourNamespace.YourModule.YourFeature.init", "elementId");
               _scriptsReady = true;
           }
           catch (JSException)
           {
               _scriptRetryCount++;
               if (_scriptRetryCount < MaxScriptRetries)
               {
                   StateHasChanged(); // trigger another render pass to retry
               }
               else
               {
                   Console.Error.WriteLine("YourNamespace script failed to load after maximum retries.");
               }
           }
       }
   }
   ```
   This piggybacks on Blazor's render cycle rather than using arbitrary delays, and neither injects script tags at runtime nor uses `eval`.

## Verification
The Blazor component successfully invokes the JS function without throwing `JSException: Could not find '[Function]' ('[Namespace]' was undefined)`.

## Example
See `Client/Modules/Enrollment/Signature.razor` and `Server/wwwroot/Modules/OpenEug.TenTrees.Module.Enrollment/Module.js` in the 10Trees project.

## Notes
- Avoid using `IJSObjectReference` and `import("./script.js")` in Oqtane modules, as the framework's routing and static file serving handles module assets differently than standard standalone Blazor apps.
- Oqtane's resource manager loads scripts asynchronously, which can create a race condition with `OnAfterRenderAsync`. The preferred mitigation is to rely on Blazor's render cycle (re-rendering via `StateHasChanged` until the namespace is defined) rather than arbitrary `Task.Delay` calls or runtime script injection.
- Avoid using `eval` to inject `<script>` tags at runtime. Dynamically adding script tags bypasses Oqtane's resource pipeline and can introduce security and ordering issues. Always register scripts through `ModuleInfo.cs` and let the framework handle loading.

## References
- Oqtane Framework Module Development Documentation