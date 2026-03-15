# Photo Release Consent — Implementation Plan

**Feature:** Photo Release Consent
**Module:** Enrollment (Option A — new action within existing module)
**Pattern:** Self-contained action component, follows `Grower/Status.razor`
**Date drafted:** 2026-03-15
**Source feature spec:** `Specs/Features/PhotoReleaseConsent.feature`

---

## Overview

Add a Photo Release Consent form as a new action (`PhotoConsent`) in the Enrollment
module. The action is accessible from the Enrollment index list — but only for
Approved enrollments. It captures the grower's consent level (Full / Limited / None)
plus a separate signature, stores both on the existing `Enrollment` record, and
surfaces the consent level as a badge column in the index table.

No new database table is required. Three new columns are added to the existing
`Enrollment` table.

---

## 1. Data Model Changes

### 1a. New enum — `PhotoConsentLevel`

**File:** `Shared/Models/Enrollment.cs`

Add after the existing `EnrollmentStatus` enum:

```csharp
public enum PhotoConsentLevel
{
    NotCaptured = 0,
    Full = 1,       // "You may use my photo with my name identified"
    Limited = 2,    // "You may use my picture in group photos without my name"
    None = 3        // "You may not use my photo at all"
}
```

### 1b. New fields on `Enrollment`

**File:** `Shared/Models/Enrollment.cs`

Add below the existing `// Status` section:

```csharp
// Photo Release Consent (captured separately after enrollment is Approved)
public PhotoConsentLevel PhotoConsentLevel { get; set; }
public bool PhotoConsentSignatureCollected { get; set; }
public DateTime? PhotoConsentSignatureDate { get; set; }
public string PhotoConsentSignatureData { get; set; }
```

These parallel the existing `SignatureCollected` / `SignatureDate` / `SignatureData`
triplet. EF Core stores the enum as `INT` (default behaviour, matching `Status`).

---

## 2. Database Migration

### 2a. New migration script

**New file:** `Sql/Scripts/Migration_AddPhotoConsent.sql`

```sql
/*
 * Migration Script: Add Photo Release Consent columns to Enrollment table
 * Run once against the live database after deploying the updated application.
 * Safe to re-run — each block checks for column existence first.
 */

IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('dbo.Enrollment') AND name = 'PhotoConsentLevel'
)
BEGIN
    ALTER TABLE [dbo].[Enrollment]
        ADD [PhotoConsentLevel] INT NOT NULL DEFAULT 0;
END

IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('dbo.Enrollment') AND name = 'PhotoConsentSignatureCollected'
)
BEGIN
    ALTER TABLE [dbo].[Enrollment]
        ADD [PhotoConsentSignatureCollected] BIT NOT NULL DEFAULT 0;
END

IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('dbo.Enrollment') AND name = 'PhotoConsentSignatureDate'
)
BEGIN
    ALTER TABLE [dbo].[Enrollment]
        ADD [PhotoConsentSignatureDate] DATETIME2(7) NULL;
END

IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('dbo.Enrollment') AND name = 'PhotoConsentSignatureData'
)
BEGIN
    ALTER TABLE [dbo].[Enrollment]
        ADD [PhotoConsentSignatureData] NVARCHAR(MAX) NULL;
END
GO
```

### 2b. Update canonical table DDL

**File:** `Sql/dbo/Tables/Enrollment.sql`

After `[SignatureData]`, before `[Status]`, add:

```sql
    [PhotoConsentLevel]                INT            NOT NULL DEFAULT 0,
    [PhotoConsentSignatureCollected]   BIT            NOT NULL DEFAULT 0,
    [PhotoConsentSignatureDate]        DATETIME2 (7)  NULL,
    [PhotoConsentSignatureData]        NVARCHAR (MAX) NULL,
```

---

## 3. Server Changes

### 3a. New request DTO

**File:** `Server/Controllers/EnrollmentController.cs`

Add below the existing `SignatureRequest` class:

```csharp
public class PhotoConsentRequest
{
    public int ModuleId { get; set; }
    public Models.PhotoConsentLevel ConsentLevel { get; set; }
    public string SignatureData { get; set; }
}
```

### 3b. New controller endpoint

**File:** `Server/Controllers/EnrollmentController.cs`

Add after the `CaptureSignature` endpoint:

