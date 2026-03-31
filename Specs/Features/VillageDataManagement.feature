@workflow-village @priority-high @multi-tenant
Feature: Village-Scoped Data Access
  As a user with edit permissions
  I want to organize data by village
  So that each village sees only their own data while authorized users see all

  Scenario: Mentor views village-specific growers
    Given I am mentor "Bondi" assigned to "Orpen Gate Village"
    When I view the grower list
    Then I should only see growers in "Orpen Gate Village"
    And I should not see growers from "Londelozzi"

  Scenario: Authorized user views all villages
    Given I have village management permissions
    When I view the grower list
    Then I should see a village filter dropdown
    When I select "All Villages"
    Then I should see growers from all villages
    When I select "Orpen Gate Village"
    Then I should only see growers from "Orpen Gate Village"

  Scenario: Add new village
    Given I have village edit permissions
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
  # Cohorts are village-scoped groupings of growers managed via the Cohort Management
  # module. Full cohort lifecycle, assignment, and tag UI scenarios are in
  # Specs/Features/CohortManagement.feature.

  Scenario: Cohort filter appears in grower list when the authorized user selects a village
    Given cohorts "Orpen Gate Village 2023" and "Orpen Gate Village 2024" exist for "Orpen Gate Village"
    When I select village "Orpen Gate Village" from the grower list village filter
    Then a cohort filter dropdown should appear
    And it should list "Orpen Gate Village 2023" and "Orpen Gate Village 2024"

  Scenario: Selecting a cohort in the grower list filters to that cohort's members
    Given multiple cohorts exist for "Orpen Gate Village"
    When I select cohort "Orpen Gate Village 2024" from the cohort filter
    Then I should see only the growers who are members of "Orpen Gate Village 2024"
    And growers from other cohorts should not appear
