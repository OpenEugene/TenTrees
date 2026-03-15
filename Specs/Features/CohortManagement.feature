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
    Then I should be prompted to select one or more cohorts
    And I select "Roebuck 1 2026"
    Then grower "Nomsa Dlamini" should be a member of cohort "Roebuck 1 2026"

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