```csharp
// POST api/<controller>/5/photoconsent
[HttpPost("{id}/photoconsent")]
[Authorize(Policy = PolicyNames.EditModule)]
public async Task<bool> CapturePhotoConsent(int id, [FromBody] PhotoConsentRequest request)
{
    var enrollment = await _EnrollmentService.GetEnrollmentAsync(id, request.ModuleId);
    if (enrollment != null && IsAuthorizedEntityId(EntityNames.Module, enrollment.ModuleId))
    {
        return await _EnrollmentService.CapturePhotoConsentAsync(
            id, request.ModuleId, request.ConsentLevel, request.SignatureData);
    }
    else
    {
        _logger.Log(LogLevel.Error, this, LogFunction.Security,
            "Unauthorized Photo Consent Capture Attempt {EnrollmentId}", id);
        HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
        return false;
    }
}
```

### 3c. Server service method

**File:** `Server/Services/EnrollmentService.cs`

Add after `CaptureSignatureAsync`:

```csharp
public Task<bool> CapturePhotoConsentAsync(int enrollmentId, int moduleId,
    Models.PhotoConsentLevel consentLevel, string signatureData)
{
    try
    {
        var enrollment = _enrollmentRepository.GetEnrollment(enrollmentId);
        if (enrollment != null)
        {
            enrollment.PhotoConsentLevel = consentLevel;
            enrollment.PhotoConsentSignatureData = signatureData;
            enrollment.PhotoConsentSignatureCollected = true;
            enrollment.PhotoConsentSignatureDate = DateTime.UtcNow;
            _enrollmentRepository.UpdateEnrollment(enrollment);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }
    catch
    {
        return Task.FromResult(false);
    }
}
```

---

## 4. Client Service Changes

**File:** `Client/Services/EnrollmentService.cs`

### 4a. Add to `IEnrollmentService` interface

```csharp
Task<bool> CapturePhotoConsentAsync(int enrollmentId, int moduleId,
    Models.PhotoConsentLevel consentLevel, string signatureData);
```

### 4b. Add request DTO

```csharp
public class PhotoConsentRequest
{
    public int ModuleId { get; set; }
    public Models.PhotoConsentLevel ConsentLevel { get; set; }
    public string SignatureData { get; set; }
}
```

### 4c. Add client implementation

```csharp
public async Task<bool> CapturePhotoConsentAsync(int enrollmentId, int moduleId,
    Models.PhotoConsentLevel consentLevel, string signatureData)
{
    return await PostJsonAsync<PhotoConsentRequest, bool>(
        CreateAuthorizationPolicyUrl($"{Apiurl}/{enrollmentId}/photoconsent",
            EntityNames.Module, moduleId),
        new PhotoConsentRequest
        {
            ModuleId = moduleId,
            ConsentLevel = consentLevel,
            SignatureData = signatureData
        });
}
```

---

## 5. EnrollmentListViewModel

**File:** `Shared/Models/EnrollmentViewModel.cs`

Add one property:

```csharp
public PhotoConsentLevel PhotoConsentLevel { get; set; }
```

**File:** `Server/Repository/EnrollmentRepository.cs`

In `GetEnrollmentListViewModels`, add to the `select new EnrollmentListViewModel` block:

```csharp
PhotoConsentLevel = e.PhotoConsentLevel,
```

---

## 6. New File: `Client/Modules/Enrollment/PhotoConsent.razor`

Self-contained action component. Follows `Grower/Status.razor` (Id parameter, loads
own data, navigates back to index). Reuses the existing signature canvas JS from
`Signature.razor`.

