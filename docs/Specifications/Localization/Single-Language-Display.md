# Single Language Display

Content is shown in one language at a time so forms are shorter and easier to read. Translated from the retired feature file `Specs/Features/SingleLanguageDisplay.feature` (high priority, mobile).

The rule is absolute: nowhere in the app do bilingual "English / Xitsonga" dual labels appear.

- **Forms** — with Xitsonga selected, no English text appears alongside Xitsonga.
- **Switching** — changing language replaces all visible content immediately; no text in the previous language remains.
- **Navigation menu** — menu items are in the selected language only.
- **Error messages** — errors during form submission appear only in the selected language.
- **Buttons** — in Xitsonga: "Hlayisa" (Save), "Teka" (Cancel), "Rhumela" (Submit) — never "Save / Hlayisa"-style bilingual buttons.

## Related pages

- [Language Selection](/Specifications/Localization/Language-Selection)
- [Localized Forms](/Specifications/Localization/Localized-Forms)

## Scenarios (Gherkin)

The original scenarios, preserved verbatim from the retired `Specs/Features/SingleLanguageDisplay.feature` as the precise acceptance criteria for the behaviour described above.

```gherkin
@workflow-localization @priority-high @mobile
Feature: Single Language Display
  As a user
  I want to see content in one language at a time
  So that forms are shorter and easier to read

  Scenario: Forms show only the selected language
    Given my language is set to Xitsonga
    When I view any form
    Then I should NOT see English text alongside Xitsonga
    And I should NOT see "English / Xitsonga" dual labels
    And all text should be in Xitsonga only

  Scenario: Switching language updates all visible content
    Given I am viewing the Mapping Form in English
    When I switch the language to Xitsonga
    Then all English text should be replaced with Xitsonga
    And no English text should remain visible
    And all buttons should display in Xitsonga

  Scenario: Navigation menu in single language
    Given my language is set to Xitsonga
    When I view the navigation menu
    Then menu items should be in Xitsonga only
    And I should NOT see bilingual menu labels

  Scenario: Error messages in single language
    Given my language is set to Xitsonga
    When an error occurs during form submission
    Then the error message should be in Xitsonga only
    And I should NOT see English error text

  Scenario: Button text in single language
    Given my language is set to Xitsonga
    When I view any form
    Then I should see "Hlayisa" for Save button
    And I should see "Teka" for Cancel button
    And I should see "Rhumela" for Submit button
    And I should NOT see "Save / Hlayisa" bilingual buttons
```
