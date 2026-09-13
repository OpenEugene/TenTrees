[Home](../../Home.md) / [Specifications](../../Specifications.md) / [Localization](../Localization.md) / Data Entry Language Independence <!-- wikidown:breadcrumb -->

# Data Entry Language Independence

Staff can view submitted data regardless of the language it was submitted in, so all submissions are reviewed consistently. This applies to staff views only; mentors do not review other mentors' submissions.

## Viewing across languages

- A form submitted in Xitsonga viewed by a staff member in English shows the grower name and data values correctly, with dropdown selections displayed in the viewer's language.
- The reverse also holds: an English submission viewed in Xitsonga shows all labels in Xitsonga with the data values intact and the grower name as entered.
- In list views with mixed-language submissions (e.g. the enrollment list), all records appear; grower names display as entered while column headers follow the viewer's current language.

## Language-neutral export

Excel exports of mixed-language data use English column headers, keep grower names as entered, and normalise Yes/No values to "Yes" and "No" regardless of submission language.

## Reporting and search

- Reports (e.g. Tree Survival Rate) include all submissions and aggregate values correctly across languages, with labels in the viewer's language.
- Search works across submission languages: a grower enrolled via the Xitsonga interface (e.g. "Mary Nkuna") is found by an English-interface search for that name, with all her data shown correctly.

## Data integrity

Stored values translate on display, not in storage. Example: a Xitsonga submission for grower "Mary Nkuna" in village "Orpen Gate" with "Owns Home = Ina" displays as "Yes" to an English viewer, with the grower name, village, and all other data preserved exactly.

## Related pages

- [Offline Localization](/Specifications/Localization/Offline-Localization) — the neutral storage format for offline sync.
- [Program Reporting](/Specifications/Program-Reporting)