```razor
@using OpenEug.TenTrees.Module.Enrollment.Services
@using OpenEug.TenTrees.Module.Village.Services
@using OpenEug.TenTrees.Models

@namespace OpenEug.TenTrees.Module.Enrollment
@inherits ModuleBase
@inject IEnrollmentService EnrollmentService
@inject IVillageService VillageService
@inject NavigationManager NavigationManager
@inject IJSRuntime JSRuntime
@inject IStringLocalizer<PhotoConsent> Localizer

@if (_isLoading)
{
    <p><em>@Localizer["Message.Loading"]</em></p>
}
else if (_enrollment == null)
{
    <div class="alert alert-danger">
        <i class="bi bi-exclamation-triangle"></i> @Localizer["Message.EnrollmentNotFound"]
    </div>
    <button class="btn btn-secondary" @onclick="NavigateToIndex">
        <i class="bi bi-arrow-left"></i> @Localizer["Action.BackToList"]
    </button>
}
else
{
    <div class="container">
        <h3>@Localizer["Page.Title"]</h3>

        <!-- Context header -->
        <div class="card mb-3">
            <div class="card-body">
                <div class="row">
                    <div class="col-md-6">
                        <strong>@Localizer["Label.Grower"]:</strong>
                        <span class="ms-2 fs-5">@_enrollment.GrowerName</span>
                    </div>
                    <div class="col-md-6">
                        <strong>@Localizer["Label.Village"]:</strong>
                        <span class="ms-2">@_villageName</span>
                    </div>
                </div>
            </div>
        </div>

        @if (_enrollment.PhotoConsentSignatureCollected)
        {
            <div class="alert alert-info">
                <i class="bi bi-info-circle"></i>
                @Localizer["Message.AlreadyCaptured"]
                <strong>@GetConsentLevelText(_enrollment.PhotoConsentLevel)</strong>.
                @Localizer["Message.CanUpdate"]
            </div>
        }

        <!-- Consent options -->
        <div class="card mb-3">
            <div class="card-header bg-primary text-white">
                <h5 class="mb-0">@Localizer["Section.ConsentOptions"]</h5>
            </div>
            <div class="card-body">
                <div class="alert alert-info mb-3">
                    <i class="bi bi-info-circle"></i> @Localizer["Consent.Info"]
                </div>

                <div class="form-check p-3 mb-2 border rounded @(_selectedConsent == PhotoConsentLevel.Full ? "border-success bg-success-subtle" : "")">
                    <input class="form-check-input" type="radio" name="consentLevel" id="consentFull"
                           checked="@(_selectedConsent == PhotoConsentLevel.Full)"
                           @onchange="@(() => _selectedConsent = PhotoConsentLevel.Full)"
                           style="width: 20px; height: 20px;" />
                    <label class="form-check-label ms-2" for="consentFull">
                        <strong>@Localizer["Consent.Full.Label"]</strong><br />
                        <small class="text-muted">@Localizer["Consent.Full.Description"]</small>
                    </label>
                </div>

                <div class="form-check p-3 mb-2 border rounded @(_selectedConsent == PhotoConsentLevel.Limited ? "border-warning bg-warning-subtle" : "")">
                    <input class="form-check-input" type="radio" name="consentLevel" id="consentLimited"
                           checked="@(_selectedConsent == PhotoConsentLevel.Limited)"
                           @onchange="@(() => _selectedConsent = PhotoConsentLevel.Limited)"
                           style="width: 20px; height: 20px;" />
                    <label class="form-check-label ms-2" for="consentLimited">
                        <strong>@Localizer["Consent.Limited.Label"]</strong><br />
                        <small class="text-muted">@Localizer["Consent.Limited.Description"]</small>
                    </label>
                </div>

                <div class="form-check p-3 mb-2 border rounded @(_selectedConsent == PhotoConsentLevel.None ? "border-danger bg-danger-subtle" : "")">
                    <input class="form-check-input" type="radio" name="consentLevel" id="consentNone"
                           checked="@(_selectedConsent == PhotoConsentLevel.None)"
                           @onchange="@(() => _selectedConsent = PhotoConsentLevel.None)"
                           style="width: 20px; height: 20px;" />
                    <label class="form-check-label ms-2" for="consentNone">
                        <strong>@Localizer["Consent.None.Label"]</strong><br />
                        <small class="text-muted">@Localizer["Consent.None.Description"]</small>
                    </label>
                </div>
            </div>
        </div>

        <!-- Signature canvas -->
        <div class="card mb-3">
            <div class="card-header bg-primary text-white">
                <h5 class="mb-0">@Localizer["Section.Signature"]</h5>
            </div>
            <div class="card-body">
                <div class="alert alert-info">
                    <i class="bi bi-info-circle"></i> @Localizer["Signature.Info"]
                </div>

                <div class="mb-3">
                    <div class="d-flex justify-content-between align-items-center mb-1">
                        <label class="form-label fw-bold mb-0">@Localizer["Label.Signature"]</label>
                        <button type="button" class="btn btn-sm btn-outline-secondary" @onclick="ClearSignature">
                            <i class="bi bi-x-circle"></i> @Localizer["Signature.Clear"]
                        </button>
                    </div>
                    <div class="border rounded bg-white" style="touch-action: none;">
                        <canvas id="photoConsentSignatureCanvas"
                                style="width: 100%; height: 180px; display: block; cursor: crosshair;"></canvas>
                    </div>
                    <small class="form-text text-muted">@Localizer["Signature.Help"]</small>
                </div>

                <div class="form-check p-3 border rounded">
                    <input class="form-check-input" type="checkbox" id="confirmConsent"
                           @bind="@_confirmed" style="width: 24px; height: 24px;" />
                    <label class="form-check-label ms-2 fw-bold" for="confirmConsent">
                        @Localizer["Signature.Confirm"]
                    </label>
                </div>
            </div>
        </div>

        @if (_isSaving)
        {
            <div class="alert alert-info text-center">
                <div class="spinner-border spinner-border-sm" role="status">
                    <span class="visually-hidden">@Localizer["Message.Saving"]</span>
                </div>
                <span class="ms-2">@Localizer["Message.Saving"]</span>
            </div>
        }

        <div class="d-flex gap-2 mb-3">
            <button type="button" class="btn btn-success"
                    @onclick="Save"
                    disabled="@(!_confirmed || _selectedConsent == PhotoConsentLevel.NotCaptured || _isSaving)">
                <i class="bi bi-check-circle"></i> @Localizer["Action.Save"]
            </button>
            <button type="button" class="btn btn-secondary" @onclick="NavigateToIndex" disabled="@_isSaving">
                <i class="bi bi-arrow-left"></i> @Localizer["Action.BackToList"]
            </button>
        </div>
    </div>
}

@code {
    private Models.Enrollment _enrollment;
    private string _villageName;
    private bool _isLoading = true;
    private bool _confirmed;
    private bool _isSaving;
    private PhotoConsentLevel _selectedConsent = PhotoConsentLevel.NotCaptured;
    private const string CanvasId = "photoConsentSignatureCanvas";

    [Parameter]
    public string Id { get; set; }

    public override SecurityAccessLevel SecurityAccessLevel => SecurityAccessLevel.View;
    public override string Actions => "PhotoConsent";
    public override string Title => "Photo Release Consent";

    protected override async Task OnParametersSetAsync()
    {
        try
        {
            _isLoading = true;
            if (!string.IsNullOrEmpty(Id) && int.TryParse(Id, out int enrollmentId))
            {
                _enrollment = await EnrollmentService.GetEnrollmentAsync(enrollmentId, ModuleState.ModuleId);

                if (_enrollment == null)
                {
                    await logger.LogWarning("Enrollment {Id} not found or unauthorized", Id);
                    return;
                }

                // Guard: only Approved enrollments may have photo consent captured
                if (_enrollment.Status != EnrollmentStatus.Approved)
                {
                    await logger.LogWarning("PhotoConsent accessed for non-Approved enrollment {Id}", Id);
                    NavigationManager.NavigateTo(NavigateUrl());
                    return;
                }

                // Pre-populate if consent was already captured (re-capture scenario)
                if (_enrollment.PhotoConsentSignatureCollected)
                {
                    _selectedConsent = _enrollment.PhotoConsentLevel;
                }

                var village = await VillageService.GetVillageAsync(_enrollment.VillageId);
                _villageName = village?.VillageName ?? _enrollment.VillageId.ToString();
            }
        }
        catch (Exception ex)
        {
            await logger.LogError(ex, "Error Loading Enrollment for PhotoConsent {Id} {Error}", Id, ex.Message);
            AddModuleMessage(Localizer["Message.LoadError"], MessageType.Error);
        }
        finally
        {
            _isLoading = false;
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && _enrollment != null)
        {
            await JSRuntime.InvokeVoidAsync(
                "eval",
                "if (typeof OpenEug === 'undefined') { " +
                "var script = document.createElement('script'); " +
                "script.src = '/Modules/OpenEug.TenTrees.Module.Enrollment/Module.js'; " +
                "document.head.appendChild(script); }");
            await Task.Delay(100);
            await JSRuntime.InvokeVoidAsync("OpenEug.TenTrees.Enrollment.SignaturePad.init", CanvasId);
        }
    }

    private async Task ClearSignature()
    {
        await JSRuntime.InvokeVoidAsync("OpenEug.TenTrees.Enrollment.SignaturePad.clear", CanvasId);
    }

    private async Task Save()
    {
        try
        {
            if (!_confirmed)
            {
                AddModuleMessage(Localizer["Message.ValidationError"], MessageType.Warning);
                return;
            }

            if (_selectedConsent == PhotoConsentLevel.NotCaptured)
            {
                AddModuleMessage(Localizer["Message.ConsentRequired"], MessageType.Warning);
                return;
            }

            var blank = await JSRuntime.InvokeAsync<bool>(
                "OpenEug.TenTrees.Enrollment.SignaturePad.isBlank", CanvasId);
            if (blank)
            {
                AddModuleMessage(Localizer["Message.SignatureRequired"], MessageType.Warning);
                return;
            }

            var signatureData = await JSRuntime.InvokeAsync<string>(
                "OpenEug.TenTrees.Enrollment.SignaturePad.getData", CanvasId);

            _isSaving = true;
            StateHasChanged();

            var success = await EnrollmentService.CapturePhotoConsentAsync(
                _enrollment.EnrollmentId,
                ModuleState.ModuleId,
                _selectedConsent,
                signatureData);

            if (success)
            {
                await logger.LogInformation(
                    "Photo Consent Captured for Enrollment {EnrollmentId} Level: {Level}",
                    _enrollment.EnrollmentId, _selectedConsent);
                AddModuleMessage(Localizer["Message.Success"], MessageType.Success);
                NavigationManager.NavigateTo(NavigateUrl());
            }
            else
            {
                _isSaving = false;
                AddModuleMessage(Localizer["Message.Error"], MessageType.Error);
            }
        }
        catch (Exception ex)
        {
            _isSaving = false;
            await logger.LogError(ex, "Error Saving Photo Consent {Error}", ex.Message);
            AddModuleMessage(Localizer["Message.Error"], MessageType.Error);
        }
    }

    private void NavigateToIndex()
    {
        NavigationManager.NavigateTo(NavigateUrl());
    }

    private string GetConsentLevelText(PhotoConsentLevel level)
    {
        return level switch
        {
            PhotoConsentLevel.Full    => Localizer["Consent.Full.Label"],
            PhotoConsentLevel.Limited => Localizer["Consent.Limited.Label"],
            PhotoConsentLevel.None    => Localizer["Consent.None.Label"],
            _                         => Localizer["ConsentLevel.NotCaptured"]
        };
    }
}
```

