@workflow-security @priority-high @multi-tenant
Feature: Role-Based UX and Data Visibility
  As a 10 Trees platform user
  I want to see only the data and features relevant to my role
  So that privacy is protected and the user experience is streamlined

  # ─── BACKGROUND ──────────────────────────────────────────────────────────────
  # The platform supports three primary roles with distinct visibility rules:
  # 1. Mentor: Field agents scoped strictly to their assigned growers (via Cohort, Village, or direct assignment).
  # 2. Project Manager (PM): Centre staff with global read/write access to program data and reporting, but no system admin rights.
  # 3. 10 Trees Admin: Full access to all program data, user management, and configuration.

  Background:
    Given the following roles exist in the system:
      | Role            | Scope Level |
      | Mentor          | Assigned    |
      | Project Manager | Global      |
      | 10 Trees Admin  | Global      |

  # ─── MENTOR DATA SCOPING ─────────────────────────────────────────────────────

  Scenario: Mentor visibility is scoped to assigned cohorts
    Given I am logged in as a "Mentor"
    And I am assigned to cohort "Roebuck 1 2026"
    And "Roebuck 1 2026" has 15 active growers
    When I navigate to the Grower List
    Then I should see exactly 15 growers
    And I should not see growers from other cohorts or villages

  Scenario: Mentor visibility falls back to assigned village if no cohorts are assigned
    Given I am logged in as a "Mentor"
    And I am assigned to "Orpen Gate Village"
    And I am not assigned to any specific cohorts
    When I navigate to the Grower List
    Then I should see all growers in "Orpen Gate Village"
    And I should not see growers from "Londelozzi"

  Scenario: Mentor is blocked from accessing unassigned grower records
    Given I am logged in as a "Mentor"
    And grower "Peter Mthembu" is assigned to a different mentor
    When I attempt to navigate directly to the record for "Peter Mthembu"
    Then I should be denied access
    And I should see a message "You are not assigned to this household"

  # ─── PM AND ADMIN GLOBAL VISIBILITY ──────────────────────────────────────────

  Scenario: Project Manager has global visibility across all villages
    Given I am logged in as a "Project Manager"
    When I navigate to the Grower List
    Then I should see a village filter dropdown
    And I should be able to select "All Villages" to view all growers in the program

  Scenario: 10 Trees Admin has global visibility across all villages
    Given I am logged in as a "10 Trees Admin"
    When I navigate to the Grower List
    Then I should see a village filter dropdown
    And I should be able to select "All Villages" to view all growers in the program

  # ─── FEATURE-SPECIFIC PERMISSIONS ────────────────────────────────────────────

  Scenario Outline: Role-based access to specific modules and actions
    Given I am logged in with the role "<Role>"
    When I attempt to access the "<Feature>"
    Then my access should be "<Access Level>"

    Examples:
      | Role            | Feature                      | Access Level |
      | Mentor          | Submit New Enrollment        | Allowed      |
      | Mentor          | Approve/Reject Enrollment    | Denied       |
      | Mentor          | Submit Garden Assessment     | Allowed      |
      | Mentor          | Change Grower Status (Exit)  | Read-Only    |
      | Mentor          | Program Reporting Module     | Denied       |
      | Mentor          | Mentor Management Module     | Denied       |
      | Project Manager | Submit New Enrollment        | Allowed      |
      | Project Manager | Approve/Reject Enrollment    | Allowed      |
      | Project Manager | Change Grower Status (Exit)  | Allowed      |
      | Project Manager | Program Reporting Module     | Allowed      |
      | Project Manager | Mentor Management Module     | Denied       |
      | 10 Trees Admin  | Approve/Reject Enrollment    | Allowed      |
      | 10 Trees Admin  | Change Grower Status (Exit)  | Allowed      |
      | 10 Trees Admin  | Program Reporting Module     | Allowed      |
      | 10 Trees Admin  | Mentor Management Module     | Allowed      |

  # ─── UI/UX STREAMLINING ──────────────────────────────────────────────────────

  Scenario: Mentor navigation menu is streamlined
    Given I am logged in as a "Mentor"
    When I view the main navigation menu
    Then I should see links for "Enrollment", "Growers", "Assessments", and "Classes"
    And I should not see links for "Reports", "Mentors", "Villages", or "Cohorts"

  Scenario: Admin navigation menu includes management features
    Given I am logged in as a "10 Trees Admin"
    When I view the main navigation menu
    Then I should see links for all data entry modules
    And I should also see links for "Reports", "Mentors", "Villages", and "Cohorts"
