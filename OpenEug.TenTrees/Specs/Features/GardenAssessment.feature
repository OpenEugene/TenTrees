@workflow-assessment @priority-high @mobile @recurring
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
      | Practice                                     | Response |
      | Do the trees look healthy?                   | Yes      |
      | Any chemical fertilizers?                    | No       |
      | Any pesticides used?                         | No       |
      | Are the trees mulched?                       | Yes      |
      | Are they making compost?                     | Yes      |
      | Are they collecting water?                   | Yes      |
      | Any leaky taps visible?                      | No       |
      | Is the garden designed to capture water?     | Yes      |
      | Are they using greywater?                    | Yes      |
    Then the assessment should be saved with timestamp

  Scenario: Record deceased trees via dropdown
    Given I have entered 10 trees planted and 8 alive
    When I am prompted "If any died, which ones?"
    And I select tree types from the dropdown
    Then the deceased tree types should be recorded

  Scenario: Record deceased trees via free text
    Given I have entered 10 trees planted and 8 alive
    When I am prompted "If any died, which ones?"
    And I enter free text "Mango, Avocado"
    Then the deceased tree description should be recorded

  Scenario: Record multiple problems
    Given I am completing an assessment
    When I select multiple problems:
      | Problem                         |
      | The trees have broken branches  |
      | The trees have yellow leaves    |
      | Pests eating the plant          |
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