---

## 7. Changes to `Index.razor`

**File:** `Client/Modules/Enrollment/Index.razor`

### 7a. Table header — add two columns after `Column.GrowerStatus`

```razor
<th>@Localizer["Column.PhotoConsent"]</th>
<th style="width: 1px;">&nbsp;</th>
```

### 7b. Table body — add badge and conditional action link after Grower Status cell

```razor
<td>
    <span class="badge @GetPhotoConsentBadgeClass(enrollment.PhotoConsentLevel)">
        @GetPhotoConsentText(enrollment.PhotoConsentLevel)
    </span>
</td>
<td>
    @if (enrollment.EnrollmentStatus == EnrollmentStatus.Approved)
    {
        <ActionLink Action="PhotoConsent"
                    Parameters="@($"id={enrollment.EnrollmentId}")"
                    ResourceKey="PhotoConsent" />
    }
</td>
```

### 7c. Helper methods — add to `@code` block

```csharp
private string GetPhotoConsentBadgeClass(PhotoConsentLevel level)
{
    return level switch
    {
        PhotoConsentLevel.Full    => "bg-success",
        PhotoConsentLevel.Limited => "bg-warning text-dark",
        PhotoConsentLevel.None    => "bg-danger",
        _                         => "bg-secondary"   // NotCaptured
    };
}

private string GetPhotoConsentText(PhotoConsentLevel level)
{
    return level switch
    {
        PhotoConsentLevel.Full    => Localizer["ConsentLevel.Full"],
        PhotoConsentLevel.Limited => Localizer["ConsentLevel.Limited"],
        PhotoConsentLevel.None    => Localizer["ConsentLevel.None"],
        _                         => Localizer["ConsentLevel.NotCaptured"]
    };
}
```

