[Home](../Home.md) / [Specifications](../Specifications.md) / Role Based Data Visibility <!-- wikidown:breadcrumb -->

# Role-Based Data Visibility

Every 10 Trees platform user sees only the data and features relevant to their role, protecting privacy and streamlining the experience. This is a high-priority security rule: the platform is shared across many villages and cohorts, and each user must only ever see what they are assigned to.

## The three primary roles

| Role | Scope | Description |
|---|---|---|
| Mentor | Assigned | Field agents scoped strictly to their assigned growers (via cohort, village, or direct assignment) |
| Project Manager | Global | Centre staff with global read/write access to program data and reporting, but no system admin rights |
| 10 Trees Admin | Global | Full access to all program data, user management, and configuration |

## Mentor data scoping

- A mentor assigned to cohorts sees exactly the growers in those cohorts — no growers from other cohorts or villages. For example, a mentor assigned only to cohort "Roebuck 1 2026", which has 15 active growers, sees exactly those 15 growers on the grower list.
- A mentor with **no** cohort assignments falls back to seeing all growers in their assigned village. For example, a mentor assigned to "Orpen Gate Village" with no cohort assignments sees every grower in Orpen Gate Village and none from "Londelozzi".
- Direct navigation to an unassigned grower's record is denied with "You are not assigned to this household". This applies even when the mentor knows the record exists — for example, a mentor who opens the record for "Peter Mthembu" while Peter is assigned to a different mentor is refused and shown that message.

## PM and Admin global visibility

Project Managers and 10 Trees Admins get a village filter dropdown on the grower list, including an "All Villages" option that shows every grower in the program.

## Feature access by role

| Feature | Mentor | Project Manager | 10 Trees Admin |
|---|---|---|---|
| Submit New Enrollment | Allowed | Allowed | Allowed |
| Approve/Reject Enrollment | Denied | Allowed | Allowed |
| Submit Garden Assessment | Allowed | — | — |
| Change Grower Status (Exit) | Read-Only | Allowed | Allowed |
| Program Reporting Module | Denied | Allowed | Allowed |
| Mentor Management Module | Denied | Denied | Allowed |

## Navigation streamlining

- **Mentor** navigation shows "Enrollment", "Growers", "Assessments", and "Classes" — and hides "Reports", "Mentors", "Villages", and "Cohorts".
- **10 Trees Admin** navigation shows all data entry modules plus "Reports", "Mentors", "Villages", and "Cohorts".

## Related pages

- [Mentor Management](/Specifications/Mentor-Management) — mentor-specific permission details and data isolation mechanics.
- [User Administration](/Specifications/User-Administration) — full role permission matrix and the platform/programme admin split.
- [Village Data Management](/Specifications/Village-Data-Management)
