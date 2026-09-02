# Localization

The 10 Trees app supports English (`en-ZA`) and Xitsonga/Shangaan (`ts-ZA`) throughout the interface, with Sepedi (`nso-ZA`) planned for the future. These pages were translated from the six localization feature files formerly in `Specs/Features/` (retired; in git history). See also the [Implementation Notes](/Specifications/Localization/Implementation-Notes) preserved from the original design document.

The guiding principles:

- **One language at a time** — users pick a language and see the whole app in it, never bilingual labels.
- **Language-neutral data** — what a user submits is stored independently of the language they submitted it in, so staff can review, search, export, and report on everything regardless of language.
- **Offline-first** — language resources are cached so localization works without connectivity in the field.

## Pages

- [Language Selection](/Specifications/Localization/Language-Selection) — device auto-detect, manual picker, persistence.
- [Localized Forms](/Specifications/Localization/Localized-Forms) — every form fully rendered in the selected language, including validation and success messages.
- [Single Language Display](/Specifications/Localization/Single-Language-Display) — no dual-language labels anywhere.
- [Offline Localization](/Specifications/Localization/Offline-Localization) — cached resources and language-neutral offline submissions.
- [Data Entry Language Independence](/Specifications/Localization/Data-Entry-Language-Independence) — staff view, search, export, and report across submission languages.
- [Staff Language Management](/Specifications/Localization/Staff-Language-Management) — managing translations without code changes.
