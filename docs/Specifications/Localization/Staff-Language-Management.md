# Staff Language Management

Admins and project managers manage translations so language content can be updated without code changes. Translated from the retired feature file `Specs/Features/StaffLanguageManagement.feature` (medium priority, staff only).

## Language settings

The Language Settings page lists supported languages with status and translation completion:

| Language | Culture code | Status | Completion |
|---|---|---|---|
| English | en-ZA | Active | 100% |
| Xitsonga (Shangaan) | ts-ZA | Active | 100% |
| Sepedi | nso-ZA | Future | 0% |

English always shows 100%; other languages show their actual completion percentage.

## Translation editor

- When a new form field is added (e.g. "Do you need help?"), the editor shows the English text and lets staff enter the Xitsonga translation (e.g. "Xana u lava ku pfuniwa?").
- Existing translations can be updated; mentors see the new text after saving.
- Updates can be **previewed** before publishing — forms render with the new translations, switchable between languages in the preview.

## Export and import for review

Translations export to an Excel file with columns: Key (resource key identifier), English, Xitsonga, Form (where used), and Last Modified. A reviewed file can be imported back: translations are validated, valid ones are imported, and a summary of changes is shown.

## Related pages

- [Localized Forms](/Specifications/Localization/Localized-Forms) — where these translations appear.

## Scenarios (Gherkin)

The original scenarios, preserved verbatim from the retired `Specs/Features/StaffLanguageManagement.feature` as the precise acceptance criteria for the behaviour described above.

```gherkin
@workflow-localization @priority-medium @staff-only
Feature: Staff Language Management
  As an admin or project manager
  I want to manage translations
  So that I can update language content without code changes

  Background:
    Given I am logged in as an admin

  Scenario: View available translations
    When I navigate to Language Settings
    Then I should see a list of supported languages:
      | Language            | Culture Code | Status    | Completion |
      | English             | en-ZA        | Active    | 100%       |
      | Xitsonga (Shangaan) | ts-ZA        | Active    | 100%       |
      | Sepedi              | nso-ZA       | Future    | 0%         |

  Scenario: View translation completion percentage
    When I navigate to Language Settings
    Then I should see the translation completion percentage for each language
    And English should show 100% complete
    And Xitsonga should show the actual completion percentage

  Scenario: Add translation for new form field
    Given a new field "Do you need help?" has been added
    When I navigate to the translation editor
    Then I should see the English text "Do you need help?"
    And I should be able to enter the Xitsonga translation
    And I should enter "Xana u lava ku pfuniwa?"
    And the translation should be saved

  Scenario: Update existing translation
    Given the field "Grower Name" exists
    When I navigate to the translation editor
    And I update the Xitsonga translation to "Vito ra Munhu Loyi Pfuniwaka"
    Then the new translation should be saved
    And mentors should see the updated translation

  Scenario: Preview translations before publishing
    Given I have made translation updates
    When I preview the changes
    Then I should see forms rendered with the new translations
    And I should be able to switch between languages in the preview

  Scenario: Export translations for review
    When I export translations
    Then I should receive an Excel file with:
      | Column         | Content                      |
      | Key            | Resource key identifier      |
      | English        | English text                 |
      | Xitsonga       | Xitsonga translation         |
      | Form           | Form name where used         |
      | Last Modified  | Date of last change          |

  Scenario: Import reviewed translations
    Given I have an Excel file with reviewed translations
    When I import the translation file
    Then new translations should be validated
    And valid translations should be imported
    And I should see a summary of imported changes
```
