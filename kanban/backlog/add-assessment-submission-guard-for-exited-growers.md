---
priority: medium
tags: [assessment, grower-status, workflow-assessment]
---

# Add assessment submission guard for exited growers

## Context

The `GET api/Assessment/can-submit/{growerId}` endpoint already exists in
`AssessmentController.cs` and is backed by `AssessmentService.CanSubmitAssessmentAsync`.
This checks frequency rules (Year 1 twice-monthly, Year 2 monthly).

However, neither the current endpoint logic nor the `Assessment/Edit.razor`
UI explicitly blocks assessment creation for growers with `GrowerStatus.Exited`.
An exited grower should not receive further assessments.

## Spec Reference

`Specs/Features/GardenAssessment.feature` — *"Prevent assessment submission for
an exited grower"* and *"System verifies submission eligibility before loading
assessment form"*

## Checklist

### Back-End
- [ ] Extend `CanSubmitAssessmentAsync` (or `AssessmentService`) to return `false` when the linked `Grower.Status == GrowerStatus.Exited`
- [ ] Return a reason code or message alongside the boolean so the UI can show an appropriate message (e.g., `{ CanSubmit: false, Reason: "GrowerExited" }`)
- [ ] Add unit/integration test covering the exited-grower case

### Front-End (`Assessment/Edit.razor`)
- [ ] On page load, call `can-submit/{growerId}` before rendering the form
- [ ] If `CanSubmit = false` due to exited status, show an alert: "This grower has left the program and cannot receive new assessments" and hide the form
- [ ] If `CanSubmit = false` due to frequency (too recent), show the existing "assessment too soon" message
- [ ] Add en-ZA and ts-ZA resource keys for the exited-grower message

### Assessment Index (`Assessment/Index.razor`)
- [ ] Consider hiding or disabling the "New Assessment" action link for growers with Exited status in any grower-scoped list view

## Technical Notes

- `CanSubmitAssessmentAsync` currently only checks assessment frequency
- Grower status check requires joining `Grower` table or injecting `IGrowerService`
- The frequency check should still apply for Active/Inactive growers as before
