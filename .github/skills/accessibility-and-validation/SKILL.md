---
name: accessibility-and-validation
description: |
  WCAG 2.1 / Section 508 compliant accessibility patterns for Blazor pages and forms
  in PWApps Oqtane modules. Covers: per-field validation error state, named-field
  messages, Bootstrap is-invalid styling, aria-invalid/aria-required attributes,
  PwDatePicker integration, scroll-to-top feedback, clickable div vs anchor/button
  keyboard accessibility, icon aria-hidden, icon-only button aria-label, loading
  role=status, table actions column visually-hidden headers, and filter label/id
  pairing. Use this any time you write, review, or audit any Blazor page or form
  that inherits ModuleBase — including index pages, edit forms, and new-record forms.
author: Skiller
version: 1.1.0
date: 2025-07-10
---

# Blazor Accessibility & Form Validation Pattern

## Problem

Two classes of issues appear repeatedly across PWApps Blazor modules:

**Validation (edit/new forms):**
- Generic "Please fill in all required fields." messages don't identify *which* fields
- No visual (`is-invalid`) or programmatic (`aria-invalid`) marking on invalid fields
- Page stays scrolled to the bottom while the message appears at the top
- Screen readers cannot identify which fields need correction

**General accessibility (all pages including indexes):**
- Clickable `<div @onclick>` cards/rows are invisible to keyboard users (SC 2.1.1)
- Decorative `oi oi-*` icons announced unnecessarily by screen readers (SC 1.1.1)
- Icon-only buttons have no accessible name (SC 1.1.1)
- Filter `<label>` elements not associated to their inputs via `for`/`id` (SC 1.3.1)
- Empty table `<th>` cells for actions columns (SC 1.3.1)
- Loading `<p>` not announced by screen readers (SC 4.1.3)

## Context / Trigger Conditions

- Writing a `Save()` / `Submit()` method on any Blazor form in a PWApps module
- Writing or reviewing any index page with clickable cards or table rows
- Adding navigation cards, action buttons, or icon elements to any page
- Reviewing existing forms or pages during accessibility improvements
- Adding new required fields to an existing form
- Spec: `Specs/System/Accessibility.feature` SC 1.1.1, SC 1.3.1, SC 2.1.1, SC 3.3.1, SC 3.3.2, SC 4.1.3

## Solution

### 1. State Fields

```csharp
private HashSet<string> _invalidFields = new();
private bool _validated;          // becomes true after first Save attempt
```

### 2. Save() Validation Block

Replace the single `bool isValid` flag with a per-field approach that also
builds a human-readable list for the message:

```csharp
private async Task Save()
{
    ClearModuleMessage();
    _invalidFields.Clear();
    _validated = true;

    var missing = new List<string>();

    if (string.IsNullOrWhiteSpace(_model.SurveyNumber)) { _invalidFields.Add("SurveyNumber"); missing.Add("Survey Number"); }
    if (_model.Pages == null || _model.Pages <= 0)       { _invalidFields.Add("Pages");        missing.Add("Pages"); }
    if (_model.DateIn == null)                           { _invalidFields.Add("DateIn");        missing.Add("Date In"); }
    if (string.IsNullOrWhiteSpace(_model.DocSize))       { _invalidFields.Add("DocSize");       missing.Add("Document Size"); }
    // ... repeat for each required field ...

    if (_model.References.Any(r => r.CodeId == 0 || string.IsNullOrWhiteSpace(r.Description)))
        missing.Add("References (incomplete)");

    if (missing.Count > 0)
    {
        AddModuleMessage($"Required: {string.Join(", ", missing)}", MessageType.Warning);
        await ScrollToPageTop();
        return;
    }

    _isSaving = true;
    // ... save logic ...
}
```

### 3. Native Input Markup

Apply `is-invalid`, `aria-invalid`, and `aria-required` to every required `<input>` and `<select>`:

```razor
<input id="pages" type="number"
       class="form-control @(_invalidFields.Contains("Pages") ? "is-invalid" : "")"
       @bind="_model.Pages"
       aria-required="true"
       aria-invalid="@(_invalidFields.Contains("Pages") ? "true" : null)" />
```

