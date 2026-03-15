---
priority: high
tags: [module, admin, growers, reporting]
---

# Implement Cohort Management Module

Feature spec: `Specs/Features/CohortManagement.feature`

Cohorts are named groups of growers within a village (e.g., "Roebuck 1 2026").
They track program phases, drive assessment frequency, and scope funder reports.

## Checklist

- [ ] DB table: `Cohort` (Id, VillageId, Name, Status [Planned|Active|Completed], CreatedDate) — SQL project
- [ ] DB table: `GrowerCohort` (GrowerCohortId, GrowerId, CohortId) — join table, replaces `Grower.CohortId` FK
- [ ] DB table: `MentorCohort` (MentorCohortId, MentorId, CohortId) — join table
- [ ] Server repository: `ICohortRepository` / `CohortRepository`
- [ ] Server controller: `CohortController` (CRUD + list-by-village)
- [ ] Client service: `CohortService`
- [ ] Admin UI: Cohort management index (list, create, rename, set status)
  - Auto-suggest name from village + year; allow override
  - Auto-increment number when village already has a cohort that year
  - Status transitions: Planned → Active → Completed
  - Default filter hides Completed; "Show completed" toggle reveals them
- [ ] Enrollment form: multi-select cohort picker (growers can belong to more than one)
- [ ] Grower record: add/remove individual cohort memberships post-enrollment
- [ ] Mentor assignment: assign mentor to one or more cohorts
- [ ] Grower list: cohort filter (admin sees all; mentor sees union of their assigned cohorts)
- [ ] Cohort summary view: member count, status, enrollment date range, assigned mentors
- [ ] Assessment schedule: frequency is per-cohort based on cohort activation year, not grower's oldest cohort
- [ ] Reporting: cohort scope filter on program reports and visit summary

## Source

- March 2026 check-in meeting (`Specs/Docs/10 Trees march Check-in.md`) — Rebecca described naming scheme ("Roebuck 2026", "Roebuck 1 2026"), multiple cohorts per village per year, and funder visit reporting
- Feb 3, 2026 meeting — cohort-scoped assessment frequency, 153 + 57 + 55 household groups
- Existing scenarios (to migrate): `VillageDataManagement.feature` lines 37–70
