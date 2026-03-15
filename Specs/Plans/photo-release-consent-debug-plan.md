# Photo Release Consent — Debug & Test Plan

**Generated:** 2026-03-15  
**Based on:** `Specs/Plans/photo-release-consent.md` + `Specs/Features/PhotoReleaseConsent.feature`  
**Status:** One critical bug fixed (see §2). All other implementation matches the plan.

---

## 1. Plan vs Feature vs Implementation — Comparison Summary

### Feature Spec (BDD Scenarios)

| # | Scenario | Pre-condition | Action | Expected |
|---|---|---|---|---|
| 1 | Full consent for Mary Nkuna | Approved enrollment | Select "You may use my photo with my name identified" + signature | Level = Full, linked to enrollment |
| 2 | Limited consent | Approved enrollment | Select "You may use my picture in group photos without my name" + signature | Level = Limited |
| 3 | No consent | Approved enrollment | Select "You may not use my photo at all" + signature | Level = None |

All three scenarios require an **Approved** enrollment — the guard in `PhotoConsent.razor` (`OnParametersSetAsync`) enforces this correctly.

### Implementation Coverage by Plan Section

| Plan Step | File | Status | Notes |
|---|---|---|---|
| 1a — `PhotoConsentLevel` enum | `Shared/Models/Enrollment.cs` | ✅ | Correct values: 0–3 |
| 1b — 4 new properties on `Enrollment` | `Shared/Models/Enrollment.cs` | ✅ | Placed after `// Status` section |
| 2b — SQL DDL update | `Sql/dbo/Tables/Enrollment.sql` | ✅ | 4 columns added before `[Status]` |
| 3a — `PhotoConsentRequest` DTO | `Server/Controllers/EnrollmentController.cs` | ✅ | After `SignatureRequest` |
| 3b — `CapturePhotoConsent` endpoint | `Server/Controllers/EnrollmentController.cs` | ✅ | Auth checks present |
| 3c — Server service method | `Server/Services/EnrollmentService.cs` | ✅ | Sets all 4 fields |
| 4a — Interface method | `Client/Services/EnrollmentService.cs` | ✅ | |
| 4b/4c — Client DTO + impl | `Client/Services/EnrollmentService.cs` | ✅ | Uses `CreateAuthorizationPolicyUrl` |
| 5 — `EnrollmentListViewModel.PhotoConsentLevel` | `Shared/Models/EnrollmentViewModel.cs` | ✅ | |
| 5 — Repository mapping | `Server/Repository/EnrollmentRepository.cs` | ✅ | `GetEnrollmentListViewModels` |
| 6 — `PhotoConsent.razor` | `Client/Modules/Enrollment/PhotoConsent.razor` | ⚠️ Bug fixed | See §2 |
| 7a/7b — Index table columns | `Client/Modules/Enrollment/Index.razor` | ✅ | Badge + conditional action link |
| 7c — Index helper methods | `Client/Modules/Enrollment/Index.razor` | ✅ | |
| 8a — `PhotoConsent.resx` | `Client/Resources/.../PhotoConsent.resx` | ✅ | All keys present |
| 8b — `PhotoConsent.ts-ZA.resx` | `Client/Resources/.../PhotoConsent.ts-ZA.resx` | ✅ | `[XS]` placeholders in place |
| 8c — `Index.resx` additions | `Client/Resources/.../Index.resx` | ✅ | 6 keys added |
| 8d — `Index.ts-ZA.resx` additions | `Client/Resources/.../Index.ts-ZA.resx` | ✅ | 6 keys added |
| 11 — DB migration script | `Sql/Scripts/Migration_AddPhotoConsent.sql` | ✅ (omitted by design) | Plan §9 step 11 says use schema tool, no script |

---

## 2. Bug Found and Fixed

### BUG-01 (Critical): Signature canvas never initialized in `PhotoConsent.razor`

