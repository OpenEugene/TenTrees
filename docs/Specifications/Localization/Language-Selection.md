# Language Selection

Mentors and staff select a preferred language so they can use the app in a language they understand. Translated from the retired feature file `Specs/Features/LanguageSelection.feature` (high priority, mobile).

## Supported languages

| Language | Culture code |
|---|---|
| English | en-ZA |
| Xitsonga (Shangaan) | ts-ZA |

## Auto-detection

On first open, the app matches the device language: a device set to `ts-ZA` displays in Xitsonga; an unsupported device language (e.g. `fr-FR`) falls back to English.

## Manual switching

A language picker lets the user switch at any time. Selecting Xitsonga immediately renders all form labels and button text in Xitsonga. The switch is instant on the current screen — e.g. viewing the Enrollment Form and switching to `ts-ZA` updates all visible text, changes the form title from "Enrollment" to "Ngheniso", and leaves no English visible.

## Persistence

The chosen language persists across sessions: closing and reopening the app keeps displaying the saved preference.

## Related pages

- [Localized Forms](/Specifications/Localization/Localized-Forms)
- [Single Language Display](/Specifications/Localization/Single-Language-Display)

## Scenarios (Gherkin)

The original scenarios, preserved verbatim from the retired `Specs/Features/LanguageSelection.feature` as the precise acceptance criteria for the behaviour described above.

```gherkin
@workflow-localization @priority-high @mobile
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

  Scenario: Supported languages list
    When I view the language picker
    Then I should see the following languages available:
      | Language            | Culture Code |
      | English             | en-ZA        |
      | Xitsonga (Shangaan) | ts-ZA        |

  Scenario: Language selection updates immediately
    Given I am viewing the Enrollment Form in English
    When I switch the language to "ts-ZA"
    Then all visible text should update to Xitsonga
    And the form title should change from "Enrollment" to "Ngheniso"
    And I should not see any English text
```
