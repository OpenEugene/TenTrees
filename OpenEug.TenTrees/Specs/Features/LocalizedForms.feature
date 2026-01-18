@workflow-localization @priority-high @mobile
Feature: Localized Forms
  As a mentor
  I want to fill out forms in my preferred language
  So that I understand all questions clearly

  Background:
    Given the app supports English and Xitsonga

  Scenario: Application Form displays in Xitsonga
    Given my language is set to Xitsonga
    When I open the Application Form
    Then I should see form labels in Xitsonga
    And I should see field placeholders in Xitsonga
    And I should see validation messages in Xitsonga
    And I should see "Vito ra Munhu" for "Beneficiary Name"
    And I should see "Ndawu" for "Village"

  Scenario: Application Form displays in English
    Given my language is set to English
    When I open the Application Form
    Then I should see "Beneficiary Name" as a field label
    And I should see "Village" as a field label
    And I should see "House Number" as a field label

  Scenario: Mapping Form displays in Xitsonga
    Given my language is set to Xitsonga
    When I open the Mapping Form
    Then I should see "Swa mepe" as the form title
    And I should see "Ina" for "Yes"
    And I should see "Ee" for "No"
    And I should see "Xiyimo xa le" for "GPS Location"

  Scenario: Garden Assessment displays in Xitsonga
    Given my language is set to Xitsonga
    When I open the Garden Assessment form
    Then all tree type options should be in Xitsonga
    And all problem checkboxes should be in Xitsonga
    And I should see "Swilo swa muako" for "Garden Assessment"

  Scenario: Release Form displays in Xitsonga
    Given my language is set to Xitsonga
    When I open the Release Form
    Then I should see the consent text in Xitsonga
    And I should see "Musayino" for "Signature"
    And I should see "Siku" for "Date"

  Scenario: Yes/No questions display correctly in Xitsonga
    Given my language is set to Xitsonga
    When I view any form with Yes/No questions
    Then I should see "Ina" for Yes options
    And I should see "Ee" for No options

  Scenario: Validation messages in selected language
    Given my language is set to Xitsonga
    When I attempt to submit a form without required fields
    Then I should see "Vito ra munhu ri laveka" for "Beneficiary name is required"
    And I should see "Ndawu yi laveka" for "Village is required"

  Scenario: Success messages in selected language
    Given my language is set to Xitsonga
    When I successfully submit an enrollment form
    Then I should see "Ngheniso yi hlayisiwe hi ku humelela" for "Enrollment saved successfully"
