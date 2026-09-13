[Home](../Home.md) / [Specifications](../Specifications.md) / Cohort Management <!-- wikidown:breadcrumb -->

# Cohort Management

Users with edit permissions organise growers and mentors into named, village-scoped cohorts so the program can track phases, run cohort-scoped reports, and manage each group independently. This is a high-priority workflow.

## Cohort lifecycle

A cohort belongs to a village and a year, and moves through three statuses:

- **Planned** — the status of every newly created cohort. On creation the system suggests a name of the form "Village Year" (e.g. "Roebuck 2026"), which can be accepted or overwritten with a custom name (e.g. saving a cohort for "Orpen Gate Village" under the name "Orpen Gate Village 2023" creates exactly that cohort for that village).
- **Active** — set by an authorized user; the cohort then appears in active cohort filters.
- **Completed** — the cohort leaves active filters by default but is visible when "Show completed cohorts" is enabled. **A completed cohort cannot be reactivated**; attempts to set it back to Active or Planned are rejected with "A completed cohort cannot be reactivated" and the status stays Completed.

When a village already has a cohort for that year, the system increments the number in the suggested name (second cohort → "Roebuck 2 2026") and the existing cohort can be renamed to match (e.g. "Roebuck 1 2026").

## Assigning growers

- At enrollment, cohort selection is **optional**. If an active cohort exists for the village the enroller may pick one (e.g. enrolling "Nomsa Dlamini" into "Roebuck" and choosing "Roebuck 1 2026" makes her a member of that cohort); growers enrolled without a cohort belong to no cohort and can be added later from the grower status screen. See [Grower Enrollment](/Specifications/Grower-Enrollment).
- A grower can belong to **multiple cohorts** (e.g. a member of "Orpen Gate Village 2023" can also be added to the active "Orpen Gate Village 2024") and appears when filtering by any of them.
- Authorized users can remove a grower from a cohort, leaving their other memberships intact.

## Cohort tags on related screens

The grower status screen has a **Cohorts** section showing membership as badges:

- Admins can add a cohort via an "Add cohort" dropdown (which excludes Completed cohorts, so an Active "Orpen Gate Village 2024" is offered while a Completed "Orpen Gate Village 2023" is not) and remove one via the badge's remove button with an inline "Remove …? Yes / No" confirmation; confirming with "Yes" ends the membership.
- Mentors see the badges read-only — no remove buttons, no add dropdown.

## Class association

Training classes and cohorts can be linked from either side — the training edit screen ("Add cohort" dropdown) or the cohort edit screen ("Add class" dropdown). Links display as tags/badges on both screens and on the training index's cohorts column. Removing a link always asks an inline Yes/No confirmation (e.g. "Remove Roebuck 1 2026? Yes / No"); choosing "No" keeps the link. See also [Class Attendance](/Specifications/Class-Attendance).

## Assigning mentors

Admins assign mentors to cohorts. A mentor can be assigned to multiple cohorts and sees growers from all of them in their grower list. See [Mentor Management](/Specifications/Mentor-Management).

## Viewing and filtering

- Cohort management lists all cohorts with status and household counts — for example "Orpen Gate Village 2023" (Completed, 153 households), "Orpen Gate Village 2024" (Active, 57) and "Roebuck 1 2026" (Planned, 55) all appear together regardless of status.
- The grower list can be filtered by cohort, showing only members.
- A mentor sees growers from all of their assigned cohorts and none from cohorts they aren't assigned to.

## Cohort summary

The summary for a cohort shows its member count (e.g. 55), status, enrollment date range, and assigned tree mentors.

## Assessment frequency

Each cohort has its own assessment frequency, set in days by centre staff when the cohort is created or edited. It drives the assessment date proximity warning: when a mentor enters an assessment dated within that many days of another assessment for the same grower, the form warns them so they can confirm they are not duplicating a recent visit. The warning never blocks saving — see [Garden Assessment](/Specifications/Garden-Assessment).

## Reporting

Program reports can be scoped to a specific cohort so results reflect only that cohort's households (e.g. the 55 households of "Roebuck 1 2026"), including visit-count summaries for funder reporting (e.g. a visit summary for that cohort for March 2026 showing the 25 home visits recorded that month). See [Program Reporting](/Specifications/Program-Reporting).
