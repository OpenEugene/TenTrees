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
