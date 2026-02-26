@workflow-assessment @priority-high @mobile @recurring
Feature: Tree Monitoring and Garden Health Assessment
  As a tree mentor
  I want to regularly assess garden health and tree survival
  So that the program can track outcomes and identify problems early

  Background:
    Given I am a tree mentor assigned to households in my village
    And I am conducting a garden visit

  # ─── FORM LINKING ───────────────────────────────────────────────────────────

  Scenario: Assessment auto-fills grower details from application form
    Given "Mary Nkuna" has an approved application on file
    When I navigate to a new assessment for "Mary Nkuna"
    Then her name and village should be pre-populated
    And I should not be prompted to re-enter her house number or ID

  # ─── TREE SURVIVAL ──────────────────────────────────────────────────────────

  Scenario: Auto-calculate tree survival rate
    Given I navigate to assessment for "Mary Nkuna"
    When I record "10" trees planted
    And I record "9" trees still alive
    Then the survival rate should display as "90%"

  Scenario: Record deceased trees via free text
    Given I have entered 10 trees planted and 8 alive
    When I am prompted "If any died, which ones?"
    And I enter free text "Mango, Avocado"
    Then the deceased tree description should be recorded

  Scenario: Record deceased trees via dropdown
    Given I have entered 10 trees planted and 8 alive
    When I am prompted "If any died, which ones?"
    And I select tree types from the dropdown
    Then the deceased tree types should be recorded

  # ─── PERMACULTURE PRACTICES ─────────────────────────────────────────────────

  Scenario: Record all permaculture practice responses and derive count
    Given I am completing an assessment for "Mary Nkuna"
    When I complete the permaculture practice questions:
      | Practice                                 | Response |
      | Do the trees look healthy?               | Yes      |
      | Any chemical fertilizers?                | No       |
      | Any pesticides used?                     | No       |
      | Are the trees mulched?                   | Yes      |
      | Are they making compost?                 | Yes      |
      | Are they collecting water?               | Yes      |
      | Any leaky taps visible?                  | No       |
      | Is the garden designed to capture water? | Yes      |
      | Are they using greywater?                | Yes      |
    Then each individual response should be saved
    And the total permaculture principles in use should be recorded as "5"
    And the principles not in use should be identifiable from the record

  # ─── PROBLEMS ───────────────────────────────────────────────────────────────

  Scenario: Record a single problem with help request
    Given I am completing an assessment
    When I select problem "The trees have yellow leaves"
    And I answer "Yes" to "Do you need someone to help with this problem?"
    Then the problem should be recorded
    And the help request flag should be set to true

  Scenario: Record multiple problems
    Given I am completing an assessment
    When I select multiple problems:
      | Problem                          |
      | The trees have broken branches   |
      | The trees have yellow leaves     |
      | The trees are losing their leaves|
      | The trees look dry               |
      | Pests eating the plant           |
    And I answer "Yes" to "Do you need someone to help with this problem?"
    Then all five problems should be recorded
    And the help request flag should be set to true

  Scenario: Complete assessment with no problems
    Given I am completing an assessment
    When I do not select any problems
    And I answer "No" to "Do you need someone to help with this problem?"
    Then the assessment should save with zero problems recorded
    And the help request flag should be set to false

  # ─── NOTES ──────────────────────────────────────────────────────────────────

  Scenario: Add narrative notes to assessment
    Given I am completing an assessment for "Mary Nkuna"
    When I enter notes "Grower has started a new compost heap near the fence"
    Then the notes should be saved with the assessment record
    And should be visible to centre staff in the report view

  # ─── SAVE & SYNC ────────────────────────────────────────────────────────────

  Scenario: Save assessment as draft without connectivity
    Given I have partially completed an assessment for "Mary Nkuna"
    And I do not have an internet connection
    When I tap "Save Draft"
    Then the draft should be stored locally on my device
    And I should see a "Draft saved" confirmation message

  Scenario: Sync draft assessment when Wi-Fi is available
    Given I have a saved draft assessment for "Mary Nkuna"
    And I am connected to Wi-Fi at the Centre
    When I open the draft and tap "Submit"
    Then the assessment should upload to the central database
    And I should see a "Assessment submitted" confirmation message

  Scenario: Show confirmation on successful submission
    Given I have completed all required fields for an assessment
    When I submit the assessment
    Then I should see a confirmation message "Your assessment has been saved"

  # ─── ASSESSMENT FREQUENCY ───────────────────────────────────────────────────

  Scenario: Accept assessment for Year 1 grower within twice-monthly schedule
    Given "Mary Nkuna" is in year 1 of the program
    And her last assessment was 14 days ago
    When I submit a new assessment
    Then the assessment should be accepted
    And the system should record the assessment as twice-monthly frequency

  Scenario: Accept assessment for Year 2 grower within monthly schedule
    Given "Grace Sithole" is in year 2 of the program
    And her last assessment was 30 days ago
    When I submit a new assessment
    Then the assessment should be accepted
    And the system should record the assessment as monthly frequency

  # ─── ACCESS CONTROL ─────────────────────────────────────────────────────────

  Scenario: Mentor can only assess their own assigned households
    Given I am tree mentor "Trygive" assigned to households in "Orpen Gate Village"
    When I attempt to navigate to an assessment for a grower assigned to a different mentor
    Then I should be denied access
    And I should see "You are not assigned to this household"

  Scenario: CentreAdmin submits assessment on behalf of a mentor
    Given I am logged in as a CentreAdmin
    And I am impersonating tree mentor "Trygive"
    When I complete and submit a garden assessment for "Mary Nkuna"
    Then the assessment should be saved
    And the record should show the submitting mentor as "Trygive"
    And the record should note it was entered by an admin

  Scenario: Centre staff can view but not edit a submitted assessment
    Given I am logged in as an Educator
    When I navigate to a submitted assessment for "Mary Nkuna"
    Then I should be able to view all assessment data
    And I should not see an edit option
