@workflow-admin @priority-high @security
Feature: User and Access Management
  As an administrator
  I want to manage user accounts and permissions
  So that the right people have appropriate access

  Scenario: Admin impersonates mentor for data entry
    Given I am administrator "Becky"
    And mentor "Bondi" has submitted paper assessment forms
    When I select "Enter data as Bondi"
    And I complete an assessment form for "Mary Nkuna"
    Then the assessment should be recorded with mentor "Bondi"
    And audit log should show:
      | Field        | Value               |
      | Action       | Assessment Created  |
      | Entered By   | Becky               |
      | On Behalf Of | Bondi               |
      | Timestamp    | [current time]      |

  Scenario: Assign mentor to village
    Given I am an administrator
    When I navigate to user "Bondi"
    And I assign her to village "Orpen Gate Village"
    Then she should only see data for "Orpen Gate Village"
    And her village assignment should be recorded

  Scenario: Assign mentor to specific households
    Given mentor "Bondi" is assigned to "Orpen Gate Village"
    When I assign households 1-10 to "Bondi"
    Then she should see those 10 households in her list
    And other mentors should not see those households assigned

  Scenario: Create new mentor account
    Given I am an administrator
    When I create new user:
      | Field   | Value                |
      | Name    | Thandi Nkosi         |
      | Role    | Mentor               |
      | Village | Orpen Gate Village   |
      | Email   | thandi@example.com   |
    Then the account should be created
    And login credentials should be generated

  Scenario: Staff role permissions
    Given the following roles exist:
      | Role               | Can Submit Forms | Can View All Villages | Can Export | Can Manage Users |
      | Mentor             | Yes              | No                    | No         | No               |
      | Educator           | Yes              | Yes                   | Yes        | No               |
      | Project Manager    | Yes              | Yes                   | Yes        | No               |
      | Admin              | Yes              | Yes                   | Yes        | Yes              |
      | Executive Director | Yes              | Yes                   | Yes        | Yes              |
    When a user logs in with role "Mentor"
    Then they should have permissions matching the Mentor row
