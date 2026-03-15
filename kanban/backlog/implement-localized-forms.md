---
priority: high
tags: [localization, mobile]
---

# Implement Localized Forms

## Feature: Localized Forms

All forms must display labels, placeholders, validation messages, and success messages in the user's selected language.

**Tags:** `@workflow-localization`, `@priority-high`, `@mobile`

## What's Missing

No kanban card existed for this feature. The `.resx` translation infrastructure is in place, but the completeness of Xitsonga translations across all forms has not been verified, and known Xitsonga keys (listed below) need to be confirmed in every form.

## Behavior Checklist

#### Enrollment / Application Form
- [ ] Field labels display in Xitsonga (e.g. "Vito ra Mulimi" for "Grower Name", "Ndawu" for "Village")
- [ ] Field placeholders in Xitsonga
- [ ] Validation: "Vito ra mulimi ri laveka" for "Grower name is required"
- [ ] Validation: "Ndawu yi laveka" for "Village is required"
- [ ] Success: "Ngheniso yi hlayisiwe hi ku humelela" for "Enrollment saved successfully"

#### Mapping Form
- [ ] Form title: "Swa mepe"
- [ ] GPS label: "Xiyimo xa le"
- [ ] Yes/No: "Ina" / "Ee"

#### Garden Assessment Form
- [ ] Form title: "Swilo swa muako"
- [ ] All tree type options translated
- [ ] All problem checkboxes translated

#### Release / Consent Form
- [ ] Consent text in Xitsonga
- [ ] "Musayino" for Signature
- [ ] "Siku" for Date

#### Buttons (all forms)
- [ ] Save → "Hlayisa"
- [ ] Cancel → "Teka"
- [ ] Submit → "Rhumela"

#### English baseline
- [ ] Enrollment form shows "Grower Name", "Village", "House Number" in English mode

## Related Feature Files

- `Specs/Features/LocalizedForms.feature`
- `Specs/Features/SingleLanguageDisplay.feature`
