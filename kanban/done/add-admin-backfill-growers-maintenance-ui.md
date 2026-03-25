---
priority: low
tags: [enrollment, admin, maintenance]
---

# Add admin backfill-growers maintenance UI

## Context

The `POST /api/Enrollment/backfill-growers` endpoint already exists in
`EnrollmentController.cs`. It creates missing `Grower` records for approved
enrollments that were submitted before the Grower table was introduced
(or where the auto-create logic failed).

There is currently no UI surface to trigger this action — admins must call
the API directly.

## Spec Reference

`Specs/Features/GrowerEnrollment.feature` — *"Admin backfills grower records
from existing enrollments"*

## What's Needed

A one-click admin button, likely in the Enrollment module settings or an
admin-only section of the Enrollment index, that calls the endpoint and
reports how many Grower records were created.

## Checklist

- [ ] Add "Backfill Growers" button visible only to Admin/Executive Director roles
- [ ] Place in `Enrollment/Settings.razor` or as a collapsible admin panel in `Index.razor`
- [ ] Call `POST api/Enrollment/backfill-growers?moduleId=X`
- [ ] Display result: "N grower records created" (or "All records up to date" if 0)
- [ ] Confirm dialog before running ("This will create Grower records for all approved enrollments that lack one. Continue?")
- [ ] Add en-ZA resource keys for button label, confirm message, and result messages
- [ ] Ensure endpoint requires Admin role (add `[Authorize(Roles = RoleNames.Admin)]` if not already present — see `tech-grower-status-management` item 7)

## Technical Notes

- Endpoint: `EnrollmentController.BackfillGrowers(int moduleId)`
- Authorization: currently uses `PolicyNames.EditModule` — may want to add explicit Admin role check
- Safe to run multiple times (idempotent — skips enrollments that already have a linked Grower)
