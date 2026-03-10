---
priority: medium
tags: []
---

# Implement User Administration & Role Management

## Feature: User and Access Management

Implement comprehensive user administration including role management, village assignments, household assignments, and impersonation for data entry.

### Priority: High
**Tags:** `@workflow-admin`, `@priority-high`, `@security`

### Module Requirements
- Extend Oqtane user management
- Create `Client/Modules/UserAdmin/` directory structure (optional custom UI)
- Create Server-side authorization middleware
- Create audit logging for impersonation
- Create bilingual resource files (en-ZA, ts-ZA)

### Behavior Checklist

#### ✅ Admin Impersonation for Data Entry
- [ ] Admin "Becky" can select "Enter data as [Mentor]"
- [ ] Select mentor "Bondi" from dropdown
- [ ] Complete form (e.g., assessment) on behalf of mentor
- [ ] Assessment recorded with CreatedBy = "Bondi"
- [ ] Audit log records:
  - Action: "Assessment Created"
  - Entered By: "Becky"
  - On Behalf Of: "Bondi"
  - Timestamp: [current time]
- [ ] Exit impersonation mode
- [ ] Audit all actions during impersonation

#### ✅ Assign Mentor to Village
- [ ] Navigate to user "Bondi"
- [ ] Assign to village "Orpen Gate Village"
- [ ] User can only see data for assigned village
- [ ] Village assignment recorded in profile
- [ ] Cannot change own village (admin only)

#### ✅ Assign Mentor to Specific Households
- [ ] Mentor "Bondi" assigned to "Orpen Gate Village"
- [ ] Assign households 1-10 to "Bondi"
- [ ] Bondi sees those 10 households in her list
- [ ] Other mentors in same village do NOT see those households
- [ ] Household assignments recorded
- [ ] Allow reassignment between mentors

#### ✅ Create New Mentor Account
- [ ] Navigate to user creation interface
- [ ] Enter user details:
  - Name (e.g., "Thandi Nkosi")
  - Role: "Mentor"
  - Village: "Orpen Gate Village"
  - Email: "thandi@example.com"
- [ ] Generate login credentials
- [ ] Send welcome email (optional)
- [ ] Account active immediately

#### ✅ Role Permissions Matrix
Implement and enforce the following permissions:

| Role | Can Submit Forms | Can View All Villages | Can Export | Can Manage Users |
|------|-----------------|---------------------|-----------|-----------------|
| Mentor | ✅ Yes | ❌ No | ❌ No | ❌ No |
| Educator | ✅ Yes | ✅ Yes | ✅ Yes | ❌ No |
| Project Manager | ✅ Yes | ✅ Yes | ✅ Yes | ❌ No |
| Admin | ✅ Yes | ✅ Yes | ✅ Yes | ✅ Yes |
| Executive Director | ✅ Yes | ✅ Yes | ✅ Yes | ✅ Yes |

- [ ] Implement role checks in UI (hide/show features)
- [ ] Implement role checks in API (authorize attributes)
- [ ] Test each role's access restrictions

#### ✅ User List Management
- [ ] View all users (Admin only)
- [ ] Filter by role
- [ ] Filter by village
- [ ] Search by name
- [ ] Edit user details
- [ ] Deactivate/reactivate user
- [ ] Reset password
- [ ] View audit log for user

### Technical Notes
- Leverage Oqtane's built-in user management where possible
- Custom roles: Create in database seeding
- Claims-based authorization: Add VillageId and HouseholdIds claims
- Impersonation: Store in session, clear on logout
- Audit logging: Create AuditLog table and service
- Bilingual localization (en-ZA, ts-ZA)

### Database Changes
- Add VillageId to UserProfile
- Add HouseholdAssignments table (UserId, HouseholdId)
- Add AuditLog table (Action, UserId, ImpersonatedUserId, Timestamp, Details)
- Create custom roles in seed data

### Security Considerations
- Impersonation requires Admin role
- Log all impersonated actions
- Display impersonation indicator in UI
- Timeout impersonation after period of inactivity
- Require confirmation before entering impersonation mode

### Related Feature File
`Specs/Features/UserAdministration.feature`
