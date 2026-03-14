---
priority: medium
tags: []
---

# Implement Exit Reason reference data (hardcoded → CRUD module)

## Feature: Exit Reason Reference Data

Provide a managed list of program exit reasons used in the Grower Program Departure workflow.

### Used In
- **GrowerProgramExit** — `ExitReason` dropdown when marking a grower as left the program
- **Grower model** — `ExitReason` is currently a free-text `string` on `ProgramExitRequest`; Phase 2 changes this to an FK

### Spec Reference
`Specs/Features/GrowerProgramExit.feature` — *"Exit reasons are selectable options"*

### Hardcoded List (Phase 1)

Start with a static list defined in a shared constants class. The `ExitReason` field on `Grower` and `ProgramExitRequest` stays as a `string` — the dropdown simply constrains input to these values.

**Predefined reasons (from spec):**
```
Moved away
Deceased
Voluntary withdrawal
Non-compliance
Other
```

#### Phase 1 Checklist
- [ ] Create `Shared/Constants/ExitReasons.cs` with a static string list matching the spec exactly
- [ ] Update `Grower/Status.razor` exit-reason input from free-text to a `<select>` dropdown bound to this list
- [ ] Bilingual labels: add en-ZA and ts-ZA entries to Grower resource files

### CRUD Admin Module (Phase 2)

Replace the static list with a database-backed `ExitReason` table and an admin CRUD module. Also updates the `Grower` model to store an FK instead of a free-text string.

#### Phase 2 Checklist
- [ ] Create `Shared/Models/ExitReason.cs` model
- [ ] Add `DbSet` to `TenTreesContext`
- [ ] Create `Server/Repository/ExitReasonRepository.cs`
- [ ] Create `Server/Services/ExitReasonService.cs`
- [ ] Create `Server/Controllers/ExitReasonController.cs`
- [ ] Create `Client/Modules/ExitReason/Index.razor` (list + add/edit/delete)
- [ ] Create `Client/Modules/ExitReason/ModuleInfo.cs`
- [ ] Create resource files (`Index.resx`, `Index.ts-ZA.resx`)
- [ ] Create `Sql/dbo/Tables/ExitReason.sql`
- [ ] Seed initial 5 exit reasons in migration script
- [ ] Update `Grower` model: replace `string ExitReason` with `int? ExitReasonId` FK
- [ ] Update `ProgramExitRequest` DTO accordingly
- [ ] Update `GrowerRepository` / `GrowerService` / `GrowerController` for FK change
- [ ] Update `Grower/Status.razor` to fetch reasons from API

### Technical Notes
- Phase 1 constants class should use the same shape `{ int Id, string Name }` as the future API response
- `ExitReason` model extends `ModelBase`
- Admin-only: only `CentreAdmin` / `Admin` can add/edit exit reasons
- "Other" reason should optionally allow a free-text notes field (`ExitNotes`) — this field already exists on `Grower`
