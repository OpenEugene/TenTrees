---
priority: high
tags: [localization, mobile]
---

# Implement Single Language Display

## Feature: Single Language Display

Forms and navigation must display in exactly one language at a time — never showing dual-language ("English / Xitsonga") labels side by side.

**Tags:** `@workflow-localization`, `@priority-high`, `@mobile`

## What's Missing

No kanban card existed for this feature. This requirement ensures the UI avoids a common bilingual-app anti-pattern where both languages are shown simultaneously, which would make forms longer and harder to read for field mentors.

## Behavior Checklist

- [ ] Forms show only the selected language — no bilingual labels visible
- [ ] Switching language replaces all visible text; no English text remains after switching to Xitsonga
- [ ] Navigation menu items display in selected language only
- [ ] Error messages display in selected language only
- [ ] Button text displays in selected language only (no "Save / Hlayisa" style labels)
- [ ] Language switch takes effect on all currently visible content without requiring page reload

## Related Feature File

`Specs/Features/SingleLanguageDisplay.feature`