---

## 8. Resource Files

### 8a. New file: `Client/Resources/OpenEug.TenTrees.Module.Enrollment/PhotoConsent.resx`

```xml
<data name="Page.Title" xml:space="preserve"><value>Photo Release Consent</value></data>
<data name="Section.ConsentOptions" xml:space="preserve"><value>Consent Options</value></data>
<data name="Section.Signature" xml:space="preserve"><value>Signature</value></data>
<data name="Label.Grower" xml:space="preserve"><value>Grower</value></data>
<data name="Label.Village" xml:space="preserve"><value>Village</value></data>
<data name="Label.Signature" xml:space="preserve"><value>Signature: *</value></data>
<data name="Consent.Info" xml:space="preserve"><value>Please select the grower's consent level for photo use and provide their signature below.</value></data>
<data name="Consent.Full.Label" xml:space="preserve"><value>Full consent</value></data>
<data name="Consent.Full.Description" xml:space="preserve"><value>You may use my photo with my name identified</value></data>
<data name="Consent.Limited.Label" xml:space="preserve"><value>Limited consent</value></data>
<data name="Consent.Limited.Description" xml:space="preserve"><value>You may use my picture in group photos without my name</value></data>
<data name="Consent.None.Label" xml:space="preserve"><value>No consent</value></data>
<data name="Consent.None.Description" xml:space="preserve"><value>You may not use my photo at all</value></data>
<data name="Signature.Info" xml:space="preserve"><value>Please provide a signature to confirm the selected consent level</value></data>
<data name="Signature.Help" xml:space="preserve"><value>Draw your signature using your finger or mouse</value></data>
<data name="Signature.Clear" xml:space="preserve"><value>Clear</value></data>
<data name="Signature.Confirm" xml:space="preserve"><value>I confirm this consent level has been explained to and agreed by the grower</value></data>
<data name="Action.Save" xml:space="preserve"><value>Save Consent</value></data>
<data name="Action.BackToList" xml:space="preserve"><value>Back to List</value></data>
<data name="Message.Loading" xml:space="preserve"><value>Loading enrollment...</value></data>
<data name="Message.LoadError" xml:space="preserve"><value>Error loading enrollment</value></data>
<data name="Message.EnrollmentNotFound" xml:space="preserve"><value>Enrollment not found or you do not have permission to view it.</value></data>
<data name="Message.AlreadyCaptured" xml:space="preserve"><value>Consent has already been recorded as: </value></data>
<data name="Message.CanUpdate" xml:space="preserve"><value>You can update the consent level and signature below.</value></data>
<data name="Message.Saving" xml:space="preserve"><value>Saving consent...</value></data>
<data name="Message.ValidationError" xml:space="preserve"><value>Please confirm the consent level before saving</value></data>
<data name="Message.ConsentRequired" xml:space="preserve"><value>Please select a consent option</value></data>
<data name="Message.SignatureRequired" xml:space="preserve"><value>Signature is required — please draw the grower's signature above</value></data>
<data name="Message.Success" xml:space="preserve"><value>Photo consent saved successfully</value></data>
<data name="Message.Error" xml:space="preserve"><value>Error saving photo consent. Please try again.</value></data>
<data name="ConsentLevel.NotCaptured" xml:space="preserve"><value>Not Captured</value></data>
<data name="ConsentLevel.Full" xml:space="preserve"><value>Full</value></data>
<data name="ConsentLevel.Limited" xml:space="preserve"><value>Limited</value></data>
<data name="ConsentLevel.None" xml:space="preserve"><value>None</value></data>
```

