---
priority: medium
tags: [village, enrollment, workflow-village]
---

# Add inactive village filtering to enrollment and grower dropdowns

## Context

The `Village` model has an `IsActive` flag and `VillageController` exposes a
`GET /api/Village/active` endpoint. However, the village picker in
`Enrollment/BasicInfo.razor` and anywhere else growers select a village may
be loading all villages — including inactive ones — rather than filtering to
`IsActive = true`.

## Spec Reference

`Specs/Features/VillageDataManagement.feature` — *"Deactivate a village:
'Old Site' should no longer appear in the active village dropdown. Existing
grower records linked to it should remain intact."*

## Checklist

### Verify current behaviour
- [ ] Check `Enrollment/BasicInfo.razor` village picker — which endpoint does it call? (`/api/Village` vs `/api/Village/active`)
- [ ] Check `Grower/Index.razor` village filter dropdown — same question
- [ ] Check `Training/Index.razor` and `Training/Edit.razor` village selectors
- [ ] Check `Report` / `ProgramReporting` village filter (when implemented)

### Fix dropdowns that load all villages
- [ ] Enrollment village picker → switch to `/api/Village/active` (or pass `activeOnly=true` param)
- [ ] Grower list village filter → admin sees all; standard user sees active only
- [ ] Training class creation village picker → active only
- [ ] Any other new-record form that accepts a village → active only

### Preserve existing records
- [ ] Confirm that deactivating a village does NOT break existing enrollment, grower, or assessment records linked to that village
- [ ] Enrollment edit of an existing record linked to an inactive village should still display the village name (read-only or with warning)

### Localization
- [ ] No new resource keys expected unless a warning label is added for inactive-village records

## Technical Notes

- `VillageController` already has `GET /api/Village/active` returning `IsActive = true` records
- `VillageService.GetActiveVillagesAsync` exists server-side
- The client `IVillageService` interface needs a corresponding `GetActiveVillagesAsync` method if not already present