For `<select>`:
```razor
<select id="docSize"
        class="form-select @(_invalidFields.Contains("DocSize") ? "is-invalid" : "")"
        @bind="_model.DocSize"
        aria-required="true"
        aria-invalid="@(_invalidFields.Contains("DocSize") ? "true" : null)">
```

### 4. PwDatePicker Integration

`PwDatePicker` has a built-in `IsInvalid` parameter that turns the input and
calendar icon red. Use it directly — no extra wrapper needed:

```razor
<PwDatePicker @bind-Value="_model.DateIn"
              IsInvalid="@_invalidFields.Contains("DateIn")" />

<%-- When using ValueChanged instead of @bind-Value: --%>
<PwDatePicker Value="_model.DocDate" ValueChanged="OnDocDateChanged"
              IsInvalid="@_invalidFields.Contains("DocDate")" />
```

### 5. Single-Field Variant (SurveyEdit pattern)

When a form has only one or two required fields, a dedicated bool is acceptable
and consistent with how `_showSurveyNumberError` is already used:

```csharp
private bool _showSurveyNumberError;

// In Save():
_showSurveyNumberError = false;
if (!_model.IsEarlySurvey && string.IsNullOrWhiteSpace(_model.SurveyNumber))
{
    _showSurveyNumberError = true;
    AddModuleMessage("Survey Number is required.", MessageType.Warning);
    await ScrollToPageTop();
    return;
}
```

```razor
<input id="surveyNumber" type="text"
       class="form-control @(_showSurveyNumberError ? "is-invalid" : "")"
       @bind="_model.SurveyNumber"
       aria-required="true"
       aria-invalid="@(_showSurveyNumberError ? "true" : null)" />
```

### 6. Required Loading Indicator

Per SC 4.1.3 (Status Messages), the loading paragraph needs `role="status"`:

```razor
@if (!_isLoaded)
{
    <p role="status"><em>Loading...</em></p>
}
```

### 7. Saving Button State

Per SC 4.1.3, spinner needs `role="status"` and `aria-hidden="true"` on the icon;
the visible text is the announced label:

```razor
<button type="button" class="btn btn-primary" @onclick="Save" disabled="@_isSaving">
    @if (_isSaving)
    {
        <span class="spinner-border spinner-border-sm me-1" role="status" aria-hidden="true"></span>
        <span>Saving...</span>
    }
    else
    {
        <span class="oi oi-check me-1" aria-hidden="true"></span>
        <span>Save</span>
    }
</button>
```

---

## Index & List Page Patterns

### 8. Clickable Cards and Rows — Use `<a>` not `<div @onclick>`

**The problem:** `<div @onclick="...">` is invisible to keyboard navigation. Tab skips
it entirely. This is a hard WCAG SC 2.1.1 failure.

**The fix:** For navigation actions use `<a href="@EditUrl(...)">`. For non-navigation
actions use `<button>`. Never use a plain `<div>` as an interactive element.

```razor
<!-- ❌ WRONG — keyboard inaccessible -->
<div class="card hover-card" @onclick="@(() => NavigateToAction("SurveyNew"))">
    ...
</div>

<!-- ✅ CORRECT — natively keyboard accessible, Tab + Enter activates -->
<a href="@EditUrl("SurveyNew")"
   class="card h-100 hover-card text-decoration-none text-reset">
    <div class="card-body">
        <h5>New Survey</h5>
        <p>Record a new survey document into the index.</p>
        <!-- Hide the decorative inner "button" from AT to avoid duplicate text -->
        <div class="btn btn-primary w-100 mt-2" aria-hidden="true">
            <span class="oi oi-plus me-1"></span> New Survey
        </div>
    </div>
</a>
```

Key points:
- `text-decoration-none text-reset` preserves card visual appearance
- `aria-hidden="true"` on the inner `<div class="btn">` prevents the card title
  being announced twice (once from the heading, once from the button text)
- `EditUrl("ActionName")` is available directly from `ModuleBase` — no
  `NavigationManager` inject needed, and the `NavigateToAction` helper method
  can be deleted entirely
- Screen reader announces: *"New Survey — Record a new survey document into the index., link"*

### 9. Decorative Icons — Always `aria-hidden="true"`

Every `oi oi-*` icon that has adjacent visible text must be hidden from AT.
This includes icons in headings, buttons, nav links, and card icon-circles.