### 8b. New file: `Client/Resources/OpenEug.TenTrees.Module.Enrollment/PhotoConsent.ts-ZA.resx`

Xitsonga translations — entries marked `[XS]` need verification by the bilingual team.

```xml
<data name="Page.Title" xml:space="preserve"><value>Nvumelano wa Swifaniso</value></data>
<data name="Section.ConsentOptions" xml:space="preserve"><value>[XS] Consent Options</value></data>
<data name="Section.Signature" xml:space="preserve"><value>Kusayina</value></data>
<data name="Label.Grower" xml:space="preserve"><value>Mulimi</value></data>
<data name="Label.Village" xml:space="preserve"><value>Rixaka</value></data>
<data name="Label.Signature" xml:space="preserve"><value>Kusayina: *</value></data>
<data name="Consent.Info" xml:space="preserve"><value>[XS] Please select the grower's consent level for photo use and provide their signature below.</value></data>
<data name="Consent.Full.Label" xml:space="preserve"><value>[XS] Full consent</value></data>
<data name="Consent.Full.Description" xml:space="preserve"><value>[XS] You may use my photo with my name identified</value></data>
<data name="Consent.Limited.Label" xml:space="preserve"><value>[XS] Limited consent</value></data>
<data name="Consent.Limited.Description" xml:space="preserve"><value>[XS] You may use my picture in group photos without my name</value></data>
<data name="Consent.None.Label" xml:space="preserve"><value>[XS] No consent</value></data>
<data name="Consent.None.Description" xml:space="preserve"><value>[XS] You may not use my photo at all</value></data>
<data name="Signature.Info" xml:space="preserve"><value>Kombela u nyika kusayina ku tiyisisa nhlayo leyi hlawulekeke ya nvumelano</value></data>
<data name="Signature.Help" xml:space="preserve"><value>Dirowa kusayina ka wena hi xivomba xa wena kumbe mouse</value></data>
<data name="Signature.Clear" xml:space="preserve"><value>Susa</value></data>
<data name="Signature.Confirm" xml:space="preserve"><value>[XS] I confirm this consent level has been explained to and agreed by the grower</value></data>
<data name="Action.Save" xml:space="preserve"><value>[XS] Save Consent</value></data>
<data name="Action.BackToList" xml:space="preserve"><value>[XS] Back to List</value></data>
<data name="Message.Loading" xml:space="preserve"><value>[XS] Loading enrollment...</value></data>
<data name="Message.LoadError" xml:space="preserve"><value>Xiphiqo xi Humelele eka ku Loda Nkandziyiso</value></data>
<data name="Message.EnrollmentNotFound" xml:space="preserve"><value>[XS] Enrollment not found or you do not have permission to view it.</value></data>
<data name="Message.AlreadyCaptured" xml:space="preserve"><value>[XS] Consent has already been recorded as: </value></data>
<data name="Message.CanUpdate" xml:space="preserve"><value>[XS] You can update the consent level and signature below.</value></data>
<data name="Message.Saving" xml:space="preserve"><value>[XS] Saving consent...</value></data>
<data name="Message.ValidationError" xml:space="preserve"><value>[XS] Please confirm the consent level before saving</value></data>
<data name="Message.ConsentRequired" xml:space="preserve"><value>[XS] Please select a consent option</value></data>
<data name="Message.SignatureRequired" xml:space="preserve"><value>Kusayina swi laveka — kombela u dirowa kusayina ka wena laha henhla</value></data>
<data name="Message.Success" xml:space="preserve"><value>[XS] Photo consent saved successfully</value></data>
<data name="Message.Error" xml:space="preserve"><value>[XS] Error saving photo consent. Please try again.</value></data>
<data name="ConsentLevel.NotCaptured" xml:space="preserve"><value>[XS] Not Captured</value></data>
<data name="ConsentLevel.Full" xml:space="preserve"><value>[XS] Full</value></data>
<data name="ConsentLevel.Limited" xml:space="preserve"><value>[XS] Limited</value></data>
<data name="ConsentLevel.None" xml:space="preserve"><value>[XS] None</value></data>
```

