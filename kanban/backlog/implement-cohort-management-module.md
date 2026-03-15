---
priority: high
tags: [module, admin, growers, reporting]
---

# Implement Cohort Management Module

Feature spec: `Specs/Features/CohortManagement.feature`

Cohorts are named groups of growers within a village (e.g., "Roebuck 1 2026").
They track program phases, drive assessment frequency, and scope funder reports.

## Checklist

- [x] DB table: `Cohort` (CohortId, VillageId, Name, Status, ActivatedOn) — `Sql/dbo/Tables/Cohort.sql`
- [x] DB table: `GrowerCohort` join table — `Sql/dbo/Tables/GrowerCohort.sql`
- [x] DB table: `MentorCohort` join table — `Sql/dbo/Tables/MentorCohort.sql`
- [x] DB migration: `Sql/Scripts/Migration_AddCohortIdToEnrollment.sql`
- [x] Shared models: `Cohort.cs`, `GrowerCohort.cs`, `MentorCohort.cs`; `CohortId` added to `Enrollment.cs`
- [x] EF context: 3 new DbSets in `TenTreesContext.cs`
- [x] Server repository: `CohortRepository.cs`
- [x] Server service: `CohortService.cs` (name suggestion, status transition guard)
- [x] Server startup: `CohortServerStartup.cs`
- [x] Server controller: `CohortController.cs` (13 endpoints including grower/mentor membership)
- [x] Client service: `CohortService.cs` (client)
- [x] Client startup: `CohortClientStartup.cs`
- [x] Admin UI: `Client/Modules/Cohort/Index.razor` + `Edit.razor` + `ModuleInfo.cs`
  - Status filter tabs (Active/Planned/Completed/All), member count, activated date
  - Auto-suggest name; status transition guard in Save()
  - Grower membership panel (list + remove)
  - Mentor assignment panel (list + remove)
- [x] Enrollment form: cohort picker in `BasicInfo.razor` (loads active cohorts for selected village)
  - `CohortId` stored on `EnrollmentDraft` and `Enrollment` model
  - `GrowerCohort` row created on enrollment approval in `EnrollmentRepository.UpdateEnrollment`
- [x] Grower list: cohort filter in `Grower/Index.razor`
  - Admin: village → cohort cascade filter
  - Mentor: auto-restricted to growers in their assigned cohorts
- [x] Assessment frequency: `AssessmentService.CanSubmitAssessmentAsync` now uses cohort `ActivatedOn` year (not grower `CreatedOn`)
- [ ] Grower record: add/remove individual cohort memberships post-enrollment (via Edit.razor grower membership panel — needs grower name lookup)
- [ ] Mentor assignment UI: assign mentor to cohort from Edit.razor (needs mentor search/picker)
- [ ] Reporting: cohort scope filter — **deferred until reporting module is built** (`ProgramReporting.feature`)

## Source

- March 2026 check-in meeting (`Specs/Docs/10 Trees march Check-in.md`) — Rebecca described naming scheme ("Roebuck 2026", "Roebuck 1 2026"), multiple cohorts per village per year, and funder visit reporting
- Feb 3, 2026 meeting — cohort-scoped assessment frequency, 153 + 57 + 55 household groups
- Existing scenarios (to migrate): `VillageDataManagement.feature` lines 37–70
