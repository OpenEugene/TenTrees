---
priority: medium
tags: []
---

# Add Cohort/Group management for organizing growers within villages

## Context

Per the Feb 3, 2026 stakeholder meeting, Rebecca described organizing households into cohorts/groups within a village:

- **Orpen Gate Village Group 1**: 153 households (initial cohort)
- **Orpen Gate Village Group 2**: 57 households (added 2025)
- Future groups as program expands (currently at ~18% of village households, goal is 20-25%)

Rebecca confirmed that "group" and "cohort" mean the same thing. Currently tracked via separate spreadsheet tabs per year.

## What's Missing

There is no feature file covering cohort/group management. `VillageDataManagement.feature` only scopes data by village, not by group within a village.

## Proposed Feature: CohortManagement.feature

Should cover:
- [ ] Create a new cohort/group within a village (e.g., "Orpen Gate Village Group 3")
- [ ] Assign growers to a cohort at enrollment time
- [ ] Filter grower lists by cohort
- [ ] View cohort summary (household count, enrollment date range)
- [ ] Cohort determines assessment frequency — Year 1 cohorts get twice-monthly visits, Year 2 get monthly
- [ ] Staff can view all cohorts; mentors see only their assigned cohort(s)
- [ ] Naming is flexible — Rebecca wants to decide with her team (could be "Group 1", "2025 Cohort", etc.)

## Relationship to Existing Features

- `GardenAssessment.feature` already has Year 1 / Year 2 frequency scenarios — cohort assignment would drive which year a grower is in
- `VillageDataManagement.feature` needs cohort as a sub-filter within village
- `ProgramReporting.feature` should support filtering/grouping reports by cohort

## Source

Feb 3, 2026 meeting transcript (~14:51–17:42 timestamps)