### 8c. Keys to add to `Index.resx`

```xml
<data name="Column.PhotoConsent" xml:space="preserve"><value>Photo Consent</value></data>
<data name="PhotoConsent" xml:space="preserve"><value>Photo Consent</value></data>
<data name="ConsentLevel.NotCaptured" xml:space="preserve"><value>Not Captured</value></data>
<data name="ConsentLevel.Full" xml:space="preserve"><value>Full</value></data>
<data name="ConsentLevel.Limited" xml:space="preserve"><value>Limited</value></data>
<data name="ConsentLevel.None" xml:space="preserve"><value>None</value></data>
```

### 8d. Keys to add to `Index.ts-ZA.resx`

```xml
<data name="Column.PhotoConsent" xml:space="preserve"><value>[XS] Photo Consent</value></data>
<data name="PhotoConsent" xml:space="preserve"><value>[XS] Photo Consent</value></data>
<data name="ConsentLevel.NotCaptured" xml:space="preserve"><value>[XS] Not Captured</value></data>
<data name="ConsentLevel.Full" xml:space="preserve"><value>[XS] Full</value></data>
<data name="ConsentLevel.Limited" xml:space="preserve"><value>[XS] Limited</value></data>
<data name="ConsentLevel.None" xml:space="preserve"><value>[XS] None</value></data>
```

