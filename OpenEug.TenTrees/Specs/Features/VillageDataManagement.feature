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

  Scenario: Add new village with contact information
    Given I am an administrator
    When I navigate to village management
    And I create a new village with:
      | Field         | Value                  |
      | Village Name  | Londelozzi             |
      | Contact Name  | Sipho Dlamini          |
      | Contact Phone | 082 555 1234           |
      | Contact Email | sipho@example.com      |
      | Notes         | Near main tar road     |
      | Is Active     | true                   |
    Then the village should be saved with all contact details
    And it should appear in the village selection dropdown for enrollment

  Scenario: Edit existing village contact details
    Given village "Orpen Gate Village" already exists
    When I update the contact phone to "083 999 0000"
    Then the village record should reflect the new phone number

  Scenario: Deactivate a village
    Given village "Old Site" is active
    When I set its "Is Active" flag to false
    Then "Old Site" should no longer appear in the active village dropdown
    And existing grower records linked to it should remain intact

  Scenario: Village data isolation
    Given "Orpen Gate Village" has 50 growers
    And "Londelozzi" has 30 growers
    When mentor from "Londelozzi" logs in
    Then they should see exactly 30 growers
    And no data from "Orpen Gate Village" should be visible
