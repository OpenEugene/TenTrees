# Grower Status Management

Administrators manage grower status across Active, Inactive, and Exited states to keep program statistics and participation records accurate. Translated from the retired feature file `Specs/Features/GrowerProgramExit.feature` (medium priority, security).

## Status badges

Grower lists colour-code status: **Active** = green, **Inactive** = yellow, **Exited** = grey.

## Active / Inactive toggle

From a grower's record an admin can click **Mark Inactive** or **Mark Active** to toggle between the two states, with a success confirmation each way.

## Recording a program exit

Both Active and Inactive growers can be exited. The admin clicks **Record Exit**, enters an exit date, selects a reason, and clicks **Confirm Exit**; the grower's status becomes Exited with the date and reason recorded.

Exit reasons are a fixed dropdown:

- Moved away
- Deceased
- Voluntary withdrawal
- Non-compliance
- Other

Rules on the exit form:

- **Exit notes** are required when the reason is "Other" (a text area appears and the exit cannot be confirmed without notes); for any other reason the notes area is not shown and notes are not required.
- Exit **date** and **reason** are both required — confirming without either shows a validation error ("Exit date is required" / "Exit reason is required") and nothing is recorded.
- **Cancel** dismisses the form leaving the status unchanged.

## Post-exit behaviour

- An exited grower's record shows the exit date and reason; the "Record Exit" and "Mark Inactive" buttons are hidden so they cannot be exited again.
- Exited growers are excluded from active-grower queries and lists.
- Exited growers cannot receive new assessments — see [Garden Assessment](/Specifications/Garden-Assessment).

## Access control

Only admins can change grower status. Mentors see the current status and exit details read-only, with no "Mark Inactive", "Mark Active", or "Record Exit" buttons.

## Scenarios (Gherkin)

The original scenarios, preserved verbatim from the retired `Specs/Features/GrowerProgramExit.feature` as the precise acceptance criteria for the behaviour described above.

```gherkin
@workflow-exit @priority-medium @security
Feature: Grower Status Management
  As an administrator
  I want to manage grower status across Active, Inactive, and Exited states
  So that we maintain accurate program statistics and participation records

  Background:
    Given I am logged in as an administrator

  # ─── STATUS OVERVIEW ────────────────────────────────────────────────────────

  Scenario: Grower status badge colours match state
    When I view a grower list
    Then Active growers should show a green badge
    And Inactive growers should show a yellow badge
    And Exited growers should show a grey badge

  # ─── ACTIVE / INACTIVE TOGGLE ───────────────────────────────────────────────

  Scenario: Mark an active grower as inactive
    Given grower "Grace Sithole" has status "Active"
    When I navigate to her record
    And I click "Mark Inactive"
    Then her status should change to "Inactive"
    And I should see a success confirmation message

  Scenario: Reactivate an inactive grower
    Given grower "Grace Sithole" has status "Inactive"
    When I navigate to her record
    And I click "Mark Active"
    Then her status should change to "Active"
    And I should see a success confirmation message

  # ─── PROGRAM EXIT ────────────────────────────────────────────────────────────

  Scenario: Record program exit for an active grower
    Given grower "Grace Sithole" has status "Active"
    When I navigate to her record
    And I click "Record Exit"
    And I enter exit date "2025-11-15"
    And I select reason "Moved away"
    And I click "Confirm Exit"
    Then her status should change to "Exited"
    And exit reason should be recorded as "Moved away"
    And exit date should be recorded as "2025-11-15"

  Scenario: Record program exit for an inactive grower
    Given grower "Grace Sithole" has status "Inactive"
    When I navigate to her record
    And I click "Record Exit"
    And I enter exit date "2025-11-15"
    And I select reason "Non-compliance"
    And I click "Confirm Exit"
    Then her status should change to "Exited"

  Scenario: Exit reasons are predefined selectable options
    Given I am recording a program exit
    When I view the exit reason dropdown
    Then I should see exactly the following options:
      | Reason                  |
      | Moved away              |
      | Deceased                |
      | Voluntary withdrawal    |
      | Non-compliance          |
      | Other                   |

  Scenario: Exit notes are required when reason is "Other"
    Given I am recording a program exit
    When I select reason "Other"
    Then an "Exit Notes" text area should appear and be required
    And I should not be able to confirm the exit without entering notes

  Scenario: Exit notes are optional when reason is not "Other"
    Given I am recording a program exit
    When I select reason "Moved away"
    Then the exit notes text area should not be shown
    And the exit should be confirmable without notes

  Scenario: Validate exit date is required before confirming exit
    Given I am on the exit form for "Grace Sithole"
    When I click "Confirm Exit" without entering an exit date
    Then I should see validation error "Exit date is required"
    And the exit should not be recorded

  Scenario: Validate exit reason is required before confirming exit
    Given I am on the exit form for "Grace Sithole"
    And I have entered an exit date
    When I click "Confirm Exit" without selecting a reason
    Then I should see validation error "Exit reason is required"
    And the exit should not be recorded

  Scenario: Cancel exit returns to grower status view
    Given I have opened the exit form for "Grace Sithole"
    When I click "Cancel"
    Then the exit form should be dismissed
    And her status should remain unchanged

  # ─── POST-EXIT BEHAVIOUR ─────────────────────────────────────────────────────

  Scenario: Exited grower cannot be exited again
    Given grower "Grace Sithole" has status "Exited"
    When I navigate to her record
    Then I should see her exit date and exit reason displayed
    And the "Record Exit" and "Mark Inactive" buttons should not be visible

  Scenario: Exited grower excluded from active grower queries
    Given grower "Grace Sithole" has exited the program
    When the system fetches active growers
    Then "Grace Sithole" should not appear in the active grower list

  # ─── ACCESS CONTROL ──────────────────────────────────────────────────────────

  Scenario: Only admins can change grower status
    Given I am logged in as a mentor
    When I navigate to a grower's status page
    Then I should not see "Mark Inactive", "Mark Active", or "Record Exit" buttons
    And I should be able to view the current status and exit details in read-only mode
```
