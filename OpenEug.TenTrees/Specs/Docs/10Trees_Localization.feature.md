# 10 Trees Localization Features

## Overview

Localization system for the 10 Trees Digital Tracking Platform supporting multiple South African languages with single-language display (not side-by-side bilingual).

### Supported Languages

| Language | ISO 639-1 | ISO 639-2/3 | .NET Culture | Status |
|----------|-----------|-------------|--------------|--------|
| English | en | eng | en-ZA | MVP |
| Xitsonga (Shangaan) | ts | tso | ts-ZA | MVP |
| Sepedi (Northern Sotho) | - | nso | nso-ZA | Future |

---

## Feature: Language Selection

```gherkin
Feature: Language Selection
  As a mentor or staff member
  I want to select my preferred language
  So that I can use the app in a language I understand

  Background:
    Given the app supports "en-ZA" and "ts-ZA" languages

  Scenario: Auto-detect language from device settings
    Given my device language is set to "ts-ZA"
    When I open the 10 Trees app
    Then the app should display in Xitsonga

  Scenario: Auto-detect falls back to English for unsupported language
    Given my device language is set to "fr-FR"
    When I open the 10 Trees app
    Then the app should display in English

  Scenario: Manually switch language via picker
    Given I am viewing the app in English
    When I tap the language picker
    And I select "Xitsonga"
    Then the app should display in Xitsonga
    And all form labels should be in Xitsonga
    And all button text should be in Xitsonga

  Scenario: Language preference persists across sessions
    Given I have set my language preference to "ts-ZA"
    When I close and reopen the app
    Then the app should display in Xitsonga
```

---

## Feature: Localized Forms

```gherkin
Feature: Localized Forms
  As a mentor
  I want to fill out forms in my preferred language
  So that I understand all questions clearly

  Scenario: Application Form displays in selected language
    Given my language is set to Xitsonga
    When I open the Application Form
    Then I should see form labels in Xitsonga
    And I should see field placeholders in Xitsonga
    And I should see validation messages in Xitsonga

  Scenario: Mapping Form displays in selected language
    Given my language is set to Xitsonga
    When I open the Mapping Form
    Then I should see "Swa mepe" as the form title
    And I should see "Ina" for "Yes"
    And I should see "Ee" for "No"

  Scenario: Garden Assessment displays in selected language
    Given my language is set to Xitsonga
    When I open the Garden Assessment form
    Then all tree type options should be in Xitsonga
    And all problem checkboxes should be in Xitsonga

  Scenario: Release Form displays in selected language
    Given my language is set to Xitsonga
    When I open the Release Form
    Then I should see the consent text in Xitsonga
    And I should see "Musayino" for "Signature"
    And I should see "Siku" for "Date"
```

---

## Feature: Single Language Display

```gherkin
Feature: Single Language Display
  As a user
  I want to see content in one language at a time
  So that forms are shorter and easier to read

  Scenario: Forms show only the selected language
    Given my language is set to Xitsonga
    When I view any form
    Then I should NOT see English text alongside Xitsonga
    And I should NOT see "English / Xitsonga" dual labels

  Scenario: Switching language updates all visible content
    Given I am viewing the Mapping Form in English
    When I switch the language to Xitsonga
    Then all English text should be replaced with Xitsonga
    And no English text should remain visible
```

---

## Feature: Offline Localization

```gherkin
Feature: Offline Localization
  As a mentor in an area with poor connectivity
  I want localization to work offline
  So that I can use the app in my language without internet

  Scenario: Language resources available offline
    Given I have previously loaded the app with internet
    And my language is set to Xitsonga
    When I lose internet connectivity
    Then the app should still display in Xitsonga
    And I should be able to switch languages without internet

  Scenario: Form submission preserves language context
    Given I am offline
    And I complete a form in Xitsonga
    When I sync the form later at the Centre
    Then the submitted data should be stored correctly
    And the data should be viewable by staff in any language
```

---

## Feature: Staff Language Management

```gherkin
Feature: Staff Language Management
  As an admin or project manager
  I want to manage translations
  So that I can update language content without code changes

  Scenario: View available translations
    Given I am logged in as an admin
    When I navigate to Language Settings
    Then I should see a list of supported languages
    And I should see the translation completion percentage for each

  Scenario: Add translation for new form field
    Given a new field "Do you need help?" has been added
    When I navigate to the translation editor
    Then I should see the English text "Do you need help?"
    And I should be able to enter the Xitsonga translation
    And the translation should be "Xana u lava ku pfuniwa?"
```

---

## Feature: Data Entry Language Independence

```gherkin
Feature: Data Entry Language Independence
  As a staff member viewing submitted data
  I want to see data regardless of submission language
  So that I can review all submissions consistently

  Scenario: View form submitted in Xitsonga
    Given a mentor submitted a form in Xitsonga
    When I view the submission as a staff member
    Then I should see the data values correctly
    And dropdown selections should display in my preferred language

  Scenario: Export data is language-neutral
    Given forms have been submitted in multiple languages
    When I export data to Excel
    Then the column headers should be in English
    And the data values should be consistent regardless of submission language
```

---

## Implementation Notes

### Resource File Structure

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

### Key Translation Pairs (Sample)

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

### Testing

- **Testers**: Trygive and Quentan (bilingual in English and Xitsonga)
- **Process**: Compare forms side-by-side in both languages to verify accuracy
- **Coverage**: All three main forms (Application, Mapping, Garden Assessment) plus Release Form
