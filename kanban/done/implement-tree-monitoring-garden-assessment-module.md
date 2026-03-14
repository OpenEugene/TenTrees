---
priority: medium
tags: []
---

# Implement Tree Monitoring & Garden Assessment Module

## Feature: Tree Monitoring and Garden Health Assessment

Implement a recurring assessment module for tracking tree survival rates, garden health, and permaculture practice compliance.

### Priority: High
**Tags:** `@workflow-assessment`, `@priority-high`, `@mobile`, `@recurring`

### Module Requirements
- Create `Client/Modules/Assessment/` directory structure
- Create matching Server-side components (Controller, Service, Repository)
- Create `Shared/Models/Assessment.cs` model
- Create `Shared/Services/IAssessmentService.cs` interface
- Create bilingual resource files (en-ZA, ts-ZA)
- Support for recurring assessments (multiple per beneficiary)
- Offline draft support with sync on reconnect

### Related Spec File
`Specs/Features/GardenAssessment.feature` _(updated 2026-02-26)_

---

### Behavior Checklist

#### ✅ Form Linking
- [ ] Auto-fill grower name and village from approved application on file
- [ ] Do not prompt for house number or ID re-entry when application exists

#### ✅ Tree Survival Tracking
- [ ] Navigate to assessment for specific beneficiary
- [ ] Record number of trees planted
- [ ] Record number of trees still alive
- [ ] Auto-calculate and display survival rate (e.g., "90%")
- [ ] Save assessment with timestamp

#### ✅ Deceased Tree Documentation
- [ ] Prompt "If any died, which ones?" when trees alive < trees planted
- [ ] Option 1: Select tree types from dropdown
- [ ] Option 2: Enter free text description (e.g., "Mango, Avocado")
- [ ] Save deceased tree information

#### ✅ Problem Identification (5 problems)
- [ ] Select problem: "The trees have yellow leaves"
- [ ] Select problem: "The trees have broken branches"
- [ ] Select problem: "The trees are losing their leaves" _(new)_
- [ ] Select problem: "The trees look dry" _(new)_
- [ ] Select problem: "Pests eating the plant"
- [ ] Support multiple problem selection
- [ ] Support zero problems selected (save with no problems + `NeedsHelp = false`)
- [ ] Answer "Do you need someone to help with this problem?" (Yes/No)
- [ ] Set help request flag when Yes selected

#### ✅ Permaculture Practice Questions
- [ ] "Do the trees look healthy?" (Yes/No)
- [ ] "Any chemical fertilizers?" (Yes/No)
- [ ] "Any pesticides used?" (Yes/No)
- [ ] "Are the trees mulched?" (Yes/No)
- [ ] "Are they making compost?" (Yes/No)
- [ ] "Are they collecting water?" (Yes/No)
- [ ] "Any leaky taps visible?" (Yes/No)
- [ ] "Is the garden designed to capture water?" (Yes/No)
- [ ] "Are they using greywater?" (Yes/No)
- [ ] Save all 9 individual boolean responses
- [ ] Compute and store `PermaculturePrinciplesCount` (count of positive practices in use)

#### ✅ Notes
- [ ] Free-text narrative notes field on assessment
- [ ] Notes saved with assessment record
- [ ] Notes visible to centre staff in report view

#### ✅ Save & Sync (Offline Support)
- [ ] "Save Draft" stores partially-completed assessment locally on device
- [ ] Show "Draft saved" confirmation when saved offline
- [ ] Auto-detect Wi-Fi / connectivity
- [ ] On reconnect, allow mentor to open draft and submit
- [ ] Show "Assessment submitted" confirmation after successful sync
- [ ] Show "Your assessment has been saved" on successful online submission

#### ✅ Assessment Frequency Tracking
- [ ] Track participant program year (Year 1 or Year 2)
- [ ] Year 1: Accept assessments every ≥ **14 days** (twice-monthly)
- [ ] Year 2: Accept assessments every ≥ **30 days** (monthly)
- [ ] Record the frequency type on the assessment record
- [ ] Display last assessment date

#### ✅ Access Control
- [ ] Mentor can only assess growers assigned to them — deny with "You are not assigned to this household"
- [ ] CentreAdmin can submit on behalf of a mentor (impersonation); record shows mentor + admin entry flag
- [ ] Educator (centre staff) role can view but not edit a submitted assessment

#### ✅ Data Persistence
- [ ] Save assessment with timestamp
- [ ] Link to beneficiary/enrollment (auto-fill from approved application)
- [ ] Store all responses
- [ ] Enable historical assessment viewing

---

### Technical Notes
- Follow Oqtane module patterns (Repository → Service → Controller → Blazor client)
- `Assessment` model extends `ModelBase` (Oqtane audit fields)
- `PermaculturePrinciplesCount` computed at save time and stored for reporting
- 5 tree problem flags as `BIT` columns (no join table needed)
- Frequency guard via `CanSubmitAssessmentAsync()` — Year 1 ≥ 14 days, Year 2 ≥ 30 days
- Offline draft: store in browser `localStorage` via JS interop; submit on reconnect
- Mentor assignment check: compare `Grower.MentorId` to current user id; CentreAdmin bypasses with impersonation flag
- Multi-step form with progress indicators
- Mobile-first design, checkboxes preferred over text entry
- Bilingual localization (en-ZA, ts-ZA)
- Uses `GrowerService.GetActiveGrowersAsync()` from Grower module (infrastructure from PR #29)

### Files to Create / Modify (15 total)

| Layer | File | Action |
|---|---|---|
| Shared | `Shared/Models/Assessment.cs` | Create |
| Shared | `Shared/Services/IAssessmentService.cs` | Create |
| Server | `Server/Repository/AssessmentRepository.cs` | Create |
| Server | `Server/Services/AssessmentService.cs` | Create |
| Server | `Server/Controllers/AssessmentController.cs` | Create |
| Server | `Server/Repository/TenTreesContext.cs` | Modify — add `DbSet` |
| Client | `Client/Modules/Assessment/ModuleInfo.cs` | Create |
| Client | `Client/Modules/Assessment/Index.razor` | Create |
| Client | `Client/Modules/Assessment/Edit.razor` | Create |
| Resources | `Client/Resources/…Assessment/Index.resx` | Create |
| Resources | `Client/Resources/…Assessment/Index.ts-ZA.resx` | Create |
| Resources | `Client/Resources/…Assessment/Edit.resx` | Create |
| Resources | `Client/Resources/…Assessment/Edit.ts-ZA.resx` | Create |
| SQL | `Sql/dbo/Tables/Assessment.sql` | Create |
| SQL | `Sql/Sql.sqlproj` | Modify — include new table |
