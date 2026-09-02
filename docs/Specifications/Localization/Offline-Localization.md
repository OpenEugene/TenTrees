# Offline Localization

Localization works offline so mentors in areas with poor connectivity can use the app in their language without internet. Translated from the retired feature file `Specs/Features/OfflineLocalization.feature` (high priority, mobile, offline).

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

## Scenarios (Gherkin)

The original scenarios, preserved verbatim from the retired `Specs/Features/OfflineLocalization.feature` as the precise acceptance criteria for the behaviour described above.

```gherkin
@workflow-localization @priority-high @mobile @offline
Feature: Offline Localization
  As a mentor in an area with poor connectivity
  I want localization to work offline
  So that I can use the app in my language without internet

  Scenario: Language resources available offline
    Given I have previously loaded the app with internet
    And my language is set to Xitsonga
    When I lose internet connectivity
    Then the app should still display in Xitsonga
    And all forms should render in Xitsonga
    And all buttons should display in Xitsonga

  Scenario: Switch languages while offline
    Given I am offline
    And I am viewing the app in English
    When I switch to Xitsonga via the language picker
    Then the app should immediately display in Xitsonga
    And I should be able to switch languages without internet

  Scenario: Form submission preserves language context
    Given I am offline
    And I complete a form in Xitsonga
    When I save the form locally
    And I sync the form later at the Centre with internet
    Then the submitted data should be stored correctly
    And the data should be viewable by staff in any language

  Scenario: Offline form data is language-neutral
    Given I have submitted forms offline in Xitsonga
    When I sync the forms to the server
    Then the data values should be stored in a language-neutral format
    And staff viewing in English should see correct data values

  Scenario: Cache language resources on first load
    Given I am online
    When I first open the app
    Then English resources should be downloaded
    And Xitsonga resources should be downloaded
    And resources should be cached for offline use
```
