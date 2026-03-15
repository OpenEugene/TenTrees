---
priority: high
tags: [mentor, admin, security, workflow-admin]
---

# Implement Tree Mentor Management Module

## Overview

Build a Tree Mentor management UI for admins to create, edit, and deactivate
mentor accounts, assign mentors to villages, and assign growers to mentors.
Mentors are standard Oqtane users in the "Tree Mentor" role. The module also
enforces data isolation so mentors only see their own assigned growers and village.

### Spec Reference
`Specs/Features/TreeMentorManagement.feature`

### Related Specs (cross-cutting)
- `Specs/Features/UserAdministration.feature` — role permissions matrix, impersonation
- `Specs/Features/GrowerEnrollment.feature` — mentor auto-assigned at enrollment
- `Specs/Features/GardenAssessment.feature` — mentor isolation in assessments

---

## Module Requirements

- Create `Client/Modules/TreeMentor/` with `Index.razor` (list) and `Edit.razor` (create/edit)
- Create `Server/Controllers/TreeMentorController.cs`
- Create `Server/Repository/ITreeMentorRepository.cs` + `TreeMentorRepository.cs`
- Create `Shared/Models/MentorProfile.cs` (village assignment, grower count)
- No new DB table required for role — use Oqtane's built-in user/role tables
- Add `VillageId` to a `MentorProfile` table (UserId FK, VillageId FK) if not already present from UserAdmin work
- Mentor–grower assignment lives on `Grower.MentorId` (already implemented)

---

## Checklist

### Mentor List (Index.razor)
- [ ] Display all Oqtane users in the "Tree Mentor" role
- [ ] Columns: Name, Username, Email, Assigned Village, Grower Count, Active status
- [ ] Filter by village
- [ ] Search by name
- [ ] "Add Mentor" button → Edit.razor (new)
- [ ] Edit button per row → Edit.razor (edit)
- [ ] Deactivate / Reactivate toggle per row (admin only)
- [ ] Loading indicator with `role="status"`

### Create / Edit Mentor (Edit.razor)
- [ ] Fields: Display Name, Email, Username, Village (dropdown — active only)
- [ ] On create: call Oqtane user creation API, assign "Tree Mentor" role, save MentorProfile
- [ ] On edit: update village assignment and display name
- [ ] Validation: Email required, Username required and unique
- [ ] Per-field `is-invalid` / `aria-invalid` on validation failure (accessibility skill)
- [ ] Save button with spinner state (accessibility skill)
- [ ] Cancel → back to Index

### Grower Assignment (within Edit.razor or sub-panel)
- [ ] Show growers currently assigned to this mentor
- [ ] Allow admin to assign unassigned growers from the same village
- [ ] Allow admin to reassign a grower to a different mentor (updates `Grower.MentorId`)
- [ ] Grower count updates immediately after assignment change

### Data Isolation (server-enforced)
- [ ] All grower list and detail endpoints filter by `Grower.MentorId == User.Identity.Name` (mentor username) for Tree Mentor role
- [ ] Admin and Educator roles bypass the filter (see all)
- [ ] Direct URL access to a grower not assigned to the logged-in mentor (by username in `Grower.MentorId`) returns 403
- [ ] Village-scoped dropdowns return active-only villages filtered to mentor's assigned village

### Mentor Auto-Assignment at Enrollment
- [ ] When a Tree Mentor submits a new enrollment, `Grower.MentorId` is set to their username (e.g., `User.Identity.Name`) automatically
- [ ] Staff submitting on behalf of a mentor can override the MentorId field (mentor username)

### Permissions
- [ ] Module only visible/accessible to users with Admin role
- [ ] Tree Mentor role: enforce read/write isolation in all controllers (growers, assessments, enrollments)
- [ ] API-level role checks match the permissions table in the feature spec

---

## Technical Notes

- Use Oqtane's `IUserService` / `IRoleService` to create users and assign roles — do not bypass the framework
- `MentorProfile` table: `UserId` (string), `VillageId` (int), primary key on `UserId`
- Village dropdown uses `/api/Village/active` endpoint (active villages only)
- Grower assignment list uses existing `Grower.MentorId` column — no schema change needed
- Authorization in controllers: compare `User.Identity.Name` to `Grower.MentorId`; Admin/Educator bypass
- No EF migrations — add `MentorProfile` table via SQL script in `Sql/dbo/tables/`

## Database Changes

- `[dbo].[MentorProfile]` table (if not created by UserAdmin work):
  - `UserId` nvarchar(256) PK (Oqtane user ID)
  - `VillageId` int FK → `Village.VillageId`
  - `CreatedBy`, `CreatedOn`, `ModifiedBy`, `ModifiedOn` (audit fields)

---

## Acceptance Criteria

- [ ] Admin can create a Tree Mentor account and assign a village
- [ ] Admin can edit village assignment and deactivate/reactivate a mentor
- [ ] Admin can assign and reassign growers between mentors
- [ ] Mentor logs in and sees only their assigned growers
- [ ] Mentor cannot access another mentor's grower record (returns access denied)
- [ ] Village dropdowns for mentors show only their assigned village
- [ ] Enrollment auto-assigns the logged-in mentor's ID
- [ ] Build passes with no errors or warnings
