---
priority: medium
tags: []
---

# Implement Problem Type reference data (hardcoded → CRUD module)

## Feature: Problem Type Reference Data

Provide a managed list of observable tree problems used in the Garden Assessment module.

### Used In
- **Assessment module** — multi-select checkboxes: "Which problems do you observe?"

### Spec Reference
`Specs/Features/GardenAssessment.feature` — *"Record multiple problems"*

### Hardcoded List (Phase 1)

Store the 5 problems as **boolean flag columns** on the `Assessment` model. The checkboxes are rendered from a static list in a shared constants class.

**Problems (from spec):**
```
The trees have broken branches   → HasBrokenBranches  (BIT)
The trees have yellow leaves     → HasYellowLeaves    (BIT)
The trees are losing their leaves→ HasLosingLeaves    (BIT)
The trees look dry               → HasLookDry         (BIT)
Pests eating the plant           → HasPests           (BIT)
```

#### Phase 1 Checklist
- [ ] Create `Shared/Constants/ProblemTypes.cs` — static list mapping display label to `Assessment` property name
- [ ] Add the 5 boolean fields to `Shared/Models/Assessment.cs`
- [ ] Add the 5 `BIT` columns to `Sql/dbo/Tables/Assessment.sql`
- [ ] Wire `Assessment/Edit.razor` Step 3 checkboxes to these fields
- [ ] Bilingual labels: add en-ZA and ts-ZA entries to Assessment `Edit.resx`

### CRUD Admin Module (Phase 2)

Replace boolean columns with a `ProblemType` reference table and an `AssessmentProblem` junction table. This allows adding new problem types without a schema migration.

#### Phase 2 Checklist
- [ ] Create `Shared/Models/ProblemType.cs`
- [ ] Create `Shared/Models/AssessmentProblem.cs` (junction: `AssessmentId`, `ProblemTypeId`)
- [ ] Add `DbSet` and `DbSet` to `TenTreesContext`
- [ ] Create `Server/Repository/ProblemTypeRepository.cs`
- [ ] Create `Server/Services/ProblemTypeService.cs`
- [ ] Create `Server/Controllers/ProblemTypeController.cs`
- [ ] Create `Client/Modules/ProblemType/Index.razor` (list + add/edit/delete)
- [ ] Create `Client/Modules/ProblemType/ModuleInfo.cs`
- [ ] Create resource files (`Index.resx`, `Index.ts-ZA.resx`)
- [ ] Create `Sql/dbo/Tables/ProblemType.sql`
- [ ] Create `Sql/dbo/Tables/AssessmentProblem.sql`
- [ ] Seed initial 5 problem types in migration script
- [ ] Remove the 5 boolean columns from `Assessment` — data migration required
- [ ] Update `AssessmentRepository`, `AssessmentService`, `AssessmentController`
- [ ] Update `Assessment/Edit.razor` to fetch problems from API

### Technical Notes
- Phase 1 boolean approach is simpler for reporting (direct column queries)
- Phase 2 junction table approach enables adding/removing problem types without a schema change
- `NeedsHelp` flag remains a single boolean on `Assessment` regardless of phase — it applies to the whole set of problems, not individual ones
- Admin-only: only `CentreAdmin` / `Admin` can add/edit problem types
