---
priority: medium
tags: []
---

# Implement Tree Type reference data (hardcoded → CRUD module)

## Feature: Tree Type Reference Data

Provide a managed list of tree species used across multiple modules as a dropdown/lookup.

### Used In
- **Assessment module** — "If any died, which ones?" dropdown (`DeceasedTreeTypes`)
- **Garden Location Mapping module** — recording counts of existing trees by type (Fruit and nut trees, Indigenous trees, etc.)

### Hardcoded List (Phase 1)

Start with a static list defined in a shared constants class. No database table yet.

**Initial tree types:**
```
Avocado
Banana
Citrus (Lemon / Orange)
Fig
Guava
Mango
Moringa
Mulberry
Pawpaw (Papaya)
Peach
Pomegranate
Indigenous (unspecified)
Other
```

#### Phase 1 Checklist
- [ ] Create `Shared/Constants/TreeTypes.cs` with a static list of `(Id, Name)` tuples
- [ ] Wire Assessment `Edit.razor` deceased-trees dropdown to this list
- [ ] Wire Garden Location Mapping tree-type selector to this list
- [ ] Bilingual labels: add en-ZA and ts-ZA entries to relevant resource files

### CRUD Admin Module (Phase 2)

Replace the static list with a database-backed `TreeType` table and an admin CRUD module.

#### Phase 2 Checklist
- [ ] Create `Shared/Models/TreeType.cs` model
- [ ] Add `DbSet` to `TenTreesContext`
- [ ] Create `Server/Repository/TreeTypeRepository.cs`
- [ ] Create `Server/Services/TreeTypeService.cs`
- [ ] Create `Server/Controllers/TreeTypeController.cs`
- [ ] Create `Client/Modules/TreeType/Index.razor` (list + add/edit/delete)
- [ ] Create `Client/Modules/TreeType/ModuleInfo.cs`
- [ ] Create resource files (`Index.resx`, `Index.ts-ZA.resx`)
- [ ] Create `Sql/dbo/Tables/TreeType.sql`
- [ ] Seed initial tree type data in migration script
- [ ] Update Assessment and Garden Mapping modules to fetch from API instead of constants

### Technical Notes
- Phase 1 constants class should use the same shape `{ int Id, string Name }` as the future API response to make Phase 2 a drop-in replacement
- `TreeType` model extends `ModelBase` (Oqtane audit fields)
- Admin-only: only `CentreAdmin` / `Admin` can add/edit/delete tree types
