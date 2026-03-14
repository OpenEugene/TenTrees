---
priority: high
tags: [village, multi-tenant, workflow-village]
---

# Implement Village Management Module

Village CRUD with contact information fields, active/inactive flag, and
multi-tenant data scoping.

## Spec Reference

`Specs/Features/VillageDataManagement.feature`

## Checklist

### Village Model (`Village.cs`)
- [x] `VillageId` (PK)
- [x] `VillageName` (required)
- [x] `ContactName`
- [x] `ContactPhone`
- [x] `ContactEmail`
- [x] `Notes`
- [x] `IsActive` flag

### Back-End (`VillageController`, `VillageService`, `VillageRepository`)
- [x] GET all villages (with optional active filter)
- [x] GET single village by ID
- [x] POST create village
- [x] PUT update village (including contact fields and IsActive)
- [x] DELETE village

### List View (`Index.razor`)
- [x] Show all villages with status indicator
- [x] Add / edit / delete actions (admin only)

### Edit View (`Edit.razor`)
- [x] Village name (required)
- [x] Contact name, phone, email
- [x] Notes free-text
- [x] IsActive toggle

### Data Scoping
- [x] Mentors see only data for their assigned village (`VillageId` on `Grower`)
- [x] Admins see all villages; village filter dropdown on list views
- [x] Village data isolation: mentor from Village B cannot see Village A's growers

### Localization
- [x] `Index.resx` and `Edit.resx` (en-ZA)