**Root cause:** The original `OnAfterRenderAsync` used `if (firstRender && _enrollment != null)`.  
`PhotoConsent.razor` loads enrollment data via an async HTTP call in `OnParametersSetAsync`.  
On the **first render** (`firstRender=true`), `_enrollment` is always `null` (HTTP call not yet complete).  
By the **second render** (when `_enrollment` is set), `firstRender=false` — the condition never becomes true.  
Result: `SignaturePad.init` is never called; the canvas renders but accepts no input.

**Why `Signature.razor` works:** Its `OnParametersSet()` is **synchronous**, reading from in-memory  
`StateService.CurrentDraft`. The data is available before the first render, so `firstRender=true &&  CurrentDraft != null` is a valid combination.

**Fix applied** (`Client/Modules/Enrollment/PhotoConsent.razor`):

```csharp
// Before (broken):
protected override async Task OnAfterRenderAsync(bool firstRender)
{
    if (firstRender && _enrollment != null)  // _enrollment always null on firstRender

// After (fixed):
private bool _padInitialized = false;

protected override async Task OnAfterRenderAsync(bool firstRender)
{
    if (!_padInitialized && _enrollment != null && !_isLoading)
    {
        _padInitialized = true;
        // ... init pad
    }
```

The `_padInitialized` flag prevents re-initialization on subsequent renders (e.g. after `StateHasChanged`  
during save).

---

## 3. Manual Debug Checklist (Run After Build)

### 3a. Index page — Photo Consent column

- [ ] Navigate to the Enrollment module index
- [ ] **Column header** "Photo Consent" appears in the table
- [ ] Enrollments with no consent show **grey "Not Captured" badge** (`bg-secondary`)
- [ ] After capturing Full consent, badge shows **green "Full"** (`bg-success`)
- [ ] After capturing Limited consent, badge shows **yellow "Limited"** (`bg-warning text-dark`)
- [ ] After capturing None, badge shows **red "None"** (`bg-danger`)
- [ ] **Photo Consent action link appears only for Approved enrollments** (Pending/Rejected rows have no link)

### 3b. PhotoConsent form — Load

- [ ] Click the Photo Consent action link on an Approved enrollment
- [ ] Page shows grower name and village name in the context header
- [ ] Page shows the three consent option radio buttons
- [ ] Page shows the signature canvas with "Sign here" placeholder
- [ ] Page shows the confirmation checkbox (unchecked)
- [ ] Save button is **disabled** on initial load (no consent selected, not confirmed)
- [ ] If consent was previously captured: the "already recorded" info banner appears and the existing consent level is pre-selected

### 3c. PhotoConsent form — Guard

- [ ] Manually navigate to `?action=PhotoConsent&id=<pending-enrollment-id>` — should redirect to Index
- [ ] Manually navigate with no `id` param — should show "Enrollment not found" alert
- [ ] Manually navigate with a non-existent id — should show "Enrollment not found" alert

### 3d. Signature canvas (Bug-01 fix verification)

- [ ] Signature canvas is **drawable** (mouse/touch creates strokes)
- [ ] "Clear" button erases strokes and restores the "Sign here" placeholder
- [ ] Attempting to save with an empty canvas shows "Signature is required" warning
- [ ] After drawing, the canvas is not blank (`isBlank` returns false)

### 3e. Consent save — Happy path

- [ ] Select a consent level → radio button highlights with coloured border
- [ ] Check the confirmation checkbox
- [ ] Draw a signature
- [ ] Save button becomes enabled
- [ ] Click Save → spinner appears, button disables
- [ ] On success: success message appears, navigates back to Index
- [ ] Index badge for that enrollment now reflects the saved consent level
- [ ] Clicking Photo Consent again shows the "already captured" banner with correct level pre-selected

### 3f. Consent save — Validation errors

- [ ] With consent selected but checkbox unchecked: Save is disabled (button `disabled` attr)
- [ ] With checkbox checked but `NotCaptured` still selected: Save is disabled
- [ ] With both checked but blank canvas: Save button enabled → click → "Signature is required" warning shown
- [ ] On server error (simulate by disconnecting): "Error saving photo consent" message shown, `_isSaving` resets

