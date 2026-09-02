# Village Data Management

Data is organized by village so each village sees only its own data while authorized users see all. Translated from the retired feature file `Specs/Features/VillageDataManagement.feature` (high priority, multi-tenant).

## Village scoping

- A mentor assigned to a village sees only that village's growers in the grower list — never another village's.
- Village data is fully isolated: a mentor from a 30-grower village sees exactly those 30, with nothing visible from other villages.
- Users with village management permissions get a village filter dropdown with an "All Villages" option; selecting a specific village narrows the list to it.

## Adding a village

A user with village edit permissions can add a new village from village management, set its contact information, and the village becomes available for mentor assignment.

## Cohort filtering in the grower list

Cohorts are village-scoped groupings of growers. When an authorized user selects a village in the grower list, a cohort filter dropdown appears listing that village's cohorts; selecting one filters the list to that cohort's members only. Full cohort lifecycle, assignment, and tag behaviour is documented in [Cohort Management](/Specifications/Cohort-Management).

## Related pages

- [Mentor Management](/Specifications/Mentor-Management) — village assignment of mentors.
- [Role-Based Data Visibility](/Specifications/Role-Based-Data-Visibility)

## Scenarios (Gherkin)

The original scenarios, preserved verbatim from the retired `Specs/Features/VillageDataManagement.feature` as the precise acceptance criteria for the behaviour described above.

```gherkin
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
```
