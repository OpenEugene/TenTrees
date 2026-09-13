[Home](../Home.md) / [Specifications](../Specifications.md) / User Administration <!-- wikidown:breadcrumb -->

# User Administration

Administrators manage user accounts and permissions so the right people have appropriate access. This is a high-priority security concern: getting assignments and roles wrong exposes household data to people who should not see it.

## Admin impersonation for data entry

An administrator can enter data on behalf of a mentor (e.g. from paper assessment forms) via "Enter data as <mentor>". The record is attributed to the mentor, and the audit log captures the action, who entered it, who it was on behalf of, and the timestamp. For example, when administrator Becky selects "Enter data as Bondi" and completes an assessment for Mary Nkuna from Bondi's paper forms, the assessment is recorded with Bondi as the mentor, and the audit log shows the action "Assessment Created", entered by Becky, on behalf of Bondi, at the current time.

## Assignments

- A mentor assigned to a village sees only that village's data, and the assignment is recorded. For example, once a 10 Trees Admin assigns Bondi to "Orpen Gate Village", she sees only Orpen Gate Village data.
- Within a village, specific households can be assigned to a mentor; those households appear only in that mentor's list. For example, if Bondi is assigned households 1–10 in Orpen Gate Village, her grower list shows exactly those ten households, other mentors do not see them as assigned to them, and Bondi does not see households assigned to other mentors.
- Creating a mentor account (name, role, village, email) generates login credentials. For example, a 10 Trees Admin creating "Thandi Nkosi" as a Mentor in Orpen Gate Village with email thandi@example.com results in a created account with generated credentials. See [Mentor Management](/Specifications/Mentor-Management) for full mentor CRUD.

## Role permission matrix

| Role | Submit Forms | View All Villages | Add Notes | Export | Manage Users |
|---|---|---|---|---|---|
| Mentor | Yes | No | No | No | No |
| Educator | No | Yes | Yes | No | No |
| Project Manager | No | Yes | Yes | Yes | No |
| 10 Trees Admin | Yes | Yes | Yes | Yes | Yes |

When a user logs in, their permissions are exactly those of their role's row — no more and no less.

An **Educator** can view any grower's full record in any village and add home visit notes, but cannot edit enrollment or assessment data. For example, an Educator opening Mary Nkuna's record in any village sees her full record and can add a home visit note, but is offered no option to edit her enrollment or assessment data.

### Role naming notes (from the spec)

- There is **no Executive Director role** — the organisation uses distributed leadership. Tri holds the title "Director of Permaculture Education and Community Development". The chief admin role is "10 Trees Admin" (not "center admin").
- The correct term for the permaculture educator role is still under discussion (Rebecca to confirm with Tri).
- **Open question (March 2026 check-in):** whether the Educator role should be able to edit grower record data beyond adding notes is unresolved; the current spec treats Educator as view + notes only, pending confirmation from Rebecca.

## Platform admin vs programme admin

The Oqtane **Platform Admin** (system-level) and the **10 Trees Admin** (programme-level) are separate roles, even when held by the same person, and role names in the UI should be prefixed to avoid confusion:

| Role | Manages Programme Data | Can Add/Remove Pages | Can Manage Site Settings |
|---|---|---|---|
| 10 Trees Admin | Yes | No | No |
| Platform Admin | No | Yes | Yes |

A user holding only 10 Trees Admin can create, edit, and assign programme records but never sees site management options ("Add Page", "Manage Modules") — the programme director has full data access across all villages without any risk of inadvertently altering site structure. For example, programme director Rebecca, holding only the 10 Trees Admin role, sees growers from all villages on the grower list and can create, edit, and assign programme records, but is never shown site management options. A user holding only Platform Admin manages pages and site settings but has no elevated access to programme data.

## Related pages

- [Role-Based Data Visibility](/Specifications/Role-Based-Data-Visibility)
- [Village Data Management](/Specifications/Village-Data-Management)
