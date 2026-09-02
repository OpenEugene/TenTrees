# Implementation Notes

Implementation notes preserved from the original localization design document (`Specs/Docs/10Trees_Localization.feature.md`, retired). That document's draft Gherkin scenarios were superseded by the six localization feature files, now documented as the sibling pages under [Localization](/Specifications/Localization).

## Supported languages

| Language | ISO 639-1 | ISO 639-2/3 | .NET Culture | Status |
|----------|-----------|-------------|--------------|--------|
| English | en | eng | en-ZA | MVP |
| Xitsonga (Shangaan) | ts | tso | ts-ZA | MVP |
| Sepedi (Northern Sotho) | - | nso | nso-ZA | Future |

## Resource file structure

```
/Resources
  ├── SharedResources.resx           # English (fallback)
  ├── SharedResources.ts-ZA.resx     # Xitsonga
  └── SharedResources.nso-ZA.resx    # Sepedi (future)

/Forms
  ├── ApplicationForm.resx
  ├── ApplicationForm.ts-ZA.resx
  ├── MappingForm.resx
  ├── MappingForm.ts-ZA.resx
  ├── GardenAssessment.resx
  ├── GardenAssessment.ts-ZA.resx
  ├── ReleaseForm.resx
  └── ReleaseForm.ts-ZA.resx
```

## Key translation pairs (sample)

| Key | English (en-ZA) | Xitsonga (ts-ZA) |
|-----|-----------------|------------------|
| Yes | Yes | Ina |
| No | No | Ee |
| Signature | Signature | Musayino |
| Date | Date | Siku |
| Name | Name | Vito |
| Address | Address | Aderese |
| Village | Village | Ndawu |
| Submit | Submit | Rhumela |

## Testing

- **Testers:** Trygive and Quentan (bilingual in English and Xitsonga)
- **Process:** Compare forms side-by-side in both languages to verify accuracy
- **Coverage:** All three main forms (Application, Mapping, Garden Assessment) plus Release Form
