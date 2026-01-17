@workflow-release @priority-high @mobile
Feature: Photo Release Consent
  As a tree mentor
  I want to capture photo release preferences
  So that the organization has proper consent for using participant photos

  Scenario: Capture release form with full consent
    Given I have an approved enrollment for "Mary Nkuna"
    When I navigate to the release form
    And I select "You may use my photo with my name identified"
    And I capture the signature
    Then the release form should be linked to the enrollment
    And the consent level should be "Full"

  Scenario: Capture release form with limited consent
    Given I have an approved enrollment
    When I select "You may use my picture in group photos without my name"
    And I capture the signature
    Then the consent level should be "Limited"

  Scenario: Capture release form with no consent
    Given I have an approved enrollment
    When I select "You may not use my photo at all"
    And I capture the signature
    Then the consent level should be "None"
