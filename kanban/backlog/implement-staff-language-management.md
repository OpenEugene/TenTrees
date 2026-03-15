---
priority: medium
tags: [localization, staff-only, admin]
---

# Implement Staff Language Management

## Feature: Staff Language Management

Admins and project managers need an in-app UI to view, add, update, preview, export, and import translations — so language content can be maintained without code changes or developer involvement.

**Tags:** `@workflow-localization`, `@priority-medium`, `@staff-only`

## What's Missing

No kanban card existed for this feature. Currently all translations live in `.resx` files managed by developers. This feature would give non-technical staff the ability to keep translations up to date as forms evolve.

## Behavior Checklist

#### Language Settings Overview
- [ ] Admin can navigate to a Language Settings page
- [ ] Page lists supported languages: English (`en-ZA`) at 100%, Xitsonga (`ts-ZA`) with actual completion %, Sepedi (`nso-ZA`) as Future at 0%
- [ ] Completion percentage is calculated from untranslated key count

#### Translation Editor
- [ ] View all translation keys with English source and Xitsonga target
- [ ] Add Xitsonga translation for a new key (e.g. "Do you need help?" → "Xana u lava ku pfuniwa?")
- [ ] Update an existing Xitsonga translation
- [ ] Changes are saved and immediately visible to mentors

#### Preview
- [ ] Preview forms with pending translation changes before publishing
- [ ] Preview allows switching between languages

#### Export / Import
- [ ] Export translations to Excel with columns: Key, English, Xitsonga, Form, Last Modified
- [ ] Import a reviewed Excel file
- [ ] Import validates entries and reports a summary of changes applied

## Technical Notes

- Oqtane uses `.resx` files for localization; this feature may require a custom admin module or integration with Oqtane's built-in localization admin (if available in 10.x)
- Consider whether import/export writes back to `.resx` files or a database-backed translation store

## Related Feature File

`Specs/Features/StaffLanguageManagement.feature`
