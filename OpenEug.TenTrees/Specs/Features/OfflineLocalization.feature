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
