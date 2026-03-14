@workflow-exit @priority-medium
Feature: Grower Program Departure
  As Centre staff
  I want to record when growers leave the program
  So that we maintain accurate program statistics

  Scenario: Record program exit with reason
    Given grower "Grace Sithole" is active in the program
    When I navigate to her record
    And I select "Mark as left program"
    And I select reason "Moved away"
    And I enter exit date "2025-11-15"
    Then her status should change to "Exited"
    And exit reason should be recorded as "Moved away"
    And exit date should be recorded

  Scenario: Exit reasons are selectable options
    Given I am recording a program exit
    When I view the exit reason dropdown
    Then I should see predefined reasons:
      | Reason                  |
      | Moved away              |
      | Deceased                |
      | Voluntary withdrawal    |
      | Non-compliance          |
      | Other                   |

  Scenario: Exited grower excluded from active reports
    Given grower "Grace Sithole" has exited the program
    When I generate an "Active Growers" report
    Then "Grace Sithole" should not appear in results
