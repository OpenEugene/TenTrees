@workflow-reporting @priority-high @staff-only
Feature: Program Reports and Data Export
  As Centre staff
  I want to generate reports and export data
  So that I can track program outcomes and report to funders

  Background:
    Given I am Centre staff (Admin, PM, or ED)
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
