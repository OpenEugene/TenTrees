@workflow-admin @priority-high @security
Feature: Tree Mentor Management
  As a 10 Trees Admin
  I want to manage tree mentor accounts, village assignments, and grower assignments
  So that mentors can access exactly the data they need and no more

  # Data model note:
  # A Tree Mentor is an Oqtane user assigned the "Tree Mentor" role — there is no
  # separate TreeMentor database table. The MentorProfile table extends the Oqtane
  # user record with 10 Trees-specific fields (VillageId, etc.) using UserId as the
  # foreign key. Deactivating a mentor means disabling the Oqtane user account;
  # grower assignments (Grower.MentorId = UserId) are preserved.

  Background:
    Given the "Tree Mentor" role exists in Oqtane
    And a MentorProfile record exists for each user in that role

  # ─── MENTOR LIST ─────────────────────────────────────────────────────────────

  Scenario: Admin views the tree mentor list
    Given I am logged in as a 10 Trees Admin
    When I navigate to the Tree Mentor management page
    Then I should see a list of all users with the "Tree Mentor" role
    And each row should show:
      | Column          |
      | Name            |
      | Username        |
      | Email           |
      | Assigned Village |
      | Grower Count    |
      | Active          |

  Scenario: Admin filters mentor list by village
    Given there are mentors assigned to "Orpen Gate Village" and "Londelozzi"
    When I filter the mentor list by village "Orpen Gate Village"
    Then I should only see mentors assigned to "Orpen Gate Village"

  Scenario: Admin searches mentor list by name
    Given I am on the Tree Mentor management page
    When I type "Bondi" in the search box
    Then the list should filter to show only mentors whose name contains "Bondi"

  # ─── CREATE MENTOR ────────────────────────────────────────────────────────────

  Scenario: Admin creates a new tree mentor account
    Given I am logged in as a 10 Trees Admin
    When I click "Add Mentor"
    And I fill in the new mentor form:
      | Field    | Value                  |
      | Name     | Thandi Nkosi           |
      | Email    | thandi@tentrees.org    |
      | Username | thandi                 |
      | Village  | Orpen Gate Village     |
    And I click "Save"
    Then a new Oqtane user account should be created for "Thandi Nkosi"
    And the "Tree Mentor" role should be assigned to the account
    And the village "Orpen Gate Village" should be recorded on the mentor profile
    And the mentor should appear in the mentor list

  Scenario: Mentor email is required
    Given I am creating a new mentor account
    When I submit the form without an email address
    Then I should see a validation error "Email is required"
    And no account should be created

  Scenario: Duplicate username is rejected
    Given a user with username "bondi" already exists
    When I attempt to create a new mentor with username "bondi"
    Then I should see an error indicating the username is already taken

  # ─── EDIT MENTOR ─────────────────────────────────────────────────────────────

  Scenario: Admin edits a mentor's village assignment
    Given I am viewing mentor "Bondi"'s profile
    When I change her assigned village from "Orpen Gate Village" to "Londelozzi"
    And I click "Save"
    Then "Bondi"'s village should be updated to "Londelozzi"
    And she should now see only growers in "Londelozzi"

  Scenario: Admin deactivates a mentor account
    Given mentor "Bondi" is currently active
    When I deactivate her account
    Then she should no longer be able to log in
    And her grower assignments should remain intact
    And she should appear in the mentor list marked as "Inactive"

  Scenario: Admin reactivates a mentor account
    Given mentor "Bondi" is currently inactive
    When I reactivate her account
    Then she should be able to log in again
    And her previous village and grower assignments should be restored

  # ─── GROWER ASSIGNMENT ───────────────────────────────────────────────────────

  Scenario: Admin assigns growers to a mentor
    Given I am viewing mentor "Bondi"'s profile
    And the following growers in "Orpen Gate Village" are unassigned:
      | Grower       |
      | Mary Nkuna   |
      | Grace Sithole |
    When I assign both growers to "Bondi"
    Then "Bondi"'s grower count should increase by 2
    And "Mary Nkuna" and "Grace Sithole" should appear in "Bondi"'s grower list

  Scenario: Admin reassigns a grower from one mentor to another
    Given grower "Mary Nkuna" is currently assigned to mentor "Bondi"
    When I reassign "Mary Nkuna" to mentor "Trygive"
    Then "Mary Nkuna" should no longer appear in "Bondi"'s grower list
    And "Mary Nkuna" should appear in "Trygive"'s grower list

  Scenario: Mentor is auto-assigned at enrollment time
    Given I am logged in as tree mentor "Bondi"
    When I submit an enrollment for a new grower
    Then the new grower record should have MentorId set to "Bondi"'s user ID
    And the grower should appear in "Bondi"'s assigned grower list

  # ─── MENTOR DATA ISOLATION ───────────────────────────────────────────────────

  Scenario: Mentor sees only her assigned growers
    Given I am logged in as tree mentor "Bondi" with 10 assigned growers
    When I navigate to the grower list
    Then I should see exactly 10 growers
    And I should not see growers assigned to other mentors

  Scenario: Mentor cannot access another mentor's grower record
    Given I am logged in as tree mentor "Bondi"
    And grower "Peter Mthembu" is assigned to mentor "Trygive"
    When I attempt to navigate directly to Peter's record
    Then I should be denied access
    And I should see a message "You are not assigned to this household"

  Scenario: Mentor sees only her assigned village
    Given I am logged in as mentor "Bondi" assigned to "Orpen Gate Village"
    When I view any village-filtered dropdown or list
    Then I should only see data for "Orpen Gate Village"
    And I should not see data from "Londelozzi" or other villages

  Scenario: Staff roles can see all villages and all mentors
    Given I am logged in as an Educator
    When I view the grower list
    Then I should see growers from all villages
    And I should be able to filter by any mentor

  # ─── MENTOR PROFILE (SELF-VIEW) ──────────────────────────────────────────────

  Scenario: Mentor views their own profile
    Given I am logged in as tree mentor "Bondi"
    When I navigate to my profile
    Then I should see:
      | Field            | Value              |
      | Name             | Bondi              |
      | Village          | Orpen Gate Village |
      | Assigned Growers | 10                 |
    And I should not see options to change my village or role

  # ─── PERMISSIONS ─────────────────────────────────────────────────────────────

  Scenario: Tree Mentor role permissions
    Given the following permission rules apply to Tree Mentors:
      | Permission             | Allowed |
      | Submit enrollment form | Yes     |
      | Submit assessment form | Yes     |
      | Submit photo release   | Yes     |
      | View assigned growers  | Yes     |
      | View all villages      | No      |
      | View other mentors     | No      |
      | Export data            | No      |
      | Manage users           | No      |
      | Access admin panel     | No      |
    When a user with role "Tree Mentor" logs in
    Then the UI should show and hide features according to the table above
    And the API should enforce the same restrictions

  Scenario: Non-admin cannot access Tree Mentor management page
    Given I am logged in as tree mentor "Bondi"
    When I attempt to navigate to the Tree Mentor management page
    Then I should be redirected or shown an "Access Denied" message
