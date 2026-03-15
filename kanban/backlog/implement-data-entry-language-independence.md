---
priority: high
tags: [localization, staff-only]
---

# Implement Data Entry Language Independence

## Feature: Data Entry Language Independence

Data submitted by mentors in Xitsonga must be viewable by staff in English (and vice versa) without loss of information. Reference values (Yes/No, dropdown selections) must be stored as language-neutral keys, not translated strings.

**Tags:** `@workflow-localization`, `@priority-high`, `@staff-only`

## What's Missing

No kanban card existed for this feature. This is a data layer concern: if Boolean or dropdown values are stored as translated strings ("Ina" instead of `true`), staff viewing in English will see untranslated data. All reference data must round-trip correctly across languages.

## Behavior Checklist

#### List and Detail Views
- [ ] Staff viewing in English can see records submitted in Xitsonga with correct values
- [ ] Staff viewing in Xitsonga can see records submitted in English with correct labels
- [ ] Column headers in list views display in the viewer's current language regardless of submission language
- [ ] Grower names (free text) display exactly as entered

#### Boolean / Yes-No Fields
- [ ] "Ina" (Xitsonga Yes) displays as "Yes" when viewed in English
- [ ] Boolean values stored as `true`/`false` in the database, not as translated strings

#### Dropdown / Reference Data
- [ ] Dropdown selections stored as keys (IDs or enum values), not translated display text
- [ ] Selections render in the viewer's current language regardless of submission language

#### Search
- [ ] Searching by grower name finds records regardless of the language used during submission

#### Export
- [ ] Excel/CSV export column headers are in English
- [ ] Yes/No values export as "Yes" / "No" regardless of submission language

#### Reporting
- [ ] Reports include all submissions regardless of submission language
- [ ] Report labels display in the requesting user's current language

## Related Feature File

`Specs/Features/DataEntryLanguageIndependence.feature`
