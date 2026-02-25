@workflow-enrollment @priority-high @mobile
Feature: Grower Enrollment Submission
  As a tree mentor
  I want to submit grower enrollments digitally
  So that enrollment data is captured accurately and linked to growers

  Background:
    Given I am a tree mentor logged into the system
    And I am at a grower's household

  Scenario: Submit new grower enrollment
    Given I navigate to "New Enrollment"
    When I select the village "Orpen Gate Village"
    And I enter the grower name "Mary Nkuna"
    And I enter house number "42"
    And I enter ID number or birthdate
    And I record household size as "5"
    And I answer "Yes" to "Do they own their home?"
    And I complete all preferred criteria questions
    And I record all commitment acknowledgments
    And I draw my signature on the signature pad
    And I check the confirmation checkbox
    Then the enrollment should be saved
    And the signature should be stored as an SVG image
    And I should see confirmation "Enrollment saved successfully"

  Scenario: Validate required fields before submission
    Given I have started a new enrollment
    When I attempt to submit without grower name
    Then I should see validation error "Grower name is required"
    And the form should not be submitted

  Scenario: Default tree mentor to current logged-in user
    Given I am logged in as mentor "Bondi"
    When I start a new enrollment
    Then the tree mentor name dropdown should be pre-selected with "Bondi"
    And the mentor ID should store the username "bondi"
    And the date should be set to today

  Scenario: Select a different tree mentor from dropdown
    Given I am logged in as "Educator A"
    And I start a new enrollment
    When I open the tree mentor name dropdown
    Then I should see a list of registered site users
    When I select "Bondi" from the tree mentor name dropdown
    Then the tree mentor name should display "Bondi"
    And the mentor ID should store the username "bondi"

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

  Scenario: Capture finger-drawn signature on canvas
    Given I have completed steps 1 through 3 of the enrollment
    When I reach the signature step
    Then I should see a signature canvas with a "Sign here" prompt
    And I should see a "Clear" button
    When I draw my signature on the canvas using touch
    Then the canvas should no longer appear blank
    When I check the confirmation checkbox
    And I tap "Submit"
    Then the signature should be saved as an SVG string in SignatureData
    And SignatureCollected should be true
    And SignatureDate should be set to today

  Scenario: Prevent submission with blank signature
    Given I have completed steps 1 through 3 of the enrollment
    And I am on the signature step
    When I check the confirmation checkbox without drawing a signature
    And I tap "Submit"
    Then I should see validation error "Signature is required"
    And the enrollment should not be saved

  Scenario: Clear signature and redraw
    Given I am on the signature step
    When I draw my signature on the canvas
    And I tap "Clear"
    Then the canvas should be blank
    And I should be able to draw a new signature

  Scenario: Signature canvas works on mobile touch device
    Given I am using a touch-screen smartphone
    And I am on the signature step
    When I press my finger on the canvas and drag to draw
    Then the signature line should follow my finger
    And the page should not scroll while I am drawing on the canvas
