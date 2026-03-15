---
priority: high
tags: [module, admin, growers, reporting]
---

# Implement Cohort Management Module

Feature spec: `Specs/Features/CohortManagement.feature`

Cohorts are named groups of growers within a village (e.g., "Roebuck 1 2026").
They track program phases, drive assessment frequency, and scope funder reports.

## Checklist

- [ ] DB table: `Cohort` (Id, VillageId, Name, CreatedDate) — SQL project
- [ ] DB column: `Grower.CohortId` FK — SQL project
- [ ] Server repository: `ICohortRepository` / `CohortRepository`
- [ ] Server controller: `CohortController` (CRUD + list-by-village)
- [ ] Client service: `CohortService`
- [ ] Admin UI: Cohort management index (list, create, rename)
  - Auto-suggest name from village + year; allow override
  - Auto-increment number when village already has a cohort that year
- [ ] Enrollment form: cohort selector when enrolling a grower
- [ ] Grower list: cohort filter dropdown (admin sees all; mentor sees assigned cohort)
- [ ] Cohort summary view: household count, enrollment date range, assigned mentors
- [ ] Assessment schedule: derive frequency from cohort enrollment year (Year 1 = twice-monthly, Year 2 = monthly)
- [ ] Reporting: cohort scope filter on program reports and visit summary

## Source

- March 2026 check-in meeting (`Specs/Docs/10 Trees march Check-in.md`) — Rebecca described naming scheme ("Roebuck 2026", "Roebuck 1 2026"), multiple cohorts per village per year, and funder visit reporting
- Feb 3, 2026 meeting — cohort-scoped assessment frequency, 153 + 57 + 55 household groups
- Existing scenarios (to migrate): `VillageDataManagement.feature` lines 37–70
