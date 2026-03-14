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
