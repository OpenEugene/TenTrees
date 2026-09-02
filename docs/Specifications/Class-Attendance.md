# Class Attendance

Centre staff track permaculture training class attendance for growers so the program knows who is eligible to receive trees. Translated from the retired feature file `Specs/Features/ClassAttendance.feature` (medium priority).

## Standard class sequence

The standard permaculture education sequence is five classes:

1. Tree Selection
2. Tree Planting
3. Water Management
4. Soil Fertility
5. General Permaculture Practices

Class sequencing can vary by cohort. For the **Roebuck 2026 cohort**, classes 1 and 2 (tree selection and planting) are front-loaded before winter so trees can go in the ground within two weeks; classes 3–5 are delivered after trees are planted. Regardless of order, growers need **5 of 5** classes to be eligible for trees.

## Recording attendance

Marking a grower present at a class increments their attendance count and records which specific class they attended (e.g. "Tree Selection"). A grower's record shows progress as "N of 5 classes completed".

## Eligibility

- A grower with 5 of 5 classes shows status **"Eligible for trees"**.
- A grower with fewer shows the remaining count (e.g. "2 classes remaining") and is not marked eligible.
- A cohort grower who has completed only the front-loaded classes shows "2 of 5 classes completed", is not yet eligible, and a note lists the remaining classes.

## Class–cohort association

Training classes can be linked to cohorts:

- From the training edit screen, an admin selects a cohort from the add dropdown; the cohort appears as a tag on the class and the link is also visible from the cohort edit screen.
- The training sessions list shows each class's linked cohorts as badges on its row.
- Removing a cohort tag from a class requires an inline confirmation.

Full cohort lifecycle and tag behaviour is documented in [Cohort Management](/Specifications/Cohort-Management).

## Scenarios (Gherkin)

The original scenarios, preserved verbatim from the retired `Specs/Features/ClassAttendance.feature` as the precise acceptance criteria for the behaviour described above.

```gherkin
@workflow-attendance @priority-medium
Feature: Permaculture Training Attendance Tracking
  As Centre staff
  I want to track class attendance for growers
  So that we know who is eligible to receive trees

  # Standard class sequence (5 classes):
  #   1. Tree Selection
  #   2. Tree Planting
  #   3. Water Management
  #   4. Soil Fertility
  #   5. General Permaculture Practices
  #
  # NOTE (Roebuck 2026 cohort): Classes 1 and 2 (tree selection and planting)
  # are front-loaded before winter so trees can go in the ground within two weeks.
  # Classes 3–5 are delivered after trees are planted.
  # Class sequencing can vary by cohort; growers need 5 of 5 to be eligible.

  Scenario: Mark class attendance
    Given I am tracking attendance for "PE Training Session 3"
    When I mark "Mary Nkuna" as present
    Then her attendance count should increase to 3
    And her record should show "3 of 5 classes completed"

  Scenario: View attendance completion status
    Given I am viewing grower "Mary Nkuna"
    When I check her attendance record
    Then I should see classes attended: "5 of 5"
    And status should show "Eligible for trees"

  Scenario: Grower not eligible without required classes
    Given grower "Grace Sithole" has attended 3 of 5 classes
    When I check her eligibility status
    Then status should show "2 classes remaining"
    And she should not be marked as "Eligible for trees"

  Scenario: Roebuck cohort grower eligible for trees after completing front-loaded classes
    Given grower "Sipho Dlamini" is in cohort "Roebuck 1 2026"
    And she has attended "Tree Selection" and "Tree Planting" classes
    When I check her eligibility for tree distribution
    Then status should show "2 of 5 classes completed"
    And she should not yet be marked as "Eligible for trees"
    And a note should indicate "Remaining classes: Water Management, Soil Fertility, General Permaculture Practices"

  Scenario: Track which specific class a grower attended
    Given I am recording attendance for class "Tree Selection"
    And this class belongs to cohort "Roebuck 1 2026"
    When I mark "Sipho Dlamini" as present
    Then her record should show "Tree Selection" as attended
    And her attendance count should increase by 1

  # ─── CLASS-COHORT ASSOCIATION ─────────────────────────────────────────────────

  Scenario: Admin links a cohort to a training class from the training edit screen
    Given training class "PE Training Session 1" exists
    And cohort "Roebuck 1 2026" is "Active"
    When I edit "PE Training Session 1"
    And I select "Roebuck 1 2026" from the cohort add dropdown
    Then "Roebuck 1 2026" should appear as a tag on "PE Training Session 1"
    And the link should also be visible from the cohort edit screen

  Scenario: Training index shows cohort badges on each class row
    Given training class "PE Training Session 1" is linked to cohorts "Roebuck 1 2026" and "Orpen Gate Village 2024"
    When I navigate to the training sessions list
    Then the row for "PE Training Session 1" should display both cohort names as badges

  Scenario: Admin removes a cohort from a training class
    Given cohort "Roebuck 1 2026" is linked to training class "PE Training Session 1"
    When I edit "PE Training Session 1"
    And I click the remove button on the "Roebuck 1 2026" tag and confirm
    Then "Roebuck 1 2026" should no longer be linked to "PE Training Session 1"
```
