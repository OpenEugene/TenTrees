# User Administration

Administrators manage user accounts and permissions so the right people have appropriate access. Translated from the retired feature file `Specs/Features/UserAdministration.feature` (high priority, security).

## Admin impersonation for data entry

An administrator can enter data on behalf of a mentor (e.g. from paper assessment forms) via "Enter data as <mentor>". The record is attributed to the mentor, and the audit log captures the action, who entered it, who it was on behalf of, and the timestamp.

## Assignments

- A mentor assigned to a village sees only that village's data, and the assignment is recorded.
- Within a village, specific households can be assigned to a mentor; those households appear only in that mentor's list.
- Creating a mentor account (name, role, village, email) generates login credentials. See [Mentor Management](/Specifications/Mentor-Management) for full mentor CRUD.

## Role permission matrix

| Role | Submit Forms | View All Villages | Add Notes | Export | Manage Users |
|---|---|---|---|---|---|
| Mentor | Yes | No | No | No | No |
| Educator | No | Yes | Yes | No | No |
| Project Manager | No | Yes | Yes | Yes | No |
| 10 Trees Admin | Yes | Yes | Yes | Yes | Yes |

An **Educator** can view any grower's full record in any village and add home visit notes, but cannot edit enrollment or assessment data.

### Role naming notes (from the spec)

- There is **no Executive Director role** — the organisation uses distributed leadership. Tri holds the title "Director of Permaculture Education and Community Development". The chief admin role is "10 Trees Admin" (not "center admin").
- The correct term for the permaculture educator role is still under discussion (Rebecca to confirm with Tri).
- **Open question (March 2026 check-in):** whether the Educator role should be able to edit grower record data beyond adding notes is unresolved; the current spec treats Educator as view + notes only, pending confirmation from Rebecca.

## Platform admin vs programme admin

The Oqtane **Platform Admin** (system-level) and the **10 Trees Admin** (programme-level) are separate roles, even when held by the same person, and role names in the UI should be prefixed to avoid confusion:

| Role | Manages Programme Data | Can Add/Remove Pages | Can Manage Site Settings |
|---|---|---|---|
| 10 Trees Admin | Yes | No | No |
| Platform Admin | No | Yes | Yes |

A user holding only 10 Trees Admin can create, edit, and assign programme records but never sees site management options ("Add Page", "Manage Modules") — the programme director has full data access across all villages without any risk of inadvertently altering site structure. A user holding only Platform Admin manages pages and site settings but has no elevated access to programme data.

## Related pages

- [Role-Based Data Visibility](/Specifications/Role-Based-Data-Visibility)
- [Village Data Management](/Specifications/Village-Data-Management)

## Scenarios (Gherkin)

The original scenarios, preserved verbatim from the retired `Specs/Features/UserAdministration.feature` as the precise acceptance criteria for the behaviour described above.

```gherkin
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
      | Role    | Mentor          |
      | Village | Orpen Gate Village   |
      | Email   | thandi@example.com   |
    Then the account should be created
    And login credentials should be generated

  # NOTE: There is no Executive Director role. The organisation uses a distributed
  # leadership model. Tri holds the title "Director of Permaculture Education and
  # Community Development". The chief admin role is "10 Trees Admin" (NOT "center admin").
  # The correct term for the permaculture educator role is still under discussion
  # (Rebecca to confirm with Tri).
  #
  # OPEN QUESTION (March 2026 check-in): Whether the Educator role should be able to
  # edit grower record data (beyond adding notes) is unresolved. Current spec treats
  # the Educator as view + notes only. Pending final confirmation from Rebecca.
  #
  # PLATFORM ADMIN NOTE: The Oqtane platform has a system-level admin role that can
  # add/remove pages and manage site structure. This is SEPARATE from the "10 Trees Admin"
  # programme role. Role names in the UI should be prefixed to avoid confusion
  # (e.g. "10 Trees Admin" vs "Platform Admin"). See scenarios below.
  Scenario: Staff role permissions
    Given the following roles exist:
      | Role              | Can Submit Forms | Can View All Villages | Can Add Notes | Can Export | Can Manage Users |
      | Mentor       | Yes              | No                    | No            | No         | No               |
      | Educator          | No               | Yes                   | Yes           | No         | No               |
      | Project Manager   | No               | Yes                   | Yes           | Yes        | No               |
      | 10 Trees Admin    | Yes              | Yes                   | Yes           | Yes        | Yes              |
    When a user logs in with role "Mentor"
    Then they should have permissions matching the Mentor row

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

  # ─── PLATFORM ADMIN vs PROGRAMME ADMIN ─────────────────────────────────────
  # The Oqtane platform admin (system-level) can add/remove pages and manage site
  # structure. The 10 Trees Admin (programme-level) manages data, villages, and users.
  # These are separate roles. Both may be held by the same person but must be
  # distinguished in the UI and documentation to prevent accidental site changes.

  Scenario: Platform admin role is separate from 10 Trees programme admin role
    Given the system has the following distinct admin roles:
      | Role            | Manages Programme Data | Can Add / Remove Pages | Can Manage Site Settings |
      | 10 Trees Admin  | Yes                    | No                     | No                       |
      | Platform Admin  | No                     | Yes                    | Yes                      |
    When a user holds only the "10 Trees Admin" role
    Then they should be able to create, edit, and assign programme records
    But they should not see options to add or remove pages or change site settings
    When a user holds only the "Platform Admin" role
    Then they should be able to manage pages and site settings
    But they should not have elevated access to programme data

  Scenario: Programme director has full data access without requiring platform admin rights
    Given I am logged in as programme director "Rebecca"
    And my role is "10 Trees Admin"
    When I view the grower list
    Then I should see growers from all villages
    And I should be able to create, edit, and assign programme records
    But I should not see site management options such as "Add Page" or "Manage Modules"
    And my actions should not be able to inadvertently alter site structure
```
