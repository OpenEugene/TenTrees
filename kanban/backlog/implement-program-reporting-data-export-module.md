---
priority: medium
tags: []
---

# Implement Program Reporting & Data Export Module

## Feature: Program Reports and Data Export

Implement comprehensive reporting and data export functionality for program tracking and funder reporting.

### Priority: High
**Tags:** `@workflow-reporting`, `@priority-high`, `@staff-only`

### Module Requirements
- Create `Client/Modules/Reports/` directory structure
- Create matching Server-side components (Controller, Service)
- Create report generation services
- Create Excel/CSV export services
- Create bilingual resource files (en-ZA, ts-ZA)

### Behavior Checklist

#### ✅ Tree Survival Report
- [ ] Select "Tree Survival Rate" report type
- [ ] Filter by village (e.g., "Orpen Gate Village")
- [ ] Set date range (from/to dates)
- [ ] Display: Total trees planted
- [ ] Display: Total trees alive
- [ ] Display: Survival rate percentage (e.g., "91%")
- [ ] Show trend over time (optional enhancement)

#### ✅ Permaculture Compliance Report
- [ ] Select "Permaculture Practices" report type
- [ ] Filter by village
- [ ] Display percentage for each practice:
  - Making compost
  - Collecting water
  - Using greywater
  - No chemical fertilizers
  - No pesticides
- [ ] Identify "Areas needing improvement"
- [ ] Show practices with <80% compliance

#### ✅ Excel Export
- [ ] Filter data by village
- [ ] Filter data by date range (e.g., "Last 30 days")
- [ ] Click "Export to Excel" button
- [ ] Generate .xlsx file
- [ ] Include all filtered records
- [ ] Match columns to data grid display
- [ ] Trigger download

#### ✅ CSV Export
- [ ] Filter assessment data
- [ ] Click "Export to CSV" button
- [ ] Generate .csv file
- [ ] Excel-compatible format (UTF-8 with BOM)
- [ ] Include headers
- [ ] Trigger download

#### ✅ Monthly Village Report
- [ ] Select month/year (e.g., "November 2025")
- [ ] Select village (e.g., "Orpen Gate Village")
- [ ] Include section: Tree Survival (rate and trend)
- [ ] Include section: New Enrollments (count this month)
- [ ] Include section: Active Assessments (count completed)
- [ ] Include section: Permaculture Compliance (practice percentages)
- [ ] Include section: Areas for Improvement (identified gaps)
- [ ] Export as PDF or print-friendly format

#### ✅ Access Control
- [ ] Restrict to Centre staff roles:
  - Admin
  - Project Manager
  - Executive Director
  - Educator
- [ ] Mentors cannot access reports
- [ ] Admin can view all villages
- [ ] Apply role-based filters

### Technical Notes
- Desktop-optimized interface (not mobile workflow)
- Use charting library for visual reports (optional)
- Excel export: Use EPPlus or similar library
- CSV export: Proper UTF-8 encoding with BOM
- Date range picker component
- Village filter dropdown
- Implement server-side report generation for performance
- Bilingual headers in exports

### Related Feature File
`Specs/Features/ProgramReporting.feature`
