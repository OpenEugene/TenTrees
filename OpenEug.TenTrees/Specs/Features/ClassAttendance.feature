@workflow-attendance @priority-medium
Feature: Permaculture Training Attendance Tracking
  As Centre staff
  I want to track class attendance for participants
  So that we know who is eligible to receive trees

  Scenario: Mark class attendance
    Given I am tracking attendance for "PE Training Session 3"
    When I mark "Mary Nkuna" as present
    Then her attendance count should increase to 3
    And her record should show "3 of 5 classes completed"

  Scenario: View attendance completion status
    Given I am viewing participant "Mary Nkuna"
    When I check her attendance record
    Then I should see classes attended: "5 of 5"
    And status should show "Eligible for trees"

  Scenario: Participant not eligible without required classes
    Given participant "Grace Sithole" has attended 3 of 5 classes
    When I check her eligibility status
    Then status should show "2 classes remaining"
    And she should not be marked as "Eligible for trees"