---

## 4. BDD Scenario Test Steps

### Scenario 1 — Full consent for Mary Nkuna

```
Given: Approved enrollment exists for "Mary Nkuna"
1. Open Enrollment Index
2. Confirm "Not Captured" grey badge on Mary Nkuna's row
3. Click the Photo Consent action link on her row
4. Verify page header shows "Mary Nkuna" and correct village
5. Select radio "Full consent" ("You may use my photo with my name identified")
6. Verify the Full option highlights green
7. Draw signature on canvas
8. Check confirmation checkbox
9. Click "Save Consent"
Expected: Navigates to Index, Mary Nkuna's badge is now green "Full"
```

### Scenario 2 — Limited consent

```
Given: Different Approved enrollment
1. Click Photo Consent action link
2. Select "Limited consent" ("You may use my picture in group photos without my name")
3. Verify the Limited option highlights yellow
4. Draw signature, check confirmation, Save
Expected: Badge shows yellow "Limited"
```

### Scenario 3 — No consent

```
Given: Different Approved enrollment
1. Click Photo Consent action link
2. Select "No consent" ("You may not use my photo at all")
3. Verify the None option highlights red
4. Draw signature, check confirmation, Save
Expected: Badge shows red "None"
```

### Scenario 4 — Re-capture (not in BDD spec, but covered by plan §10)

```
Given: Enrollment already has Full consent captured
1. Click Photo Consent action link
2. Verify "Consent has already been recorded as: Full consent" banner
3. Verify Full radio is pre-selected
4. Change to Limited, draw new signature, confirm, Save
Expected: Badge updates to "Limited"
```

---

## 5. API Verification (Browser DevTools / HTTP Client)

| Endpoint | Method | Expected Response |
|---|---|---|
| `GET api/Enrollment/listviewmodels?moduleid=X` | GET | Each item includes `photoConsentLevel` field |
| `POST api/Enrollment/{id}/photoconsent` | POST `{moduleId, consentLevel:1, signatureData:"<svg>"}` | `true` |
| `POST api/Enrollment/{id}/photoconsent` | POST with wrong moduleId | `403 Forbidden` |
| `GET api/Enrollment/{id}/{moduleId}` after save | GET | `photoConsentLevel`, `photoConsentSignatureCollected: true`, `photoConsentSignatureDate` are set |

---

## 6. Known Pre-existing Issue (Out of Scope)

**`CaptureSignatureAsync` in `Client/Services/EnrollmentService.cs` uses `ModuleId = 0`:**

```csharp
// Existing enrollment signature — ModuleId is hardcoded 0 (pre-existing bug, unrelated to photo consent)
return await PostJsonAsync<SignatureRequest, bool>($"{Apiurl}/{enrollmentId}/signature",
    new SignatureRequest { ModuleId = 0, SignatureData = signatureData });
```

The `CapturePhotoConsentAsync` implementation is correct and does not repeat this mistake.  
The signature bug should be tracked separately.

---

## 7. Post-Deploy Database Check

After applying the schema migration, verify the 4 new columns exist:

```sql
SELECT name, type_name(system_type_id) AS type, is_nullable, default_object_id
FROM sys.columns
WHERE object_id = OBJECT_ID('dbo.Enrollment')
  AND name IN ('PhotoConsentLevel','PhotoConsentSignatureCollected',
               'PhotoConsentSignatureDate','PhotoConsentSignatureData');
-- Expected: 4 rows
-- PhotoConsentLevel: int, not null, default 0
-- PhotoConsentSignatureCollected: bit, not null, default 0
-- PhotoConsentSignatureDate: datetime2, nullable
-- PhotoConsentSignatureData: nvarchar(max), nullable
```

Verify existing rows default correctly:
```sql
SELECT COUNT(*) FROM dbo.Enrollment WHERE PhotoConsentLevel != 0;
-- Expected: 0 (all existing records default to NotCaptured)
```