---

## 9. Implementation Sequence

Execute in this order to avoid compile errors:

1. **Shared model** — add `PhotoConsentLevel` enum + 4 properties to `Enrollment.cs`
2. **ViewModel** — add `PhotoConsentLevel` to `EnrollmentListViewModel`
3. **Repository** — map `PhotoConsentLevel` in `GetEnrollmentListViewModels`
4. **Service interface** — add `CapturePhotoConsentAsync` to `IEnrollmentService`
5. **Server service** — implement `CapturePhotoConsentAsync`
6. **Controller** — add `PhotoConsentRequest` DTO + `CapturePhotoConsent` endpoint
7. **Client service** — add `PhotoConsentRequest` DTO + client implementation
8. **New razor component** — create `PhotoConsent.razor`
9. **Index.razor** — add column header, badge cell, conditional action link, helper methods
10. **Resource files** — create `PhotoConsent.resx` + `PhotoConsent.ts-ZA.resx`; add keys to `Index.resx` + `Index.ts-ZA.resx`
11. **Database schema** — apply the 4 new columns via the project's schema tool (no separate `Sql/Scripts/Migration_AddPhotoConsent.sql` migration script; rely on the schema tool to generate and apply the changes)
12. **Build and test**

---

## 10. Gotchas

**ModuleId in the request body** — The controller calls `GetEnrollmentAsync(id, request.ModuleId)` then `IsAuthorizedEntityId(EntityNames.Module, enrollment.ModuleId)`. Pass `ModuleState.ModuleId` from the component (not 0) and use `CreateAuthorizationPolicyUrl` on the client, matching the pattern in other write endpoints.

**Canvas ID uniqueness** — Use `id="photoConsentSignatureCanvas"` (not `"signatureCanvas"`) to avoid collision with `Signature.razor` if both are ever mounted in the same page.

**`OnAfterRenderAsync` timing** — Keep the `Task.Delay(100)` guard after injecting the script tag, matching `Signature.razor`. Removing it causes intermittent JS errors on mobile.

**EF Core column discovery** — No `OnModelCreating` changes required. EF Core discovers the new scalar columns from the C# properties automatically. The `PhotoConsentLevel` enum stores as `INT DEFAULT 0` matching the existing `Status` column convention.

**Re-capture** — When `PhotoConsentSignatureCollected == true`, the `Save` handler overwrites the existing record. The component shows an info banner in this case. This is intentional — growers can change their mind.

---

## 11. Files Changed Summary

| File | Change |
|---|---|
| `Shared/Models/Enrollment.cs` | Add enum + 4 properties |
| `Shared/Models/EnrollmentViewModel.cs` | Add 1 property |
| `Server/Repository/EnrollmentRepository.cs` | Add mapping in list view query |
| `Client/Services/EnrollmentService.cs` | Add interface method + DTO + client impl |
| `Server/Services/EnrollmentService.cs` | Add service method |
| `Server/Controllers/EnrollmentController.cs` | Add endpoint + DTO |
| `Client/Modules/Enrollment/Index.razor` | Add column + badge + action link + helpers |
| `Client/Modules/Enrollment/PhotoConsent.razor` | **New file** |
| `Client/Resources/.../PhotoConsent.resx` | **New file** |
| `Client/Resources/.../PhotoConsent.ts-ZA.resx` | **New file** |
| `Client/Resources/.../Index.resx` | Add 6 keys |
| `Client/Resources/.../Index.ts-ZA.resx` | Add 6 keys |
| `Sql/dbo/Tables/Enrollment.sql` | Add 4 columns to DDL (applied via project schema tool) |

