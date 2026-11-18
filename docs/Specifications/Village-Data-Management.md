[Home](../Home.md) / [Specifications](../Specifications.md) / Village Data Management <!-- wikidown:breadcrumb -->

# Village Data Management

Data is organized by village so each village sees only its own data while authorized users see all. This is a high-priority, multi-tenant concern: the village is the boundary that keeps one community's records separate from another's.

## Village scoping

- A mentor assigned to a village sees only that village's growers in the grower list — never another village's. For example, mentor Bondi, assigned to Orpen Gate Village, sees Orpen Gate Village growers and none from Londelozzi.
- Village data is fully isolated: if Orpen Gate Village has 50 growers and Londelozzi has 30, a mentor from Londelozzi sees exactly those 30, with nothing visible from Orpen Gate Village.
- Users with village management permissions get a village filter dropdown with an "All Villages" option; selecting "All Villages" shows growers from every village, and selecting a specific village (such as Orpen Gate Village) narrows the list to it.

## Adding a village

A user with village edit permissions can add a new village (for example, Londelozzi) from village management, set its contact information, and the village becomes available for mentor assignment.

## Cohort and mentor filtering in the grower list

Cohorts are village-scoped groupings of growers. When an authorized user selects a village in the grower list, a cohort filter dropdown appears listing that village's cohorts (for example, "Orpen Gate Village 2023" and "Orpen Gate Village 2024" for Orpen Gate Village); selecting one filters the list to that cohort's members only, and growers from other cohorts do not appear. A mentor filter follows the cohort filter: it lists the mentors assigned to the selected cohort — or, when the cohort has none assigned directly, the mentors of the growers shown — and choosing one shows only that mentor's growers. The village, cohort and mentor choices are kept in the page address, so a filtered list survives a refresh and can be bookmarked or shared. Full cohort lifecycle, assignment, and tag behaviour is documented in [Cohort Management](/Specifications/Cohort-Management).

## Related pages

- [Mentor Management](/Specifications/Mentor-Management) — village assignment of mentors.
- [Role-Based Data Visibility](/Specifications/Role-Based-Data-Visibility)
