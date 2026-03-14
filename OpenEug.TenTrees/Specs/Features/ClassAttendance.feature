@workflow-attendance @priority-medium
Feature: Permaculture Training Attendance Tracking
  As Centre staff
  I want to track class attendance for growers
  So that we know who is eligible to receive trees

  # ─── TRAINING CLASS MANAGEMENT ──────────────────────────────────────────────

  Scenario: Create a training class for a village
    Given I am Centre staff
    When I create a new training class:
      | Field       | Value              |
      | Village     | Orpen Gate Village |
      | Class Name  | PE Training Session 3 |
      | Class Date  | 2025-11-10         |
      | Class Number| 3                  |
    Then the class should appear in the village's training list
    And it should be available for attendance marking

  # ─── ATTENDANCE MARKING ──────────────────────────────────────────────────────

  Scenario: Mark attendance for multiple growers in a single submission
    Given I am tracking attendance for "PE Training Session 3"
    And the following growers are enrolled in "Orpen Gate Village":
      | Grower         |
      | Mary Nkuna     |
      | Grace Sithole  |
      | Thandi Nkosi   |
    When I mark present/absent for all growers and submit
    Then attendance records should be saved for each grower in a single request
    And each grower's classes-attended count should be updated accordingly

  Scenario: Mark individual grower as present
    Given I am tracking attendance for "PE Training Session 3"
    When I mark "Mary Nkuna" as present
    Then her attendance count should increase to 3
    And her record should show "3 of 5 classes completed"

  Scenario: Mark individual grower as absent
    Given I am tracking attendance for "PE Training Session 3"
    When I mark "Grace Sithole" as absent
    Then her attendance count should not change
    And she should remain at her previous classes-attended count

  # ─── PER-GROWER ATTENDANCE SUMMARY ──────────────────────────────────────────

  Scenario: View per-grower attendance summary
    Given I am viewing grower "Mary Nkuna"
    When I check her attendance record
    Then I should see:
      | Field            | Value                |
      | Classes Attended | 5                    |
      | Total Required   | 5                    |
      | Is Eligible      | Yes                  |
      | Status Display   | Eligible for trees   |

  Scenario: Grower not eligible without required classes
    Given grower "Grace Sithole" has attended 3 of 5 classes
    When I check her eligibility status
    Then status should show "2 classes remaining"
    And "Is Eligible" should be false
    And she should not be marked as "Eligible for trees"

  # ─── VILLAGE TRAINING STATUS SUMMARY ────────────────────────────────────────

  Scenario: View training status summary for a village
    Given I am an admin viewing training for "Orpen Gate Village"
    When I request the training status summary
    Then I should see aggregate counts:
      | Category    | Description                              |
      | Eligible    | Growers who completed all 5 classes      |
      | In Progress | Growers who attended 1–4 classes         |
      | Not Started | Growers who have attended 0 classes      |
      | Total       | All growers in the village               |

  Scenario: Filter attendance summaries by village
    Given I am Centre staff with access to all villages
    When I request attendance summaries for "Londelozzi"
    Then I should only see grower attendance records from "Londelozzi"
    And growers from "Orpen Gate Village" should not appear

  # ─── ELIGIBILITY DISPLAY ────────────────────────────────────────────────────

  Scenario: Eligible growers are clearly identified in the attendance list
    Given the following growers are in "Orpen Gate Village":
      | Grower        | Classes Attended |
      | Mary Nkuna    | 5                |
      | Grace Sithole | 3                |
    When I view the attendance summary list
    Then "Mary Nkuna" should show "Eligible for trees"
    And "Grace Sithole" should show "2 classes remaining"
