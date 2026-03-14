---
priority: high
tags: [enrollment, mobile, workflow-enrollment]
---

# Implement Grower Enrollment Module

Four-step enrollment wizard with digital signature, status management, list view with
dashboard and filters, and full back-end CRUD.

## Spec Reference

`Specs/Features/GrowerEnrollment.feature`

## Checklist

### Step 1 — Basic Info (`BasicInfo.razor`)
- [x] Village picker (active villages only)
- [x] Grower name (required)
- [x] House number
- [x] ID number or birth date
- [x] Household size
- [x] Owns home (yes/no)
- [x] Auto-fill tree mentor from logged-in user; override picker for staff
- [x] Default enrollment date to today

### Step 2 — Preferred Criteria (`Criteria.razor`)
- [x] 6 yes/no questions (PE enrolled, PE graduate, garden tended, child-headed, woman-headed, empty yard)

### Step 3 — Commitments (`Commitments.razor`)
- [x] 6 commitment acknowledgments (no chemicals, attend classes, no tree cutting, women/children, care while away, yard access)

### Step 4 — Digital Signature (`Signature.razor`)
- [x] Touch-based canvas signature pad
- [x] Clear/redraw capability
- [x] Confirmation checkbox required before submit
- [x] Signature stored as SVG string in `SignatureData`
- [x] `SignatureCollected = true`, `SignatureDate = today` on save
- [x] Validation error if canvas is blank on submit
- [x] Page scroll suppressed while drawing

### API & Back-End (`EnrollmentController`, `EnrollmentService`, `EnrollmentRepository`)
- [x] CRUD endpoints (GET, POST, PUT, DELETE)
- [x] `listviewmodels` endpoint returning `EnrollmentListViewModel` with village name
- [x] `validate` endpoint
- [x] `{id}/signature` endpoint for signature capture
- [x] `mentor/{userid}` auto-fill endpoint
- [x] `status/{status}` filter endpoint
- [x] `village/{villageid}` filter endpoint
- [x] `backfill-growers` maintenance endpoint (backend only — see separate card for UI)

### List View (`Index.razor`)
- [x] Status summary dashboard cards (Pending / Approved / Rejected / Total)
- [x] Dual status columns: `EnrollmentStatus` badge + `GrowerStatus` badge per row
- [x] Row highlight: yellow for Pending, red for Rejected
- [x] Filter by enrollment status dropdown
- [x] Filter by village dropdown
- [x] Clear Filters button resets both dropdowns
- [x] Village name resolved from lookup (not raw ID)

### Localization
- [x] `Index.resx` and `Edit.resx` (en-ZA)
- [x] Localization keys for all labels, badges, filters, and messages
