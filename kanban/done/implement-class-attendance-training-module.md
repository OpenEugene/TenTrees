---
priority: high
tags: [training, attendance, workflow-attendance]
---

# Implement Class Attendance & Training Module

Training class management, batch attendance marking, per-grower eligibility tracking,
and village-level training status summary.

## Spec Reference

`Specs/Features/ClassAttendance.feature`

## Checklist

### Training Class Management (`Edit.razor`, `Index.razor`)
- [x] Create training class (village, class name, class date, class number 1–5)
- [x] Edit and delete training classes
- [x] List training classes filtered by village

### Attendance Marking (`Attendance.razor`)
- [x] Display all growers in a village for a selected training class
- [x] Mark each grower present or absent per class
- [x] Submit all attendance entries in a single `MarkAttendanceRequest` batch
- [x] Individual `ClassAttendance` records saved (one per grower per class)

### Per-Grower Attendance Summary
- [x] `AttendanceSummaryViewModel` returned per grower: `ClassesAttended`, `TotalRequired` (5), `IsEligible`, `StatusDisplay`
- [x] `IsEligible = true` when `ClassesAttended >= 5`
- [x] `StatusDisplay` shows "Eligible for trees" or "N classes remaining"
- [x] `grower-summary/{growerId}` API endpoint for individual lookup

### Village Training Status Summary
- [x] `TrainingStatusSummary` aggregate: Eligible / InProgress / NotStarted / Total
- [x] `status-summary` API endpoint, filterable by `villageId`
- [x] `summaries` API endpoint returns per-grower list, filterable by village

### Access Control
- [x] `ViewModule` policy for reads; `EditModule` policy for marking attendance
- [x] Village-scoped filtering so mentors only see their village's growers

### Localization
- [x] `Index.resx` (en-ZA) for training list
- [x] Attendance and eligibility labels in resource files
