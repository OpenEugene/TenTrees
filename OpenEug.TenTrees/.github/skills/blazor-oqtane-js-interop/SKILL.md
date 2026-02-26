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
   In `OnAfterRenderAsync`, dynamically ensure the script is attached to the DOM, then poll until the namespace is available before invoking the function.
   ```csharp
   protected override async Task OnAfterRenderAsync(bool firstRender)
   {
       if (firstRender)
       {
           // Ensure the script is loaded before calling it
           await JSRuntime.InvokeVoidAsync("eval", "if (typeof YourNamespace === 'undefined') { var script = document.createElement('script'); script.src = '/Modules/Your.Module.Name/Module.js'; document.head.appendChild(script); }");

           // Poll until the namespace is available or timeout after 5 seconds
           const int maxWaitMs = 5000;
           const int pollIntervalMs = 50;
           int elapsed = 0;
           while (elapsed < maxWaitMs)
           {
               var isDefined = await JSRuntime.InvokeAsync<bool>("eval", "typeof YourNamespace !== 'undefined'");
               if (isDefined) break;
               await Task.Delay(pollIntervalMs);
               elapsed += pollIntervalMs;
           }

           await JSRuntime.InvokeVoidAsync("YourNamespace.YourModule.YourFeature.init", "elementId");
       }
   }
   ```

## Verification
The Blazor component successfully invokes the JS function without throwing `JSException: Could not find '[Function]' ('[Namespace]' was undefined)`.

## Example
See `Client/Modules/Enrollment/Signature.razor` and `Server/wwwroot/Modules/OpenEug.TenTrees.Module.Enrollment/Module.js` in the 10Trees project.

## Notes
- Avoid using `IJSObjectReference` and `import("./script.js")` in Oqtane modules, as the framework's routing and static file serving handles module assets differently than standard standalone Blazor apps.
- The `eval` workaround is necessary because Oqtane's resource manager loads scripts asynchronously, creating a race condition with `OnAfterRenderAsync`.
- The polling mechanism (checking `typeof YourNamespace !== 'undefined'` in a loop) is more reliable than a fixed `Task.Delay` because it responds as soon as the namespace is ready rather than waiting an arbitrary amount of time. The 5-second timeout prevents infinite loops in error conditions. If the namespace is still undefined after 5 seconds, the subsequent `InvokeVoidAsync` call will throw a `JSException`; handle this case if graceful degradation is needed.
- Oqtane does not currently expose resource loading completion events for module scripts, making this polling pattern the recommended workaround for the race condition.

## References
- Oqtane Framework Module Development Documentation

## Activation History
[Append a line each time this skill is used: YYYY-MM-DD brief context]