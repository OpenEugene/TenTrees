[Home](../Home.md) / [Specifications](../Specifications.md) / Mentor Management <!-- wikidown:breadcrumb -->

# Mentor Management

10 Trees Admins manage tree mentor accounts, village assignments, and grower assignments so mentors can access exactly the data they need and no more. This is a high-priority, security-sensitive area of the application.

## Data model

A Mentor is an Oqtane user assigned the **"Mentor" role** — there is no separate Mentor database table. The `MentorProfile` table extends the Oqtane user record with 10 Trees-specific fields (VillageId, etc.) using UserId as the foreign key; every user in the Mentor role is expected to have a profile record. Deactivating a mentor means disabling the Oqtane user account; grower assignments are preserved. `Grower.MentorId` currently stores the mentor's **username** (not the numeric UserId); if that ever migrates to UserId, spec and implementation must change together.

## Mentor list

The Mentor management page lists all users in the Mentor role with columns: Name, Username, Email, Assigned Village, Grower Count, Active. The list can be filtered by village (e.g. choosing "Orpen Gate Village" hides mentors assigned to "Londelozzi") and searched by name, where typing "Bondi" narrows the list to mentors whose name contains that text.

## Creating and editing mentors

- **Add Mentor** collects name, email, username, and village (e.g. "Thandi Nkosi", thandi@tentrees.org, username "thandi", "Orpen Gate Village"); saving creates an Oqtane user account, assigns the Mentor role, records the village on the mentor profile, and adds the mentor to the list.
- Email is required ("Email is required") and no account is created until it is supplied; a duplicate username (e.g. a second "bondi") is rejected with an error saying the username is already taken.
- An admin can change a mentor's assigned village; the mentor then sees only growers in the new village.
- **Deactivating** a mentor prevents login but keeps grower assignments intact; the list marks them "Inactive". **Reactivating** restores login plus previous village and grower assignments.

## Cohort assignment

From the cohort edit screen an admin assigns mentors to a cohort or removes them; assigned mentors appear in the cohort's mentor list. See [Cohort Management](/Specifications/Cohort-Management).

## Grower assignment

- Admins assign unassigned growers to a mentor from the mentor's profile; the mentor's grower count and list update (assigning two growers raises the count by two and both appear in the mentor's list).
- A grower can be reassigned from one mentor to another (e.g. from "Bondi" to "Trygive"), moving between their lists.
- When a mentor submits an enrollment, the new grower is auto-assigned to them (`MentorId` = the mentor's username) and appears in their assigned grower list.

## Mentor data isolation

Mentors are scoped to growers via **cohort membership**: a mentor sees all growers belonging to any cohort they are assigned to. A mentor with **no** cohort assignments falls back to seeing all growers in their assigned **village**.

- A mentor assigned to one cohort with 15 members sees exactly those 15 growers and none outside the cohort.
- A mentor assigned to two non-overlapping cohorts (15 + 10, e.g. "Roebuck 1 2026" and "Orpen Gate Village 2024") sees 25.
- Navigating directly to another mentor's grower record is denied with "You are not assigned to this household".
- Village-filtered dropdowns and lists show only the mentor's own village.
- Staff roles (e.g. Educator) see all villages and can filter by any mentor.

## Mentor profile (self-view)

A mentor's own profile shows their name, village, and assigned grower count — with no options to change their village or role.

## Permissions

| Permission | Mentor allowed |
|---|---|
| Submit enrollment form | Yes |
| Submit assessment form | Yes |
| Submit photo release | Yes |
| View assigned growers | Yes |
| View all villages | No |
| View other mentors | No |
| Export data | No |
| Manage users | No |
| Access admin panel | No |

The UI shows/hides features per this table and the API enforces the same restrictions. Non-admins attempting to open the Mentor management page are redirected or shown "Access Denied".

## Related pages

- [Role-Based Data Visibility](/Specifications/Role-Based-Data-Visibility)
- [Village Data Management](/Specifications/Village-Data-Management)
