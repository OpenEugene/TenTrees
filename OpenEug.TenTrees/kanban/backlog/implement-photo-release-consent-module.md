---
priority: medium
tags: []
---

# Implement Photo Release Consent Module

## Feature: Photo Release Consent

Implement digital photo release consent capture with three consent levels and signature integration.

### Priority: High
**Tags:** `@workflow-release`, `@priority-high`, `@mobile`

### Module Requirements
- Create `Client/Modules/PhotoRelease/` directory structure
- OR add to Enrollment module as additional action/step
- Create matching Server-side components (Controller, Service, Repository)
- Create `Shared/Models/PhotoRelease.cs` model
- Create bilingual resource files (en-ZA, ts-ZA)
- Reuse signature capture from Enrollment module

### Behavior Checklist

#### ✅ Full Consent Capture
- [ ] Navigate to release form (approved enrollment required)
- [ ] Display consent options
- [ ] Select "You may use my photo with my name identified"
- [ ] Capture signature (reuse signature component)
- [ ] Save release form
- [ ] Link to enrollment (EnrollmentId FK)
- [ ] Set ConsentLevel = "Full"
- [ ] Display confirmation message

#### ✅ Limited Consent Capture
- [ ] Display consent options
- [ ] Select "You may use my picture in group photos without my name"
- [ ] Capture signature
- [ ] Save release form
- [ ] Link to enrollment
- [ ] Set ConsentLevel = "Limited"
- [ ] Display confirmation message

#### ✅ No Consent Capture
- [ ] Display consent options
- [ ] Select "You may not use my photo at all"
- [ ] Capture signature (acknowledgment of choice)
- [ ] Save release form
- [ ] Link to enrollment
- [ ] Set ConsentLevel = "None"
- [ ] Display confirmation message

#### ✅ Integration and Workflow
- [ ] Require approved enrollment before photo release
- [ ] Link photo release to specific enrollment
- [ ] Display photo release status on enrollment record
- [ ] Optional: Make photo release part of enrollment workflow
- [ ] Allow updating consent (new signature required)
- [ ] Store consent history (audit trail)

#### ✅ Data Storage
- [ ] EnrollmentId (foreign key)
- [ ] ConsentLevel (enum: Full, Limited, None)
- [ ] SignatureData (base64 image or similar)
- [ ] SignatureDate (DateTime)
- [ ] CreatedBy, CreatedOn (audit fields)

#### ✅ Reporting
- [ ] Admin can view consent status for all participants
- [ ] Filter participants by consent level
- [ ] Export consent status to Excel/CSV
- [ ] Flag participants missing photo release

### Technical Notes
- Mobile-first design (mentor workflow)
- Signature capture: Reuse from Enrollment/Signature.razor
- Consent text: Full bilingual localization (en-ZA, ts-ZA)
- Consent options: Radio buttons (single selection)
- Required field: ConsentLevel and Signature
- Consider making photo release part of enrollment wizard (optional)

### Consent Text (to be localized)
**English:**
- Full: "You may use my photo with my name identified"
- Limited: "You may use my picture in group photos without my name"
- None: "You may not use my photo at all"

**Xitsonga:**
- (Translations needed from bilingual testers)

### Database Changes
- Create PhotoRelease table:
  - PhotoReleaseId (PK)
  - EnrollmentId (FK)
  - ConsentLevel (string or enum)
  - SignatureData (nvarchar(max) or varbinary)
  - SignatureDate (datetime2)
  - Audit fields (CreatedBy, CreatedOn, ModifiedBy, ModifiedOn)
- Migration script

### Related Feature File
`Specs/Features/PhotoReleaseConsent.feature`
