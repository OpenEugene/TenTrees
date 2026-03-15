@workflow-admin @priority-high @security
Feature: Mentor Management
  As a 10 Trees Admin
  I want to manage tree mentor accounts, village assignments, and grower assignments
  So that mentors can access exactly the data they need and no more

  # Data model note:
  # A Mentor is an Oqtane user assigned the "Mentor" role — there is no
  # separate Mentor database table. The MentorProfile table extends the Oqtane
  # user record with 10 Trees-specific fields (VillageId, etc.) using UserId as the
  # foreign key. Deactivating a mentor means disabling the Oqtane user account;
  # grower assignments are preserved. Currently, Grower.MentorId stores the mentor's
  # username (User.Identity.Name / PageState.User.Username), not the numeric UserId.
  # If we later migrate Grower.MentorId to use UserId instead of username, the
  # feature scenarios and implementation should be updated together.

  Background:
    Given the "Mentor" role exists in Oqtane
    And a MentorProfile record exists for each user in that role

  # ─── MENTOR LIST ─────────────────────────────────────────────────────────────

  Scenario: Admin views the tree mentor list
    Given I am logged in as a 10 Trees Admin
    When I navigate to the Mentor management page
    Then I should see a list of all users with the "Mentor" role
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
    Given I am on the Mentor management page
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
    And the "Mentor" role should be assigned to the account
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

  # ─── COHORT ASSIGNMENT ───────────────────────────────────────────────────────

  Scenario: Admin assigns a mentor to a cohort
    Given I am logged in as a 10 Trees Admin
    And cohort "Roebuck 1 2026" exists
    When I edit cohort "Roebuck 1 2026"
    And I assign mentor "Bondi" to the cohort
    Then "Bondi" should appear in the assigned mentors list for "Roebuck 1 2026"

  Scenario: Admin removes a mentor from a cohort
    Given mentor "Bondi" is assigned to cohort "Roebuck 1 2026"
    When I remove "Bondi" from cohort "Roebuck 1 2026" via the cohort edit screen
    Then "Bondi" should no longer appear in the assigned mentors list

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
    Given I am logged in as mentor "Bondi"
    When I submit an enrollment for a new grower
    Then the new grower record should have MentorId set to "Bondi"'s username
    And the grower should appear in "Bondi"'s assigned grower list

  # ─── MENTOR DATA ISOLATION ───────────────────────────────────────────────────
  # Mentors are scoped to growers via cohort membership. A mentor sees all growers
  # who belong to any cohort the mentor is assigned to. If a mentor has no cohort
  # assignments, they fall back to seeing all growers in their assigned village.

  Scenario: Mentor sees only growers from their assigned cohorts
    Given I am logged in as mentor "Bondi"
    And "Bondi" is assigned to cohort "Roebuck 1 2026" with 15 grower members
    When I navigate to the grower list
    Then I should see exactly 15 growers
    And I should not see growers who are not members of "Roebuck 1 2026"

  Scenario: Mentor assigned to multiple cohorts sees growers from all of them
    Given "Bondi" is assigned to cohorts "Roebuck 1 2026" and "Orpen Gate Village 2024"
    And "Roebuck 1 2026" has 15 growers and "Orpen Gate Village 2024" has 10 growers (no overlap)
    When I navigate to the grower list as "Bondi"
    Then I should see 25 growers total

  Scenario: Mentor cannot access another mentor's grower record
    Given I am logged in as mentor "Bondi"
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
    Given I am logged in as mentor "Bondi"
    When I navigate to my profile
    Then I should see:
      | Field            | Value              |
      | Name             | Bondi              |
      | Village          | Orpen Gate Village |
      | Assigned Growers | 10                 |
    And I should not see options to change my village or role

  # ─── PERMISSIONS ─────────────────────────────────────────────────────────────

  Scenario: Mentor role permissions
    Given the following permission rules apply to Mentors:
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
    When a user with role "Mentor" logs in
    Then the UI should show and hide features according to the table above
    And the API should enforce the same restrictions

  Scenario: Non-admin cannot access Mentor management page
    Given I am logged in as mentor "Bondi"
    When I attempt to navigate to the Mentor management page
    Then I should be redirected or shown an "Access Denied" message
