@workflow-enrollment @priority-high @mobile
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
      | Question                                              | Response |
      | Are they currently enrolled in PE with a garden growing? | Yes      |
      | Are they a graduate of PE in the past?                | Yes      |
      | If so, is their garden planted and tended?            | Yes      |
      | Child headed household?                               | No       |
      | Woman headed household?                               | Yes      |
      | Empty or nearly empty yard?                           | No       |
    Then all criteria responses should be saved

  Scenario: Record commitment acknowledgments
    Given I am completing an enrollment
    When I record commitment responses:
      | Commitment                                                     | Acknowledged |
      | Committed to not using chemicals or pesticides                 | Yes          |
      | Committed to attend five permaculture training classes         | Yes          |
      | Committed not to cut trees                                     | Yes          |
      | Committed to standing for women and children with no abuse     | Yes          |
      | Agree to care for trees while away from home                   | Yes          |
      | Give permission for mentor to enter yard                       | Yes          |
    Then all commitments should be recorded
