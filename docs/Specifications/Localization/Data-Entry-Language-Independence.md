# Data Entry Language Independence

Staff can view submitted data regardless of the language it was submitted in, so all submissions are reviewed consistently. Translated from the retired feature file `Specs/Features/DataEntryLanguageIndependence.feature` (high priority, staff only).

## Viewing across languages

- A form submitted in Xitsonga viewed by a staff member in English shows the grower name and data values correctly, with dropdown selections displayed in the viewer's language.
- The reverse also holds: an English submission viewed in Xitsonga shows all labels in Xitsonga with the data values intact and the grower name as entered.
- In list views with mixed-language submissions, all records appear; grower names display as entered while column headers follow the viewer's current language.

## Language-neutral export

Excel exports of mixed-language data use English column headers, keep grower names as entered, and normalise Yes/No values to "Yes" and "No" regardless of submission language.

## Reporting and search

- Reports (e.g. Tree Survival Rate) include all submissions and aggregate values correctly across languages, with labels in the viewer's language.
- Search works across submission languages: a grower enrolled via the Xitsonga interface is found by an English-interface search.

## Data integrity

Stored values translate on display, not in storage. Example: a Xitsonga submission with "Owns Home = Ina" displays as "Yes" to an English viewer, with all other data preserved exactly.

## Related pages

- [Offline Localization](/Specifications/Localization/Offline-Localization) — the neutral storage format for offline sync.
- [Program Reporting](/Specifications/Program-Reporting)

## Scenarios (Gherkin)

The original scenarios, preserved verbatim from the retired `Specs/Features/DataEntryLanguageIndependence.feature` as the precise acceptance criteria for the behaviour described above.

```gherkin
@workflow-localization @priority-high @staff-only
Feature: Data Entry Language Independence
  As a staff member viewing submitted data
  I want to see data regardless of submission language
  So that I can review all submissions consistently

  Scenario: View form submitted in Xitsonga
    Given a mentor submitted a form in Xitsonga
    When I view the submission as a staff member in English
    Then I should see the grower name correctly
    And I should see the data values correctly
    And dropdown selections should display in my preferred language

  Scenario: View form submitted in English
    Given a mentor submitted a form in English
    When I view the submission as a staff member in Xitsonga
    Then I should see all labels in Xitsonga
    And I should see the data values correctly
    And the grower name should display as entered

  Scenario: Mixed language submissions in list view
    Given forms have been submitted in both English and Xitsonga
    When I view the enrollment list
    Then I should see all enrollments
    And grower names should display as entered
    And column headers should be in my current language

  Scenario: Export data is language-neutral
    Given forms have been submitted in multiple languages
    When I export data to Excel
    Then the column headers should be in English
    And the grower names should be as entered
    And Yes/No values should be exported as "Yes" and "No" regardless of submission language

  Scenario: Report generation with mixed language data
    Given enrollments exist in both English and Xitsonga
    When I generate a "Tree Survival Rate" report
    Then the report should include all submissions
    And the report labels should be in my current language
    And data values should be correctly aggregated

  Scenario: Search works across submission languages
    Given "Mary Nkuna" was enrolled using Xitsonga interface
    When I search for "Mary Nkuna" in English interface
    Then I should find the enrollment record
    And I should see all her data correctly

  Scenario: Data integrity across languages
    Given a form was submitted in Xitsonga with:
      | Field          | Value      |
      | Grower         | Mary Nkuna |
      | Village        | Orpen Gate |
      | Owns Home      | Ina        |
    When I view the data in English
    Then "Ina" should display as "Yes"
    And all other data should be preserved correctly
```
