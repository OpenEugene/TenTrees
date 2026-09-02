# Program Reporting

Centre staff (10 Trees Admin or Project Manager) generate reports and export data to track program outcomes and report to funders. Translated from the retired feature file `Specs/Features/ProgramReporting.feature` (high priority, staff only).

## Reports

- **Tree Survival Rate** — filtered by village and date range; shows total trees planted, total alive, and the survival rate percentage.
- **Permaculture Practices** — per-village percentages for each practice (making compost, collecting water, using greywater, no chemical fertilizers, no pesticides) plus a callout of areas needing improvement (the lowest-scoring practice).
- **Monthly Village Report** — for a chosen month and village, includes tree survival rate and trend, new enrollment count, completed assessment count, permaculture compliance percentages, and identified areas for improvement.
- **Home Visits** — for a date range, shows total home visits per staff member (either filtered to one staff member or one row per staff member); results are exportable. Visit recording is described in [Garden Assessment](/Specifications/Garden-Assessment).
- **Cohort Comparison** — lists cohorts with household counts, with survival and compliance metrics filterable by cohort. See [Cohort Management](/Specifications/Cohort-Management).

## Data export

- **Export to Excel** downloads a `.xlsx` containing all currently filtered records, with columns matching the data grid.
- **Export to CSV** downloads a `.csv` compatible with Excel import.

Exports are language-neutral regardless of the language a form was submitted in — see [Data Entry Language Independence](/Specifications/Localization/Data-Entry-Language-Independence).

## Scenarios (Gherkin)

The original scenarios, preserved verbatim from the retired `Specs/Features/ProgramReporting.feature` as the precise acceptance criteria for the behaviour described above.

```gherkin
@workflow-reporting @priority-high @staff-only
Feature: Program Reports and Data Export
  As Centre staff
  I want to generate reports and export data
  So that I can track program outcomes and report to funders

  Background:
    Given I am Centre staff (10 Trees Admin or Project Manager)
    And I have access to reporting functions

  Scenario: Generate tree survival report
    When I select report "Tree Survival Rate"
    And I filter by village "Orpen Gate Village"
    And I set date range "2025-10-01" to "2025-10-31"
    Then I should see:
      | Metric              | Value |
      | Total trees planted | 500   |
      | Total trees alive   | 455   |
      | Survival rate       | 91%   |

  Scenario: Generate permaculture compliance report
    When I select report "Permaculture Practices"
    And I filter by village "Orpen Gate Village"
    Then I should see percentage using each practice:
      | Practice                | Percentage |
      | Making compost          | 85%        |
      | Collecting water        | 90%        |
      | Using greywater         | 75%        |
      | No chemical fertilizers | 95%        |
      | No pesticides           | 92%        |
    And I should see "Areas needing improvement: Using greywater"

  Scenario: Export data to Excel
    Given I have filtered data by village "Orpen Gate Village"
    And date range "Last 30 days"
    When I click "Export to Excel"
    Then a .xlsx file should download
    And it should contain all filtered records
    And columns should match the data grid

  Scenario: Export data to CSV
    Given I have filtered assessment data
    When I click "Export to CSV"
    Then a .csv file should download
    And it should be compatible with Excel import

  Scenario: Generate monthly village report
    When I generate monthly report for "November 2025"
    And village "Orpen Gate Village"
    Then report should include:
      | Section                    | Content                   |
      | Tree Survival              | Rate and trend            |
      | New Enrollments            | Count this month          |
      | Active Assessments         | Count completed           |
      | Permaculture Compliance    | Practice percentages      |
      | Areas for Improvement      | Identified gaps           |

  Scenario: Generate home visit count report for funders
    When I select report "Home Visits"
    And I filter by staff member "Joel"
    And I set date range "2025-03-01" to "2025-03-31"
    Then I should see:
      | Metric                | Value |
      | Staff member          | Joel  |
      | Total home visits     | 25    |
    And I should be able to export the results

  Scenario: Generate home visit report for all staff in a month
    When I select report "Home Visits"
    And I set date range "2025-03-01" to "2025-03-31"
    Then I should see a row per staff member showing their visit count for that month

  Scenario: Generate cohort comparison report
    When I select report "Cohort Comparison"
    Then I should see a list of cohorts including:
      | Cohort Name              | Households |
      | Orpen Gate Village 2023  | 153        |
      | Open Gate Village 2024   | 57         |
      | Roebuck 1 2026           | 55         |
    And I can filter survival and compliance metrics by cohort
```
