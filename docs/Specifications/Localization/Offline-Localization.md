[Home](../../Home.md) / [Specifications](../../Specifications.md) / [Localization](../Localization.md) / Offline Localization <!-- wikidown:breadcrumb -->

# Offline Localization

Localization works offline so mentors in areas with poor connectivity can use the app in their language without internet.

## Cached resources

On first load with internet, both English and Xitsonga language resources are downloaded and cached for offline use. After that:

- Losing connectivity does not affect the display language — forms and buttons keep rendering in the selected language.
- The user can **switch languages while offline** via the picker, with the change applied immediately.

## Language-neutral offline submissions

A form completed offline (e.g. in Xitsonga) is saved locally and synced later at the Centre. On sync:

- Data values are stored in a **language-neutral format**.
- The submission is viewable by staff in any language — a staff member in English sees the correct values.

## Related pages

- [Garden Assessment](/Specifications/Garden-Assessment) — draft save and sync workflow.
- [Data Entry Language Independence](/Specifications/Localization/Data-Entry-Language-Independence)
