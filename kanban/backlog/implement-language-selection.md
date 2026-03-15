---
priority: high
tags: [localization, mobile]
---

# Implement Language Selection

## Feature: Language Selection

Allow mentors and staff to use the app in their preferred language (English or Xitsonga).

**Tags:** `@workflow-localization`, `@priority-high`, `@mobile`

## What's Missing

No kanban card existed for this feature. The app ships with `.resx` files for `en-ZA` and `ts-ZA`, but the language picker UI, auto-detection, and persistence behaviour have not been verified against the feature scenarios.

## Behavior Checklist

- [ ] Auto-detect language from device/browser settings (`ts-ZA` → Xitsonga, `en-ZA` → English)
- [ ] Fall back to English for unsupported device languages (e.g. `fr-FR`)
- [ ] Language picker control visible and accessible on mobile
- [ ] Selecting a language switches all visible text immediately
- [ ] Language preference persisted across sessions (local storage or user profile)
- [ ] Picker shows exactly two options: English (`en-ZA`) and Xitsonga / Shangaan (`ts-ZA`)

## Related Feature File

`Specs/Features/LanguageSelection.feature`
