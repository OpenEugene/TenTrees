@workflow-cohort @priority-high
Feature: Cohort Management
  As a 10 Trees Admin
  I want to organise growers into named cohorts within a village
  So that I can track program phases, run cohort-scoped reports, and manage each group independently

  Background:
    Given the following villages exist: "Orpen Gate Village", "Roebuck"

  # ─── CREATING COHORTS ────────────────────────────────────────────────────────

  Scenario: System auto-suggests a cohort name from village and year
    Given I am a 10 Trees Admin
    When I create a new cohort for village "Roebuck" in year "2026"
    Then the system should suggest the name "Roebuck 2026"
    And I should be able to accept or overwrite the suggested name

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

  # ─── ASSIGNING GROWERS TO A COHORT ───────────────────────────────────────────

  Scenario: Admin assigns a grower to a cohort at enrollment
    Given cohort "Roebuck 1 2026" exists for village "Roebuck"
    When I enroll grower "Nomsa Dlamini" into "Roebuck"
    Then I should be prompted to select a cohort
    And I select "Roebuck 1 2026"
    Then grower "Nomsa Dlamini" should belong to cohort "Roebuck 1 2026"

  Scenario: Admin moves a grower to a different cohort
    Given grower "Sipho Nkosi" belongs to cohort "Orpen Gate Village 2023"
    When I edit the grower record and change their cohort to "Orpen Gate Village 2024"
    Then grower "Sipho Nkosi" should belong to cohort "Orpen Gate Village 2024"
    And they should no longer appear in "Orpen Gate Village 2023"

  # ─── VIEWING AND FILTERING ───────────────────────────────────────────────────

  Scenario: Admin views all cohorts
    Given the following cohorts exist:
      | Cohort Name              | Village            | Households |
      | Orpen Gate Village 2023  | Orpen Gate Village | 153        |
      | Orpen Gate Village 2024  | Orpen Gate Village | 57         |
      | Roebuck 1 2026           | Roebuck            | 55         |
    When I navigate to cohort management
    Then I should see all three cohorts listed with their household counts

  Scenario: Admin filters grower list by cohort
    Given multiple cohorts exist for "Orpen Gate Village"
    When I select cohort "Orpen Gate Village 2024" from the filter
    Then I should see only the 57 growers in that cohort
    And growers from other cohorts should not appear

  Scenario: Mentor sees only their assigned cohort's growers
    Given mentor "Bondi" is assigned to cohort "Roebuck 1 2026"
    When mentor "Bondi" views the grower list
    Then they should see only growers in "Roebuck 1 2026"
    And they should not see growers from other cohorts

  # ─── COHORT SUMMARY ──────────────────────────────────────────────────────────

  Scenario: Admin views cohort summary
    Given cohort "Roebuck 1 2026" exists with 55 growers
    When I view the cohort summary for "Roebuck 1 2026"
    Then I should see the household count "55"
    And I should see the enrollment date range
    And I should see the list of assigned tree mentors

  # ─── ASSESSMENT FREQUENCY ────────────────────────────────────────────────────

  Scenario: Year-1 cohort growers receive twice-monthly garden assessments
    Given grower "Nomsa Dlamini" was enrolled in cohort "Roebuck 1 2026" this year
    When I view her assessment schedule
    Then her assessment frequency should be "twice monthly"

  Scenario: Year-2 cohort growers receive monthly garden assessments
    Given grower "Sipho Nkosi" was enrolled in cohort "Orpen Gate Village 2023" two years ago
    When I view his assessment schedule
    Then his assessment frequency should be "monthly"

  # ─── REPORTING ───────────────────────────────────────────────────────────────

  Scenario: Report can be scoped to a specific cohort
    Given cohort "Roebuck 1 2026" exists with 55 households
    When I generate a program report
    And I select cohort "Roebuck 1 2026" as the scope
    Then the report results should reflect only those 55 households

  Scenario: Report shows visit counts per cohort for funder reporting
    Given cohort "Roebuck 1 2026" exists
    And 25 home visits were recorded for growers in that cohort in March 2026
    When I generate a visit summary report for "Roebuck 1 2026" in March 2026
    Then I should see a total of 25 home visits
