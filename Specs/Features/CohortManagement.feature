@workflow-cohort @priority-high
Feature: Cohort Management
  As a 10 Trees Admin
  I want to organise growers and mentors into named cohorts within a village
  So that I can track program phases, run cohort-scoped reports, and manage each group independently

  Background:
    Given the following villages exist: "Orpen Gate Village", "Roebuck"

  # ─── COHORT LIFECYCLE ────────────────────────────────────────────────────────

  Scenario: Admin creates a planned cohort
    Given I am a 10 Trees Admin
    When I create a new cohort for village "Roebuck" in year "2026"
    Then the cohort should have status "Planned"
    And the system should suggest the name "Roebuck 2026"
    And I should be able to accept or overwrite the suggested name

  Scenario: Admin activates a planned cohort
    Given cohort "Roebuck 1 2026" has status "Planned"
    When I set the cohort status to "Active"
    Then the cohort status should be "Active"
    And it should appear in active cohort filters

  Scenario: Admin marks a cohort as completed
    Given cohort "Orpen Gate Village 2023" has status "Active"
    When I set the cohort status to "Completed"
    Then the cohort status should be "Completed"
    And it should no longer appear in active cohort filters by default
    But it should be visible when "Show completed cohorts" is enabled

  Scenario: Completed cohort cannot be reactivated
    Given cohort "Orpen Gate Village 2023" has status "Completed"
    When I attempt to set the cohort status to "Active" or "Planned"
    Then the status should remain "Completed"
    And I should see a message "A completed cohort cannot be reactivated"

  Scenario: System increments the cohort number when a village already has one that year
    Given "Roebuck" already has a cohort named "Roebuck 2026"
    When I create a second cohort for "Roebuck" in year "2026"
    Then the system should suggest the name "Roebuck 2 2026"
    And the existing cohort should be renameable to "Roebuck 1 2026"

  Scenario: Admin saves a cohort with a custom name
    Given I am creating a cohort for village "Orpen Gate Village"
    When I enter the name "Orpen Gate Village 2023"
    And I save the cohort
    Then a cohort named "Orpen Gate Village 2023" should exist for "Orpen Gate Village"

  # ─── ASSIGNING GROWERS ───────────────────────────────────────────────────────

  Scenario: Admin adds a grower to a cohort at enrollment
    Given cohort "Roebuck 1 2026" is "Active" for village "Roebuck"
    When I enroll grower "Nomsa Dlamini" into "Roebuck"
    Then I should optionally be able to select a cohort from the enrollment form
    And I select "Roebuck 1 2026"
    Then grower "Nomsa Dlamini" should be a member of cohort "Roebuck 1 2026"

  Scenario: Cohort selection is optional at enrollment
    Given cohort "Roebuck 1 2026" is "Active" for village "Roebuck"
    When I enroll grower "Grace Sithole" into "Roebuck" without selecting a cohort
    Then the enrollment should be saved successfully
    And "Grace Sithole" should not be a member of any cohort
    And she can be added to a cohort later from the grower status screen

  Scenario: Admin adds a grower to a second cohort
    Given grower "Sipho Nkosi" is a member of cohort "Orpen Gate Village 2023"
    And cohort "Orpen Gate Village 2024" is "Active"
    When I add "Sipho Nkosi" to cohort "Orpen Gate Village 2024"
    Then grower "Sipho Nkosi" should be a member of both cohorts
    And they should appear when filtering by either cohort

  Scenario: Admin removes a grower from a cohort
    Given grower "Sipho Nkosi" is a member of cohorts "Orpen Gate Village 2023" and "Orpen Gate Village 2024"
    When I remove "Sipho Nkosi" from cohort "Orpen Gate Village 2023"
    Then they should only be a member of "Orpen Gate Village 2024"

  # ─── CLASS ASSOCIATION ───────────────────────────────────────────────────────

  Scenario: Admin links a training class to a cohort from the training edit screen
    Given training class "PE Training Session 1" exists
    And cohort "Roebuck 1 2026" is "Active"
    When I edit "PE Training Session 1"
    And I select "Roebuck 1 2026" from the "Add cohort" dropdown
    Then "Roebuck 1 2026" should appear as a tag on "PE Training Session 1"

  Scenario: Admin links a training class to a cohort from the cohort edit screen
    Given cohort "Roebuck 1 2026" is "Active"
    And training class "PE Training Session 1" exists
    When I edit cohort "Roebuck 1 2026"
    And I select "PE Training Session 1" from the "Add class" dropdown
    Then "PE Training Session 1" should appear in the linked classes list for "Roebuck 1 2026"

  Scenario: Admin removes a cohort from a training class with confirmation
    Given cohort "Roebuck 1 2026" is linked to training class "PE Training Session 1"
    When I click the remove button on the "Roebuck 1 2026" tag in the training edit screen
    Then I should see an inline confirmation "Remove Roebuck 1 2026? Yes / No"
    When I click "Yes"
    Then "Roebuck 1 2026" should no longer appear as a tag on "PE Training Session 1"

  Scenario: Admin cancels removing a cohort tag from a training class
    Given cohort "Roebuck 1 2026" is linked to training class "PE Training Session 1"
    When I click the remove button on the "Roebuck 1 2026" tag
    Then I should see an inline confirmation
    When I click "No"
    Then "Roebuck 1 2026" should still appear as a tag on "PE Training Session 1"

  Scenario: Training index displays cohort tags for each class
    Given training class "PE Training Session 1" is linked to cohort "Roebuck 1 2026"
    When I navigate to the training sessions list
    Then "PE Training Session 1" should display "Roebuck 1 2026" as a badge in the cohorts column

  # ─── ASSIGNING MENTORS ───────────────────────────────────────────────────────

  Scenario: Admin assigns a mentor to a cohort
    Given cohort "Roebuck 1 2026" is "Active"
    When I assign mentor "Bondi" to cohort "Roebuck 1 2026"
    Then mentor "Bondi" should be listed as a mentor for that cohort

  Scenario: Mentor is assigned to multiple cohorts
    Given mentor "Bondi" is assigned to cohort "Orpen Gate Village 2023"
    When I also assign mentor "Bondi" to cohort "Roebuck 1 2026"
    Then mentor "Bondi" should be assigned to both cohorts
    And they should see growers from both cohorts in their grower list

  # ─── VIEWING AND FILTERING ───────────────────────────────────────────────────

  Scenario: Admin views all cohorts
    Given the following cohorts exist:
      | Cohort Name              | Village            | Status    | Households |
      | Orpen Gate Village 2023  | Orpen Gate Village | Completed | 153        |
      | Orpen Gate Village 2024  | Orpen Gate Village | Active    | 57         |
      | Roebuck 1 2026           | Roebuck            | Planned   | 55         |
    When I navigate to cohort management
    Then I should see all three cohorts with their status and household counts

  Scenario: Admin filters grower list by cohort
    Given grower "Nomsa Dlamini" is a member of cohort "Orpen Gate Village 2024" only
    When I select cohort "Orpen Gate Village 2024" from the filter
    Then "Nomsa Dlamini" should appear in the results
    And growers who are not members of that cohort should not appear

  Scenario: Mentor sees growers from all their assigned cohorts
    Given mentor "Bondi" is assigned to cohorts "Roebuck 1 2026" and "Orpen Gate Village 2024"
    When mentor "Bondi" views the grower list
    Then they should see growers from both cohorts
    And they should not see growers from cohorts they are not assigned to

  # ─── COHORT TAGS ON RELATED SCREENS ─────────────────────────────────────────

  Scenario: Admin sees cohort tags on grower status screen
    Given grower "Nomsa Dlamini" is a member of cohort "Roebuck 1 2026"
    When I navigate to grower "Nomsa Dlamini"'s status screen
    Then I should see a "Cohorts" section
    And "Roebuck 1 2026" should appear as a badge

  Scenario: Admin removes a cohort tag from a grower status screen with confirmation
    Given grower "Nomsa Dlamini" is a member of cohort "Roebuck 1 2026"
    When I am on the grower status screen and click the remove button on the "Roebuck 1 2026" tag
    Then I should see an inline confirmation "Remove Roebuck 1 2026? Yes / No"
    When I click "Yes"
    Then "Nomsa Dlamini" should no longer be a member of "Roebuck 1 2026"

  Scenario: Admin adds a cohort to a grower from the grower status screen
    Given grower "Nomsa Dlamini" is not yet in any cohort
    And cohort "Roebuck 1 2026" is "Active" for village "Roebuck"
    When I navigate to the grower status screen
    And I select "Roebuck 1 2026" from the "Add cohort" dropdown
    Then "Nomsa Dlamini" should be a member of "Roebuck 1 2026"

  Scenario: Mentor can view cohort tags on grower status screen but cannot add or remove
    Given I am logged in as tree mentor "Bondi"
    And grower "Nomsa Dlamini" is a member of cohort "Roebuck 1 2026"
    When I navigate to grower "Nomsa Dlamini"'s status screen
    Then I should see "Roebuck 1 2026" as a badge
    And I should not see a remove button on the cohort tag
    And I should not see an "Add cohort" dropdown

  Scenario: Completed cohorts are excluded from the add-cohort dropdown
    Given cohort "Orpen Gate Village 2023" has status "Completed"
    And cohort "Orpen Gate Village 2024" has status "Active"
    When I open the "Add cohort" dropdown for a grower
    Then I should see "Orpen Gate Village 2024"
    And I should not see "Orpen Gate Village 2023"

  # ─── COHORT SUMMARY ──────────────────────────────────────────────────────────

  Scenario: Admin views cohort summary
    Given cohort "Roebuck 1 2026" has 55 grower members
    When I view the cohort summary for "Roebuck 1 2026"
    Then I should see the member count "55"
    And I should see the cohort status
    And I should see the enrollment date range
    And I should see the list of assigned tree mentors

  # ─── ASSESSMENT FREQUENCY ────────────────────────────────────────────────────

  Scenario: Assessment frequency is based on the cohort's age, not the grower's oldest cohort
    Given cohort "Roebuck 1 2026" was activated in 2026
    And grower "Nomsa Dlamini" is a member of "Roebuck 1 2026"
    When I view her assessment schedule for cohort "Roebuck 1 2026"
    Then the frequency should be "twice monthly"

  Scenario: Completed cohort growers retain monthly frequency
    Given cohort "Orpen Gate Village 2023" was activated in 2023 and is now "Completed"
    And grower "Sipho Nkosi" is a member of "Orpen Gate Village 2023"
    When I view his assessment schedule for cohort "Orpen Gate Village 2023"
    Then the frequency should be "monthly"

  # ─── REPORTING ───────────────────────────────────────────────────────────────

  Scenario: Report can be scoped to a specific cohort
    Given cohort "Roebuck 1 2026" has 55 member households
    When I generate a program report scoped to cohort "Roebuck 1 2026"
    Then the report results should reflect only those 55 households

  Scenario: Report shows visit counts per cohort for funder reporting
    Given cohort "Roebuck 1 2026" exists
    And 25 home visits were recorded for growers in that cohort in March 2026
    When I generate a visit summary report for "Roebuck 1 2026" in March 2026
    Then I should see a total of 25 home visits
