# 10 Trees Digital Platform - BDD Project Plan

**Oqtane Application**  
**Prepared for:** Open Eugene / Zingela Ulwazi Trust  
**Principal Consultant:** Mark Davis  
**Version:** 1.0  
**Date:** January 2026

---

## Table of Contents

1. [Executive Summary](#1-executive-summary)
2. [Workflow Inventory](#2-workflow-inventory)
3. [BDD Feature Definitions](#3-bdd-feature-definitions)
4. [Workflow-to-Module Mapping](#4-workflow-to-module-mapping)
5. [Module Actions & Behaviors](#5-module-actions--behaviors)
6. [Claude Project Structure](#6-claude-project-structure)
7. [GitHub Copilot Collaboration Pattern](#7-github-copilot-collaboration-pattern)
8. [Traceability Matrix](#8-traceability-matrix)
9. [Implementation Roadmap](#9-implementation-roadmap)
10. [Appendix: BDD Tag Reference](#appendix-a-bdd-tag-reference)

---

## 1. Executive Summary

The 10 Trees Digital Platform is a web-based application built on the Oqtane framework to support the 10 Trees program operated by Zingela Ulwazi Trust in rural South Africa. The platform digitizes paper-based workflows for tracking beneficiary enrollments, garden mapping, tree monitoring, and program reporting.

This document provides a comprehensive Behavior-Driven Development (BDD) project plan that establishes complete traceability from business workflows through feature definitions to module implementations.

### 1.1 Key Stakeholders

- **Tree Mentors (17 total):** Field staff collecting data on smartphones
- **Centre Staff:** Admin, Project Manager, Educator, Executive Director
- **Beneficiaries:** Women in the community receiving trees and training
- **External Villages:** Future partner organizations using the 10 Trees brand

### 1.2 Technical Constraints

- Mobile-first design for small smartphones (Nokia, Samsung, off-brand)
- Unreliable network connectivity in rural areas
- Bilingual support: English and Xitsonga (Shangaan)
- Minimal text entry; maximize tick-box/Yes-No questions

---

## 2. Workflow Inventory

The following workflows have been identified from project documentation and stakeholder meetings. Each workflow is tagged for cross-reference throughout this document.

| ID | Workflow Name | Description |
|----|---------------|-------------|
| WF-001 | Participant Enrollment | Complete enrollment process for new beneficiaries |
| WF-002 | Release Form Capture | Capture photo release consent with preference options |
| WF-003 | Garden Site Mapping | Document garden location and existing resources |
| WF-004 | Garden Assessment | Ongoing monitoring of tree health and garden practices |
| WF-005 | Class Attendance | Track permaculture training participation |
| WF-006 | Program Exit | Record participant departure with reason |
| WF-007 | Village Data Management | Organize and filter data by village |
| WF-008 | Reporting & Export | Generate reports and export data for grants |
| WF-009 | User Administration | Manage mentors, staff access, impersonation |

---

## 3. BDD Feature Definitions

Features are organized by workflow and tagged for traceability. Each feature includes background context and scenarios with Given-When-Then steps.

### 3.1 Participant Enrollment (WF-001)

**Tags:** `@workflow-enrollment` `@priority-high` `@mobile`

```gherkin
Feature: Beneficiary Enrollment Submission
  As a tree mentor
  I want to submit beneficiary enrollments digitally
  So that enrollment data is captured accurately and linked to participants

  Background:
    Given I am a tree mentor logged into the system
    And I am at a beneficiary's household

  Scenario: Submit new beneficiary enrollment
    Given I navigate to "New Enrollment"
    When I select the village "Orpen Gate Village"
    And I enter the beneficiary name "Mary Nkuna"
    And I enter house number "42"
    And I enter ID number or birthdate
    And I record household size as "5"
    And I answer "Yes" to "Do they own their home?"
    And I complete all preferred criteria questions
    And I record all commitment acknowledgments
    And I capture the e-signature
    Then the enrollment should be saved
    And I should see confirmation "Enrollment saved successfully"

  Scenario: Validate required fields before submission
    Given I have started a new enrollment
    When I attempt to submit without beneficiary name
    Then I should see validation error "Beneficiary name is required"
    And the form should not be submitted

  Scenario: Auto-populate mentor information
    Given I am logged in as mentor "Bondi"
    When I start a new enrollment
    Then the evaluator name should be pre-filled as "Bondi"
    And the date should be set to today

  Scenario: Record preferred criteria responses
    Given I am completing an enrollment
    When I answer the preferred criteria questions:
      | Question | Response |
      | Are they currently enrolled in PE with a garden growing? | Yes |
      | Are they a graduate of PE in the past? | Yes |
      | If so, is their garden planted and tended? | Yes |
      | Child headed household? | No |
      | Woman headed household? | Yes |
      | Empty or nearly empty yard? | No |
    Then all criteria responses should be saved

  Scenario: Record commitment acknowledgments
    Given I am completing an enrollment
    When I record commitment responses:
      | Commitment | Acknowledged |
      | Committed to not using chemicals or pesticides | Yes |
      | Committed to attend five permaculture training classes | Yes |
      | Committed not to cut trees | Yes |
      | Committed to standing for women and children with no abuse | Yes |
      | Agree to care for trees while away from home | Yes |
      | Give permission for mentor to enter yard | Yes |
    Then all commitments should be recorded
```

### 3.2 Release Form Capture (WF-002)

**Tags:** `@workflow-release` `@priority-high` `@mobile`

```gherkin
Feature: Photo Release Consent
  As a tree mentor
  I want to capture photo release preferences
  So that the organization has proper consent for using participant photos

  Scenario: Capture release form with full consent
    Given I have an approved enrollment for "Mary Nkuna"
    When I navigate to the release form
    And I select "You may use my photo with my name identified"
    And I capture the signature
    Then the release form should be linked to the enrollment
    And the consent level should be "Full"

  Scenario: Capture release form with limited consent
    Given I have an approved enrollment
    When I select "You may use my picture in group photos without my name"
    And I capture the signature
    Then the consent level should be "Limited"

  Scenario: Capture release form with no consent
    Given I have an approved enrollment
    When I select "You may not use my photo at all"
    And I capture the signature
    Then the consent level should be "None"
```

### 3.3 Garden Site Mapping (WF-003)

**Tags:** `@workflow-mapping` `@priority-high` `@mobile` `@gps`

```gherkin
Feature: Garden Location and Resource Documentation
  As a tree mentor
  I want to document garden locations and existing resources
  So that the program can track garden sites and plan tree distribution

  Background:
    Given I am a tree mentor with an approved enrollment
    And I am at the beneficiary's garden site

  Scenario: Complete garden mapping with GPS
    Given I navigate to mapping for "Mary Nkuna"
    Then beneficiary information should be auto-filled
    When I capture GPS coordinates
    And I answer water availability questions:
      | Question | Response |
      | Do you have water in the plot? | Yes |
      | Is there any water catchment system (Jojo tank)? | Yes |
    And I record existing trees:
      | Type | Count |
      | Existing trees/productive plants | 5 |
      | Indigenous trees | 2 |
      | Fruit and nut trees | 3 |
    And I answer "Yes" to "Is there space for more trees?"
    And I answer "Yes" to "Is the property fenced?"
    And I answer "Yes" to "Are there resources like compost or mulch?"
    Then the mapping should be saved
    And it should be linked to the enrollment

  Scenario: Manual GPS entry by staff
    Given I am a staff member at the Centre
    And a mapping exists without GPS coordinates
    When I open the mapping record
    And I manually enter latitude "-24.5271"
    And I manually enter longitude "31.1367"
    Then the GPS location should be updated

  Scenario: Link mapping to existing enrollment
    Given enrollments exist for "Mary Nkuna" and "Grace Sithole"
    When I start a new mapping
    And I search for "Mary"
    Then I should see "Mary Nkuna" in results
    And selecting her should auto-fill:
      | Field | Value |
      | Beneficiary name | Mary Nkuna |
      | House number | 42 |
      | Village | Orpen Gate Village |
```

### 3.4 Garden Assessment (WF-004)

**Tags:** `@workflow-assessment` `@priority-high` `@mobile` `@recurring`

```gherkin
Feature: Tree Monitoring and Garden Health Assessment
  As a tree mentor
  I want to regularly assess garden health and tree survival
  So that the program can track outcomes and identify problems early

  Background:
    Given I am a tree mentor assigned to households
    And I am conducting a garden visit

  Scenario: Complete garden assessment with tree survival tracking
    Given I navigate to assessment for "Mary Nkuna"
    When I record "10" trees planted
    And I record "9" trees still alive
    Then the survival rate should display as "90%"
    When I select problem "The trees have yellow leaves"
    And I answer "Yes" to "Do you need help with this problem?"
    And I complete permaculture practice questions:
      | Practice | Response |
      | Do the trees look healthy? | Yes |
      | Any chemical fertilizers? | No |
      | Any pesticides used? | No |
      | Are the trees mulched? | Yes |
      | Are they making compost? | Yes |
      | Are they collecting water? | Yes |
      | Any leaky taps visible? | No |
      | Is the garden designed to capture water? | Yes |
      | Are they using greywater? | Yes |
    Then the assessment should be saved with timestamp

  Scenario: Record deceased trees
    Given I have entered 10 trees planted and 8 alive
    When I am prompted "If any died, which ones?"
    Then I should be able to select from tree type dropdown
    Or I should be able to enter free text "Mango, Avocado"

  Scenario: Record multiple problems
    Given I am completing an assessment
    When I select multiple problems:
      | Problem |
      | The trees have broken branches |
      | The trees have yellow leaves |
      | Pests eating the plant |
    And I answer "Yes" to "Do you need someone to help with this problem?"
    Then all problems should be recorded
    And help request flag should be set to true

  Scenario: Assessment frequency for Year 1 participant
    Given "Mary Nkuna" is in year 1 of the program
    And her last assessment was 10 days ago
    When I submit a new assessment
    Then the assessment should be accepted
    And the system should track that assessments are twice monthly

  Scenario: Assessment frequency for Year 2 participant
    Given "Grace Sithole" is in year 2 of the program
    And her last assessment was 20 days ago
    When I submit a new assessment
    Then the assessment should be accepted
    And the system should track that assessments are monthly
```

### 3.5 Class Attendance (WF-005)

**Tags:** `@workflow-attendance` `@priority-medium`

```gherkin
Feature: Permaculture Training Attendance Tracking
  As Centre staff
  I want to track class attendance for participants
  So that we know who is eligible to receive trees

  Scenario: Mark class attendance
    Given I am tracking attendance for "PE Training Session 3"
    When I mark "Mary Nkuna" as present
    Then her attendance count should increase to 3
    And her record should show "3 of 5 classes completed"

  Scenario: View attendance completion status
    Given I am viewing participant "Mary Nkuna"
    When I check her attendance record
    Then I should see classes attended: "5 of 5"
    And status should show "Eligible for trees"

  Scenario: Participant not eligible without required classes
    Given participant "Grace Sithole" has attended 3 of 5 classes
    When I check her eligibility status
    Then status should show "2 classes remaining"
    And she should not be marked as "Eligible for trees"
```

### 3.6 Program Exit (WF-006)

**Tags:** `@workflow-exit` `@priority-medium`

```gherkin
Feature: Participant Program Departure
  As Centre staff
  I want to record when participants leave the program
  So that we maintain accurate program statistics

  Scenario: Record program exit with reason
    Given participant "Grace Sithole" is active in the program
    When I navigate to her record
    And I select "Mark as left program"
    And I select reason "Moved away"
    And I enter exit date "2025-11-15"
    Then her status should change to "Exited"
    And exit reason should be recorded as "Moved away"
    And exit date should be recorded

  Scenario: Exit reasons are selectable options
    Given I am recording a program exit
    When I view the exit reason dropdown
    Then I should see predefined reasons:
      | Reason |
      | Moved away |
      | Deceased |
      | Voluntary withdrawal |
      | Non-compliance |
      | Other |

  Scenario: Exited participant excluded from active reports
    Given participant "Grace Sithole" has exited the program
    When I generate an "Active Participants" report
    Then "Grace Sithole" should not appear in results
```

### 3.7 Village Data Management (WF-007)

**Tags:** `@workflow-village` `@priority-high` `@multi-tenant`

```gherkin
Feature: Village-Scoped Data Access
  As a system administrator
  I want to organize data by village
  So that each village sees only their own data while admins see all

  Scenario: Mentor views village-specific beneficiaries
    Given I am mentor "Bondi" assigned to "Orpen Gate Village"
    When I view the beneficiary list
    Then I should only see beneficiaries in "Orpen Gate Village"
    And I should not see beneficiaries from "Londelozzi"

  Scenario: Admin views all villages
    Given I am logged in as administrator "Becky"
    When I view the beneficiary list
    Then I should see a village filter dropdown
    When I select "All Villages"
    Then I should see beneficiaries from all villages
    When I select "Orpen Gate Village"
    Then I should only see beneficiaries from "Orpen Gate Village"

  Scenario: Add new village
    Given I am an administrator
    When I navigate to village management
    And I add village "Londelozzi"
    And I set village contact information
    Then the village should be available for assignment
    And mentors can be assigned to it

  Scenario: Village data isolation
    Given "Orpen Gate Village" has 50 beneficiaries
    And "Londelozzi" has 30 beneficiaries
    When mentor from "Londelozzi" logs in
    Then they should see exactly 30 beneficiaries
    And no data from "Orpen Gate Village" should be visible
```

### 3.8 Reporting & Export (WF-008)

**Tags:** `@workflow-reporting` `@priority-high` `@staff-only`

```gherkin
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
      | Metric | Value |
      | Total trees planted | 500 |
      | Total trees alive | 455 |
      | Survival rate | 91% |

  Scenario: Generate permaculture compliance report
    When I select report "Permaculture Practices"
    And I filter by village "Orpen Gate Village"
    Then I should see percentage using each practice:
      | Practice | Percentage |
      | Making compost | 85% |
      | Collecting water | 90% |
      | Using greywater | 75% |
      | No chemical fertilizers | 95% |
      | No pesticides | 92% |
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
      | Section | Content |
      | Tree Survival | Rate and trend |
      | New Enrollments | Count this month |
      | Active Assessments | Count completed |
      | Permaculture Compliance | Practice percentages |
      | Areas for Improvement | Identified gaps |
```

### 3.9 User Administration (WF-009)

**Tags:** `@workflow-admin` `@priority-high` `@security`

```gherkin
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
      | Field | Value |
      | Action | Assessment Created |
      | Entered By | Becky |
      | On Behalf Of | Bondi |
      | Timestamp | [current time] |

  Scenario: Assign mentor to village
    Given I am an administrator
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
    Given I am an administrator
    When I create new user:
      | Field | Value |
      | Name | Thandi Nkosi |
      | Role | Mentor |
      | Village | Orpen Gate Village |
      | Email | thandi@example.com |
    Then the account should be created
    And login credentials should be generated

  Scenario: Staff role permissions
    Given the following roles exist:
      | Role | Can Submit Forms | Can View All Villages | Can Export | Can Manage Users |
      | Mentor | Yes | No | No | No |
      | Educator | Yes | Yes | Yes | No |
      | Project Manager | Yes | Yes | Yes | No |
      | Admin | Yes | Yes | Yes | Yes |
      | Executive Director | Yes | Yes | Yes | Yes |
    When a user logs in with role "Mentor"
    Then they should have permissions matching the Mentor row
```

---

## 4. Workflow-to-Module Mapping

Each workflow maps to one or more Oqtane modules. This mapping ensures complete coverage and identifies shared components.

| Workflow | Primary Module | Supporting Modules | Oqtane Components |
|----------|---------------|-------------------|-------------------|
| WF-001 Enrollment | TenTrees.Enrollment | TenTrees.Participant | EditForm, ListView |
| WF-002 Release | TenTrees.Release | TenTrees.Participant | EditForm |
| WF-003 Mapping | TenTrees.Mapping | TenTrees.Participant, GPS Service | EditForm, MapView |
| WF-004 Assessment | TenTrees.Assessment | TenTrees.Participant | EditForm, ListView |
| WF-005 Attendance | TenTrees.Attendance | TenTrees.Participant | Checklist, GridView |
| WF-006 Exit | TenTrees.Participant | - | EditForm, StatusUpdate |
| WF-007 Village | TenTrees.Village | All modules | Filter, MultiTenant |
| WF-008 Reporting | TenTrees.Reports | All data modules | ReportViewer, Export |
| WF-009 Admin | TenTrees.Admin | Oqtane.Users | UserManagement |

### Module Dependencies

```
TenTrees.Participant (Core entity - all modules depend on this)
    ├── TenTrees.Enrollment
    ├── TenTrees.Release
    ├── TenTrees.Mapping
    ├── TenTrees.Assessment
    └── TenTrees.Attendance

TenTrees.Village (Multi-tenant filter - applies to all)
    └── All modules filter by village

TenTrees.Reports (Aggregates from all)
    ├── TenTrees.Participant
    ├── TenTrees.Enrollment
    ├── TenTrees.Mapping
    ├── TenTrees.Assessment
    └── TenTrees.Attendance

TenTrees.Admin (Cross-cutting)
    ├── Oqtane.Users
    └── TenTrees.Village
```

---

## 5. Module Actions & Behaviors

Detailed breakdown of actions each module must support, derived from BDD features.

### 5.1 TenTrees.Participant Module

| Action | Behavior | Input | Output | Source |
|--------|----------|-------|--------|--------|
| `CreateParticipant` | Create new beneficiary record | ParticipantDto | ParticipantId | WF-001 |
| `GetParticipant` | Retrieve participant by ID | int id | ParticipantDto | All |
| `SearchParticipants` | Find by name, ID, village | SearchCriteria | List<ParticipantDto> | WF-003, WF-004 |
| `GetByVillage` | List all in village | int villageId | List<ParticipantDto> | WF-007 |
| `GetByMentor` | List assigned to mentor | int mentorId | List<ParticipantDto> | WF-004 |
| `UpdateStatus` | Change active/exited status | int id, Status | bool success | WF-006 |
| `RecordExitReason` | Store exit date and reason | int id, ExitDto | bool success | WF-006 |
| `GetLinkedRecords` | All forms for participant | int id | LinkedRecordsDto | WF-008 |

**Entity: Participant**
```csharp
public class Participant
{
    public int ParticipantId { get; set; }
    public int VillageId { get; set; }
    public int? MentorId { get; set; }
    public string BeneficiaryName { get; set; }
    public string HouseNumber { get; set; }
    public string IdNumber { get; set; }
    public DateTime? BirthDate { get; set; }
    public int HouseholdSize { get; set; }
    public bool OwnsHome { get; set; }
    public ParticipantStatus Status { get; set; }
    public DateTime? ExitDate { get; set; }
    public string ExitReason { get; set; }
    public DateTime CreatedOn { get; set; }
    public int CreatedBy { get; set; }
}
```

### 5.2 TenTrees.Enrollment Module

| Action | Behavior | Input | Output | Source |
|--------|----------|-------|--------|--------|
| `CreateEnrollment` | Submit new enrollment | EnrollmentDto | EnrollmentId | WF-001 |
| `ValidateRequired` | Check all required fields | EnrollmentDto | ValidationResult | WF-001 |
| `CaptureSignature` | Store e-signature | int appId, SignatureData | bool success | WF-001 |
| `AutoFillMentor` | Pre-populate mentor info | int userId | MentorInfo | WF-001 |
| `GetByStatus` | Filter by pending/approved | EnrollmentStatus | List<EnrollmentDto> | WF-008 |
| `GetByVillage` | Filter by village | int villageId | List<EnrollmentDto> | WF-007 |

**Entity: Enrollment**
```csharp
public class Enrollment
{
    public int EnrollmentId { get; set; }
    public int ParticipantId { get; set; }
    public int MentorId { get; set; }
    public DateTime EnrollmentDate { get; set; }
    
    // Preferred Criteria
    public bool EnrolledInPE { get; set; }
    public bool PEGraduate { get; set; }
    public bool GardenPlantedAndTended { get; set; }
    public bool ChildHeadedHousehold { get; set; }
    public bool WomanHeadedHousehold { get; set; }
    public bool EmptyYard { get; set; }
    
    // Commitments
    public bool CommitNoChemicals { get; set; }
    public bool CommitAttendClasses { get; set; }
    public bool CommitNoCuttingTrees { get; set; }
    public bool CommitStandForWomenChildren { get; set; }
    public bool CommitCareWhileAway { get; set; }
    public bool CommitAllowYardAccess { get; set; }
    
    public bool SignatureCollected { get; set; }
    public DateTime? SignatureDate { get; set; }
    public EnrollmentStatus Status { get; set; }
}
```

### 5.3 TenTrees.Mapping Module

| Action | Behavior | Input | Output | Source |
|--------|----------|-------|--------|--------|
| `CreateMapping` | Create garden mapping | MappingDto | MappingId | WF-003 |
| `CaptureGPS` | Store lat/long | int mappingId, GpsCoords | bool success | WF-003 |
| `UpdateGPS` | Staff updates GPS later | int mappingId, GpsCoords | bool success | WF-003 |
| `RecordResources` | Document water, trees, etc. | int mappingId, ResourcesDto | bool success | WF-003 |
| `LinkToParticipant` | Auto-fill from participant | int participantId | ParticipantInfo | WF-003 |
| `GetByParticipant` | Get mapping for participant | int participantId | MappingDto | WF-008 |

**Entity: GardenMapping**
```csharp
public class GardenMapping
{
    public int MappingId { get; set; }
    public int ParticipantId { get; set; }
    public int MentorId { get; set; }
    public DateTime MappingDate { get; set; }
    
    // Location
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public bool GpsAddedByStaff { get; set; }
    
    // Water
    public bool HasWaterInPlot { get; set; }
    public bool HasWaterCatchment { get; set; }
    
    // Existing Resources
    public int ExistingTreesCount { get; set; }
    public int IndigenousTreesCount { get; set; }
    public int FruitNutTreesCount { get; set; }
    public bool HasSpaceForMoreTrees { get; set; }
    public bool PropertyFenced { get; set; }
    public bool HasCompostOrMulch { get; set; }
}
```

### 5.4 TenTrees.Assessment Module

| Action | Behavior | Input | Output | Source |
|--------|----------|-------|--------|--------|
| `CreateAssessment` | Submit garden assessment | AssessmentDto | AssessmentId | WF-004 |
| `CalculateSurvival` | Compute survival rate | int planted, int alive | decimal rate | WF-004 |
| `RecordProblems` | Store problem checkboxes | int assessmentId, List<Problem> | bool success | WF-004 |
| `RecordDeadTrees` | Document which trees died | int assessmentId, string trees | bool success | WF-004 |
| `TrackPermaculture` | Record practice responses | int assessmentId, PracticesDto | bool success | WF-004 |
| `RequestHelp` | Flag for intervention | int assessmentId, bool needsHelp | bool success | WF-004 |
| `GetHistory` | All assessments for participant | int participantId | List<AssessmentDto> | WF-008 |
| `GetByDateRange` | Filter by date | DateTime start, DateTime end | List<AssessmentDto> | WF-008 |

**Entity: GardenAssessment**
```csharp
public class GardenAssessment
{
    public int AssessmentId { get; set; }
    public int ParticipantId { get; set; }
    public int MentorId { get; set; }
    public DateTime AssessmentDate { get; set; }
    
    // Tree Tracking
    public int TreesPlanted { get; set; }
    public int TreesAlive { get; set; }
    public decimal SurvivalRate { get; set; } // Calculated
    public string DeadTreesList { get; set; }
    
    // Permaculture Practices
    public bool TreesLookHealthy { get; set; }
    public bool UsesChemicalFertilizers { get; set; }
    public bool UsesPesticides { get; set; }
    public bool TreesMulched { get; set; }
    public bool MakingCompost { get; set; }
    public bool CollectingWater { get; set; }
    public bool LeakyTapsVisible { get; set; }
    public bool GardenCapturesWater { get; set; }
    public bool UsingGreywater { get; set; }
    
    // Problems
    public bool HasBrokenBranches { get; set; }
    public bool HasYellowLeaves { get; set; }
    public bool TreesLosingLeaves { get; set; }
    public bool TreesLookDry { get; set; }
    public bool PestsEatingPlant { get; set; }
    public bool NeedsHelpWithProblem { get; set; }
}
```

### 5.5 TenTrees.Reports Module

| Action | Behavior | Input | Output | Source |
|--------|----------|-------|--------|--------|
| `GenerateSurvivalReport` | Aggregate survival rates | ReportCriteria | SurvivalReportDto | WF-008 |
| `GeneratePermacultureReport` | Summarize practice compliance | ReportCriteria | PermacultureReportDto | WF-008 |
| `GenerateMonthlyReport` | Comprehensive monthly summary | int villageId, DateTime month | MonthlyReportDto | WF-008 |
| `ExportToExcel` | Generate .xlsx download | ExportCriteria | byte[] fileData | WF-008 |
| `ExportToCSV` | Generate .csv download | ExportCriteria | byte[] fileData | WF-008 |
| `GetReportList` | Available report types | - | List<ReportType> | WF-008 |

### 5.6 TenTrees.Admin Module

| Action | Behavior | Input | Output | Source |
|--------|----------|-------|--------|--------|
| `ImpersonateMentor` | Enter data as specific mentor | int adminId, int mentorId | ImpersonationContext | WF-009 |
| `EndImpersonation` | Return to admin context | - | bool success | WF-009 |
| `AssignMentorToVillage` | Set mentor's village scope | int mentorId, int villageId | bool success | WF-009 |
| `AssignHouseholds` | Link households to mentor | int mentorId, List<int> householdIds | bool success | WF-009 |
| `ManageVillages` | CRUD for villages | VillageDto | VillageId | WF-007 |
| `GetAuditLog` | Retrieve admin actions | AuditCriteria | List<AuditEntry> | WF-009 |
| `CreateAuditEntry` | Log admin action | AuditEntry | bool success | WF-009 |

### 5.7 TenTrees.Village Module

| Action | Behavior | Input | Output | Source |
|--------|----------|-------|--------|--------|
| `CreateVillage` | Add new village | VillageDto | VillageId | WF-007 |
| `GetVillage` | Retrieve village by ID | int villageId | VillageDto | All |
| `GetAllVillages` | List all villages | - | List<VillageDto> | WF-007 |
| `UpdateVillage` | Modify village info | VillageDto | bool success | WF-007 |
| `GetVillageStats` | Summary statistics | int villageId | VillageStatsDto | WF-008 |

---

## 6. Claude Project Structure

Recommended organization for managing the 10 Trees project within a Claude Project workspace.

### 6.1 Folder Structure

```
10-Trees-Project/
├── 00-Context/
│   ├── Project_Overview.md
│   ├── Stakeholder_Map.md
│   ├── Technical_Constraints.md
│   └── Glossary.md
│
├── 01-Workflows/
│   ├── WF-001_Enrollment.md
│   ├── WF-002_Release.md
│   ├── WF-003_Mapping.md
│   ├── WF-004_Assessment.md
│   ├── WF-005_Attendance.md
│   ├── WF-006_Exit.md
│   ├── WF-007_Village.md
│   ├── WF-008_Reporting.md
│   └── WF-009_Admin.md
│
├── 02-Features/
│   ├── enrollment/
│   │   ├── enrollment-submission.feature
│   │   ├── validation.feature
│   │   └── signature-capture.feature
│   ├── release/
│   │   └── photo-consent.feature
│   ├── mapping/
│   │   ├── garden-mapping.feature
│   │   └── gps-capture.feature
│   ├── assessment/
│   │   ├── tree-monitoring.feature
│   │   ├── survival-calculation.feature
│   │   └── permaculture-tracking.feature
│   ├── attendance/
│   │   └── class-tracking.feature
│   ├── exit/
│   │   └── program-departure.feature
│   ├── village/
│   │   └── data-scoping.feature
│   ├── reporting/
│   │   ├── survival-report.feature
│   │   ├── permaculture-report.feature
│   │   └── export.feature
│   ├── admin/
│   │   ├── impersonation.feature
│   │   └── user-management.feature
│
├── 03-Modules/
│   ├── TenTrees.Participant.md
│   ├── TenTrees.Enrollment.md
│   ├── TenTrees.Release.md
│   ├── TenTrees.Mapping.md
│   ├── TenTrees.Assessment.md
│   ├── TenTrees.Attendance.md
│   ├── TenTrees.Village.md
│   ├── TenTrees.Reports.md
│   └── TenTrees.Admin.md
│
├── 04-Planning/
│   ├── Sprint_Backlog.md
│   ├── Traceability_Matrix.md
│   ├── Risk_Register.md
│   └── Decision_Log.md
│
├── 05-Artifacts/
│   ├── Data_Model.md
│   ├── API_Contracts.md
│   ├── UI_Wireframes.md
│   └── Localization_Keys.md
│
└── 06-Reference/
    ├── Oqtane_Patterns.md
    ├── Blazor_Mobile_Guidelines.md
    └── Testing_Strategy.md
```

### 6.2 File Naming Conventions

- **Workflows:** `WF-NNN_WorkflowName.md` (e.g., `WF-001_Enrollment.md`)
- **Features:** `kebab-case.feature` (e.g., `enrollment-submission.feature`)
- **Modules:** `TenTrees.ModuleName.md` matching Oqtane module namespace
- **All files include header:** ID, Version, Last Updated, Status

### 6.3 Workflow Document Template

Each workflow file should contain:

```markdown
# WF-001: Participant Enrollment

## Metadata
- **ID:** WF-001
- **Version:** 1.0
- **Status:** Approved
- **Last Updated:** 2025-01-15

## Description
[Business value and purpose]

## Actors
- Primary: Tree Mentor
- Secondary: Centre Staff

## Preconditions
- Mentor is logged in
- Mentor is at beneficiary household

## Main Flow
1. Mentor navigates to New Enrollment
2. Mentor selects village
3. [continued steps...]

## Alternate Flows
### AF-1: Existing Participant
1. System detects duplicate
2. [steps...]

## Business Rules
- BR-001: All commitments must be acknowledged
- BR-002: Signature is required

## Linked Features
- @enrollment/enrollment-submission.feature
- @enrollment/validation.feature

## Linked Modules
- TenTrees.Enrollment
- TenTrees.Participant
```

### 6.4 Module Document Template

Each module file should contain:

```markdown
# TenTrees.Enrollment Module

## Overview
Handles beneficiary enrollment submission and management.

## Dependencies
- TenTrees.Participant (required)
- TenTrees.Village (required)

## Entities
### Enrollment
[Entity definition with properties]

## Actions
### CreateEnrollment
- **Input:** EnrollmentDto
- **Output:** int EnrollmentId
- **Validation:** [rules]
- **Errors:** [error codes]

## API Endpoints
| Method | Route | Action |
|--------|-------|--------|
| POST | /api/tentrees/enrollment | CreateEnrollment |
| GET | /api/tentrees/enrollment/{id} | GetEnrollment |

## UI Components
- EnrollmentEdit.razor
- EnrollmentList.razor

## Source Features
- WF-001: @enrollment
```

---

## 7. GitHub Copilot Collaboration Pattern

This section establishes a clear division of responsibilities between Claude (planning) and GitHub Copilot Agent Mode (coding).

### 7.1 Role Definitions

| Tool | Primary Responsibility | Artifacts Produced |
|------|----------------------|-------------------|
| **Claude** | Planning, BDD, architecture, refinement | Features, behaviors, acceptance criteria, API contracts, entity definitions |
| **GitHub Copilot** | Code generation, implementation, testing | C# code, Razor components, unit tests, migrations |

### 7.2 Workflow: Feature to Implementation

```
┌─────────────────────────────────────────────────────────────────┐
│                        CLAUDE PHASE                              │
├─────────────────────────────────────────────────────────────────┤
│ Step 1: Generate/Refine BDD Feature                             │
│   Input: Workflow description, stakeholder feedback             │
│   Output: Complete .feature file with scenarios                 │
│                                                                  │
│ Step 2: Produce Module Action Spec                              │
│   Input: Feature file, existing module definitions              │
│   Output: Action list with signatures, behaviors, edge cases    │
│                                                                  │
│ Step 3: Define Entity/DTO                                       │
│   Input: Action requirements                                    │
│   Output: C# entity class definition, validation rules          │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                    DEVELOPER HANDOFF                             │
├─────────────────────────────────────────────────────────────────┤
│ Copy artifacts to repo:                                         │
│   - /docs/features/[workflow]/[feature].feature                 │
│   - /docs/modules/[ModuleName].md                               │
│   - /docs/entities/[EntityName].md                              │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                     COPILOT PHASE                                │
├─────────────────────────────────────────────────────────────────┤
│ Step 4: Developer invokes Copilot Agent Mode                    │
│   Prompt: "Implement [Module].[Action] per spec in /docs/..."   │
│                                                                  │
│ Step 5: Copilot generates code                                  │
│   - Service method                                              │
│   - Repository implementation                                   │
│   - Controller endpoint                                         │
│   - Razor component                                             │
│   - Unit tests                                                  │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                    REVIEW & ITERATE                              │
├─────────────────────────────────────────────────────────────────┤
│ Step 6: Claude reviews implementation                           │
│   Developer pastes code to Claude                               │
│   Claude checks against BDD scenarios                           │
│   Claude identifies gaps or missing edge cases                  │
│                                                                  │
│ Step 7: Iterate until all scenarios pass                        │
└─────────────────────────────────────────────────────────────────┘
```

### 7.3 Copilot Prompt Templates

#### For New Module Scaffolding

```
Create Oqtane module TenTrees.Assessment following the Oqtane module pattern.

Include:
1. Server/Entities/GardenAssessment.cs - entity per /docs/entities/GardenAssessment.md
2. Server/Repository/IGardenAssessmentRepository.cs - interface
3. Server/Repository/GardenAssessmentRepository.cs - EF Core implementation
4. Server/Services/IGardenAssessmentService.cs - interface
5. Server/Services/GardenAssessmentService.cs - business logic
6. Server/Controllers/GardenAssessmentController.cs - API endpoints
7. Client/Services/GardenAssessmentService.cs - HTTP client
8. Client/Modules/TenTrees.Assessment/Index.razor - list view
9. Client/Modules/TenTrees.Assessment/Edit.razor - edit form

Follow existing TenTrees module patterns. Use dependency injection.
```

#### For Specific Action Implementation

```
Implement CalculateSurvival in TenTrees.Assessment.

Specification:
- Input: int treesPlanted, int treesAlive
- Output: decimal survivalRate (0.00 to 100.00)
- Handle: treesPlanted = 0 (return 0, not divide-by-zero)
- Handle: treesAlive > treesPlanted (return 100, log warning)
- Round to 2 decimal places

Add unit tests covering:
1. Normal calculation (10 planted, 9 alive = 90.00)
2. All alive (10 planted, 10 alive = 100.00)
3. None alive (10 planted, 0 alive = 0.00)
4. Zero planted (0 planted, 0 alive = 0.00)
5. Invalid data (10 planted, 15 alive = 100.00 with warning)
```

#### For Blazor Component

```
Create mobile-friendly Blazor form for Garden Assessment.

Requirements:
- Fields from /docs/entities/GardenAssessment.md
- Bootstrap 5 responsive layout
- Tick-box (checkbox) inputs for Yes/No questions
- Number inputs for tree counts
- Auto-calculate survival rate on blur
- Bilingual labels using IStringLocalizer
- Validation per /docs/modules/TenTrees.Assessment.md
- Submit calls GardenAssessmentService.CreateAssessment()
- Show confirmation message on success
- Mobile-first: stack all fields vertically
- Large touch targets (min 44px)
```

#### For Unit Test Generation

```
Generate xUnit tests for GardenAssessmentService.

Test all methods in /docs/modules/TenTrees.Assessment.md:
- CreateAssessment: valid input, missing required fields, invalid participant
- CalculateSurvival: all edge cases listed in spec
- RecordProblems: single problem, multiple problems, no problems
- GetHistory: participant with assessments, participant without assessments

Use Moq for repository mocking. Follow AAA pattern (Arrange, Act, Assert).
```

### 7.4 Handoff Checklist

Before handing a feature to Copilot, ensure Claude has produced:

- [ ] Complete `.feature` file with all scenarios
- [ ] Module action spec with method signatures
- [ ] Entity/DTO definitions with all properties
- [ ] Validation rules for each field
- [ ] Error codes and handling expectations
- [ ] Any Oqtane-specific patterns to follow
- [ ] Localization key list for bilingual labels
- [ ] API endpoint routes

### 7.5 Review Checklist

After Copilot generates code, verify with Claude:

- [ ] All BDD scenarios have corresponding implementation
- [ ] Edge cases from spec are handled
- [ ] Validation rules match spec
- [ ] Error handling is complete
- [ ] Mobile responsiveness requirements met
- [ ] Localization is implemented
- [ ] Unit tests cover all scenarios

---

## 8. Traceability Matrix

Complete mapping from workflow to feature to module action.

| Workflow | Feature Tag | Scenario | Module | Action | Priority |
|----------|-------------|----------|--------|--------|----------|
| WF-001 | @enrollment | Submit new enrollment | Enrollment | CreateEnrollment | High |
| WF-001 | @enrollment | Validate required fields | Enrollment | ValidateRequired | High |
| WF-001 | @enrollment | Auto-populate mentor | Enrollment | AutoFillMentor | High |
| WF-001 | @enrollment | Record criteria | Enrollment | CreateEnrollment | High |
| WF-001 | @enrollment | Record commitments | Enrollment | CreateEnrollment | High |
| WF-001 | @enrollment | Capture signature | Enrollment | CaptureSignature | High |
| WF-002 | @release | Full consent | Release | CaptureConsent | High |
| WF-002 | @release | Limited consent | Release | CaptureConsent | High |
| WF-002 | @release | No consent | Release | CaptureConsent | High |
| WF-003 | @mapping | Complete mapping with GPS | Mapping | CreateMapping | High |
| WF-003 | @mapping | Capture GPS | Mapping | CaptureGPS | High |
| WF-003 | @mapping | Manual GPS by staff | Mapping | UpdateGPS | High |
| WF-003 | @mapping | Link to participant | Mapping | LinkToParticipant | High |
| WF-004 | @assessment | Complete assessment | Assessment | CreateAssessment | High |
| WF-004 | @assessment | Calculate survival | Assessment | CalculateSurvival | High |
| WF-004 | @assessment | Record problems | Assessment | RecordProblems | High |
| WF-004 | @assessment | Record dead trees | Assessment | RecordDeadTrees | High |
| WF-004 | @assessment | Track permaculture | Assessment | TrackPermaculture | High |
| WF-004 | @assessment | Request help | Assessment | RequestHelp | High |
| WF-005 | @attendance | Mark attendance | Attendance | MarkAttendance | Medium |
| WF-005 | @attendance | View completion status | Attendance | GetAttendanceStatus | Medium |
| WF-006 | @exit | Record exit | Participant | RecordExitReason | Medium |
| WF-006 | @exit | Exit reasons dropdown | Participant | GetExitReasons | Medium |
| WF-007 | @village | Mentor village scope | Village | FilterByVillage | High |
| WF-007 | @village | Admin all villages | Village | GetAllVillages | High |
| WF-007 | @village | Add new village | Village | CreateVillage | High |
| WF-008 | @reporting | Survival report | Reports | GenerateSurvivalReport | High |
| WF-008 | @reporting | Permaculture report | Reports | GeneratePermacultureReport | High |
| WF-008 | @reporting | Monthly report | Reports | GenerateMonthlyReport | High |
| WF-008 | @reporting | Export Excel | Reports | ExportToExcel | High |
| WF-008 | @reporting | Export CSV | Reports | ExportToCSV | High |
| WF-009 | @admin | Impersonate mentor | Admin | ImpersonateMentor | High |
| WF-009 | @admin | Assign mentor to village | Admin | AssignMentorToVillage | High |
| WF-009 | @admin | Assign households | Admin | AssignHouseholds | High |
| WF-009 | @admin | Audit logging | Admin | CreateAuditEntry | High |

---

## 9. Implementation Roadmap

Phased approach aligned with the 10 Trees Work Plan timeline.

### 9.1 Phase 1: Core Forms (January - February 2026)

**Target:** Digital forms ready for testing by end of January

**Modules:**
- [ ] TenTrees.Participant - Core entity CRUD
- [ ] TenTrees.Enrollment - Full enrollment form
- [ ] TenTrees.Mapping - Garden documentation
- [ ] TenTrees.Assessment - Tree monitoring

**Key Deliverables:**
- Mobile-responsive UI for all forms
- Bilingual labels (English/Xitsonga)
- Basic validation
- Form submission and confirmation

**BDD Coverage:**
- WF-001: All enrollment scenarios
- WF-003: Core mapping scenarios
- WF-004: Core assessment scenarios

### 9.2 Phase 2: Data Organization (February 2026)

**Target:** Village-level data views and linked records

**Modules:**
- [ ] TenTrees.Village - Multi-tenant filtering
- [ ] TenTrees.Release - Photo consent capture

**Key Deliverables:**
- Village filter on all list views
- Linked record navigation (Enrollment → Mapping → Assessments)
- Basic summary statistics

**BDD Coverage:**
- WF-002: Release form scenarios
- WF-007: Village scoping scenarios

### 9.3 Phase 3: Training Launch (March 2026)

**Target:** Production deployment with trained staff

**Modules:**
- [ ] TenTrees.Admin - User management, impersonation
- [ ] TenTrees.Reports - Export functionality

**Key Deliverables:**
- User role management
- Mentor impersonation for data entry
- Excel/CSV export
- Basic reporting views
- Training documentation

**BDD Coverage:**
- WF-008: Reporting scenarios
- WF-009: Admin scenarios

### 9.4 Phase 4: Enhancement (April 2026+)

**Target:** Stability and optional features

**Modules:**
- [ ] TenTrees.Attendance - Class tracking (if not deferred to Excel)

**Key Deliverables:**
- Additional villages onboarding
- Photo upload integration (optional)
- Performance optimization
- Additional report types

**BDD Coverage:**
- WF-005: Attendance scenarios
- WF-006: Exit scenarios

### 9.5 Sprint Planning Template

```markdown
## Sprint [N]: [Date Range]

### Goals
- [ ] Goal 1
- [ ] Goal 2

### Features
| Feature | Scenarios | Module | Points |
|---------|-----------|--------|--------|
| | | | |

### Tasks
- [ ] Task 1 (assignee)
- [ ] Task 2 (assignee)

### Risks
- Risk 1: Mitigation

### Definition of Done
- [ ] All scenarios pass
- [ ] Code reviewed
- [ ] Mobile tested
- [ ] Bilingual labels complete
```

### 9.6 Risk Mitigation

| Risk | Impact | Mitigation |
|------|--------|------------|
| Device variety | Medium | Test on representative phones before each phase |
| Translation accuracy | Medium | Engage native Xitsonga speakers for review |
| Data entry burden | Medium | Minimize typing, maximize tick-boxes |
| User adoption | High | Hands-on training with real scenarios |
| Scope creep | Medium | Strict BDD-driven acceptance criteria |
| Network at villages | Medium | Mentors submit data at Centre with reliable Wi-Fi |

---

## Appendix A: BDD Tag Reference

| Tag | Description | Usage |
|-----|-------------|-------|
| `@workflow-[name]` | Links feature to workflow | `@workflow-enrollment` |
| `@priority-high` | Phase 1 implementation | Core functionality |
| `@priority-medium` | Phase 2-3 implementation | Important but not launch-critical |
| `@priority-low` | Phase 4 or optional | Enhancement features |
| `@mobile` | Requires mobile-optimized UI | All mentor-facing forms |
| `@staff-only` | Centre staff access only | Reports, admin functions |
| `@gps` | Uses device location | Mapping features |
| `@recurring` | Repeated data entry | Assessment tracking |
| `@multi-tenant` | Village-level scoping | Data isolation features |
| `@security` | Access control implications | Admin, impersonation |
| `@wip` | Work in progress | Not ready for implementation |
| `@manual` | Requires manual testing | Complex UI interactions |

---

## Appendix B: Glossary

| Term | Definition |
|------|------------|
| **Beneficiary** | Woman enrolled in the 10 Trees program receiving trees |
| **Tree Mentor** | Field staff who visits households and collects data |
| **Centre** | Zingela Ulwazi headquarters with Wi-Fi |
| **PE** | Permaculture Education program |
| **Village** | Geographic grouping of beneficiaries |
| **Assessment** | Regular garden health check (twice monthly Year 1, monthly Year 2) |
| **Survival Rate** | Trees alive ÷ Trees planted × 100 |
| **Impersonation** | Admin entering data on behalf of a mentor |

---

## Appendix C: Localization Keys

Key naming convention: `[Module].[Component].[Element]`

```
// Enrollment Form
Enrollment.Edit.Title = "10 Trees Enrollment Form" / "Fomo ya ku mepa ka 10 Trees"
Enrollment.Edit.BeneficiaryName = "Beneficiary's name" / "Vito ra mu amukeli"
Enrollment.Edit.HouseNumber = "House Number" / "Nomboro ya yindlu"
Enrollment.Edit.OwnsHome = "Do they own their home?" / "Xana kaya leri va tshamaku ka roho ira voho xi mfumo keh?"
Enrollment.Edit.HouseholdSize = "Number of people living in the home" / "Nhlayu ya vanhu lava va tshamaku ka ndyangu lowu"

// Commitments
Enrollment.Commitments.NoChemicals = "Are you committed to not using chemicals or pesticides?" / "Uti yimiserile ku byala unga tirhisi tikhemikhali?"
Enrollment.Commitments.AttendClasses = "Are you committed to attend five permaculture training classes?" / "Uti yimiserile ku ngena tlhanu wati dyodzo ta permaculture?"

// Assessment Form
Assessment.Edit.Title = "Garden Monitoring Form" / "Fomo ya ku kambela xirapa"
Assessment.Edit.TreesPlanted = "How many trees were planted?" / "Nhlayu ya misinya yi byariwile?"
Assessment.Edit.TreesAlive = "How many are still alive?" / "Nhlayu ya misinya yi ha hanyile?"
Assessment.Edit.SurvivalRate = "Survival rate" / "Mpimo wa ku hanya"

// Problems
Assessment.Problems.BrokenBranches = "The trees have broken branches" / "Misinya yi na mahlatsi ya tshukile"
Assessment.Problems.YellowLeaves = "The trees have yellow leaves" / "Misinya yi na marhavi ya xihlaza"
Assessment.Problems.NeedsHelp = "Do you need someone to come and help with a problem?" / "Xana u lava munhu a ta ku pfuna?"
```

---

*Document generated by Claude for Open Eugene / 10 Trees Project*
*For questions, contact Mark Davis, Principal Consultant*
