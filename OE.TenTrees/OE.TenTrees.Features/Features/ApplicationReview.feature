Feature: Application Review

Scenario: Approve a qualifying application
  Given there is a pending application for "Peter"
  When I navigate to the application review page
  And I review the criteria responses
  And the applicant meets minimum requirements
  And I click "Approve"
  Then the application status should change to "Approved"
  And the beneficiary should be eligible for garden setup

Scenario: Reject a non-qualifying application
  Given there is a pending application with insufficient criteria met
  When I review the application
  And I click "Reject"
  And I enter rejection reason "Does not meet minimum gardening criteria"
  Then the application status should change to "Rejected"
  And the rejection reason should be saved