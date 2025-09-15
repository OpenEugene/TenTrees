Feature: Application Submission

Scenario: Submit a complete application
  Given I am logged in as an evaluator
  When I navigate to the new application form
  And I enter the evaluator name "Maria"
  And I enter the beneficiary name "Peter"
  And I enter house number "123"
  And I enter ID or birthdate "1985-05-15"
  And I complete all preferred criteria checkboxes
  And I complete all commitment confirmations
  And I click submit
  Then the application should be saved with status "Pending Review"
  And I should see a confirmation message
  And the application should appear in the applications list

Scenario: Submit application with missing required fields
  Given I am logged in as an evaluator
  When I navigate to the new application form
  And I leave the beneficiary name field empty
  And I click submit
  Then I should see validation errors for required fields
  And the application should not be saved