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
    Given I am a 10 Trees Admin
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
    Given I am a 10 Trees Admin
    When I create new user:
      | Field   | Value                |
      | Name    | Thandi Nkosi         |
      | Role    | Tree Mentor          |
      | Village | Orpen Gate Village   |
      | Email   | thandi@example.com   |
    Then the account should be created
    And login credentials should be generated

  # NOTE: There is no Executive Director role. The organisation uses a distributed
  # leadership model. Tri holds the title "Director of Permaculture Education and
  # Community Development". The chief admin role is "10 Trees Admin" (NOT "center admin").
  # The correct term for the permaculture educator role is still under discussion
  # (Rebecca to confirm with Tri).
  Scenario: Staff role permissions
    Given the following roles exist:
      | Role              | Can Submit Forms | Can View All Villages | Can Add Notes | Can Export | Can Manage Users |
      | Tree Mentor       | Yes              | No                    | No            | No         | No               |
      | Educator          | No               | Yes                   | Yes           | No         | No               |
      | Project Manager   | No               | Yes                   | Yes           | Yes        | No               |
      | 10 Trees Admin    | Yes              | Yes                   | Yes           | Yes        | Yes              |
    When a user logs in with role "Tree Mentor"
    Then they should have permissions matching the Tree Mentor row

  Scenario: Tree mentor sees only their 10 assigned households
    Given I am tree mentor "Bondi" with 10 assigned households
    When I view the grower list
    Then I should only see my 10 assigned households
    And I should not see households assigned to other mentors

  Scenario: Educator can add visit notes to any grower record
    Given I am logged in as an Educator
    When I navigate to grower "Mary Nkuna" in any village
    Then I should be able to view her full record
    And I should be able to add a home visit note
    But I should not see an option to edit her enrollment or assessment data