```razor
<!-- Heading icon -->
<h4><span class="oi oi-pencil me-2" aria-hidden="true"></span>Workflows</h4>

<!-- Button with text -->
<button type="button" class="btn btn-primary">
    <span class="oi oi-plus me-1" aria-hidden="true"></span> Add New Survey
</button>

<!-- Nav link -->
<NavLink href="@NavigateUrl()">
    <span class="oi oi-arrow-left" aria-hidden="true"></span> Back to Home
</NavLink>
```

### 10. Icon-Only Buttons — Require `aria-label`

When a button contains *only* an icon with no adjacent text (e.g., edit/delete row
buttons in tables), the button needs an `aria-label` and the icon needs `aria-hidden`:

```razor
<!-- ❌ WRONG — screen reader announces "pencil" or nothing -->
<button class="btn btn-sm btn-outline-primary" title="Edit survey">
    <span class="oi oi-pencil"></span>
</button>

<!-- ✅ CORRECT — announces "Edit survey 1042" -->
<button class="btn btn-sm btn-outline-primary"
        aria-label="Edit survey @context.SurveyId">
    <span class="oi oi-pencil" aria-hidden="true"></span>
</button>
```

Note: `title` is NOT sufficient — it only shows on mouse hover, not announced
reliably by all screen readers.

### 11. Table Actions Column Header

Empty `<th>` elements in the actions column confuse AT. Use `visually-hidden`:

```razor
<!-- ❌ WRONG -->
<th style="width: 45px;"></th>

<!-- ✅ CORRECT -->
<th style="width: 45px;"><span class="visually-hidden">Actions</span></th>
```

### 12. Filter Controls — Label Association

Every `<label>` must have a `for` attribute matching the `id` of its control.
Labels without `for` are not programmatically associated and fail SC 1.3.1.

```razor
<!-- ❌ WRONG -->
<label class="form-label">Survey Number</label>
<input type="text" class="form-control" @bind="_search" />

<!-- ✅ CORRECT -->
<label class="form-label" for="filterSurveyNum">Survey Number</label>
<input id="filterSurveyNum" type="text" class="form-control" @bind="_search" />
```

## Verification

Pattern is in production use in:
- `Client/Modules/Surveyor/SurveyNew.razor` — HashSet validation, named message
- `Client/Modules/Surveyor/PlatNew.razor` — HashSet validation, named message
- `Client/Modules/Surveyor/SurveyEdit.razor` — single-bool variant
- `Client/Modules/Surveyor/Index.razor` — `<a>` cards, aria-hidden icons
- `Client/Modules/Surveyor/SurveyIndex.razor` — filter label/id, icon-only buttons, actions th
- `Client/Modules/Surveyor/SurveyRecordIndex.razor` — icon-only buttons, actions th

Build passes with `run_build` after all changes.

## Notes

- `aria-invalid` should be **omitted entirely** (null) when the field is valid or
  unvalidated — setting it to `"false"` is redundant but not harmful. Razor
  `@(_condition ? "true" : null)` correctly omits the attribute when null.
- `_invalidFields` is cleared at the start of every `Save()` call so stale error
  state doesn't persist after the user fixes issues.
- The `HashSet<string>` key names are arbitrary — pick stable names matching the
  field/property name (e.g., `"DateIn"`, `"DocSize"`) for readability.
- References rows get `is-invalid` via inline checks when `_validated` is true:
  ```razor
  class="form-select @(_validated && _model.References[index].CodeId == 0 ? "is-invalid" : "")"
  ```
- When converting `<div @onclick>` cards to `<a href>`, the `NavigateToAction`
  helper method and `@inject NavigationManager` can be deleted if no longer used.
- `title` on buttons is **not** a substitute for `aria-label` — it only appears
  on mouse hover and is not reliably announced by screen readers.

## References

- `Specs/System/Accessibility.feature` — SC 1.1.1, SC 1.3.1, SC 2.1.1, SC 3.3.1, SC 3.3.2, SC 4.1.3
- WCAG 2.1: https://www.w3.org/TR/WCAG21/
- Bootstrap `is-invalid`: https://getbootstrap.com/docs/5.0/forms/validation/
- Bootstrap `visually-hidden`: https://getbootstrap.com/docs/5.0/helpers/visually-hidden/
