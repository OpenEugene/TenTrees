@workflow-village @priority-high @multi-tenant
Feature: Village-Scoped Data Access
  As a system administrator
  I want to organize data by village
  So that each village sees only their own data while admins see all

  Scenario: Mentor views village-specific growers
    Given I am mentor "Bondi" assigned to "Orpen Gate Village"
    When I view the grower list
    Then I should only see growers in "Orpen Gate Village"
    And I should not see growers from "Londelozzi"

  Scenario: Admin views all villages
    Given I am logged in as administrator "Becky"
    When I view the grower list
    Then I should see a village filter dropdown
    When I select "All Villages"
    Then I should see growers from all villages
    When I select "Orpen Gate Village"
    Then I should only see growers from "Orpen Gate Village"

  Scenario: Add new village
    Given I am an administrator
    When I navigate to village management
    And I add village "Londelozzi"
    And I set village contact information
    Then the village should be available for assignment
    And mentors can be assigned to it

  Scenario: Village data isolation
    Given "Orpen Gate Village" has 50 growers
    And "Londelozzi" has 30 growers
    When mentor from "Londelozzi" logs in
    Then they should see exactly 30 growers
    And no data from "Orpen Gate Village" should be visible

  # ─── COHORT MANAGEMENT ──────────────────────────────────────────────────────

  Scenario: System auto-generates a cohort name from village and year
    Given I am a 10 Trees Admin
    When I create a new cohort for village "Roebuck" in year "2026"
    Then the system should suggest the name "Roebuck 2026"
    And I should be able to accept or overwrite the suggested name

  Scenario: Admin creates a numbered cohort for a village with multiple cohorts
    Given "Roebuck" already has a cohort named "Roebuck 2026"
    When I create a second cohort for "Roebuck" in year "2026"
    Then the system should suggest the name "Roebuck 2 2026"
    And I should be able to rename it to "Roebuck 1 2026" and "Roebuck 2 2026" respectively

  Scenario: Admin views all cohorts
    Given the following cohorts exist:
      | Cohort Name             | Village            | Households |
      | Orpen Gate Village 2023 | Orpen Gate Village | 153        |
      | Open Gate Village 2024  | Orpen Gate Village | 57         |
      | Roebuck 1 2026          | Roebuck            | 55         |
    When I navigate to cohort management
    Then I should see all three cohorts listed with their household counts

  Scenario: Filter grower list by cohort
    Given multiple cohorts exist for "Orpen Gate Village"
    When I select cohort "Open Gate Village 2024" from the filter
    Then I should see only the 57 growers in that cohort
    And growers from other cohorts should not appear

  Scenario: Reports can be scoped to a specific cohort
    Given cohort "Roebuck 1 2026" exists with 55 households
    When I generate a tree survival report
    And I select cohort "Roebuck 1 2026"
    Then results should reflect only those 55 households
