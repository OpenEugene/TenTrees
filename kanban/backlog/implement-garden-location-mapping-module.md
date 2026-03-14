---
priority: medium
tags: []
---

# Implement Garden Location Mapping Module

## Feature: Garden Location and Resource Documentation

Implement a mobile-first module for documenting garden locations and existing resources for enrolled beneficiaries.

### Priority: High
**Tags:** `@workflow-mapping`, `@priority-high`, `@mobile`, `@gps`

### Module Requirements
- Create `Client/Modules/Mapping/` directory structure
- Create matching Server-side components (Controller, Service, Repository)
- Create `Shared/Models/Mapping.cs` model
- Create bilingual resource files (en-ZA, ts-ZA)

### Behavior Checklist

#### ✅ GPS Capture and Water Resources
- [ ] Complete garden mapping with GPS coordinates
- [ ] Beneficiary information auto-fills from enrollment
- [ ] Capture GPS coordinates via device
- [ ] Record water availability (water in plot)
- [ ] Record water catchment system (Jojo tank)
- [ ] Link mapping to existing enrollment

#### ✅ Existing Tree Inventory
- [ ] Record count of existing trees/productive plants
- [ ] Record count of indigenous trees
- [ ] Record count of fruit and nut trees
- [ ] Answer "Is there space for more trees?"
- [ ] Answer "Is the property fenced?"
- [ ] Answer "Are there resources like compost or mulch?"

#### ✅ Manual GPS Entry (Staff Feature)
- [ ] Allow staff to manually enter latitude
- [ ] Allow staff to manually enter longitude
- [ ] Update existing mapping records with GPS

#### ✅ Enrollment Search and Linking
- [ ] Search for existing enrollments by name
- [ ] Display search results
- [ ] Auto-fill beneficiary name from selected enrollment
- [ ] Auto-fill house number from selected enrollment
- [ ] Auto-fill village from selected enrollment

#### ✅ Data Persistence
- [ ] Save mapping data to database
- [ ] Link to EnrollmentId
- [ ] Display confirmation message
- [ ] Validation for required fields

### Technical Notes
- Follow Oqtane module patterns (ModuleBase, IModule)
- Use EditUrl() for navigation between actions
- Mobile-first design with Bootstrap cards
- Touch-friendly UI (44x44px minimum touch targets)
- Implement bilingual localization

### Related Feature File
`Specs/Features/GardenLocationMapping.feature`
