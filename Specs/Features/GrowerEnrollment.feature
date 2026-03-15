@workflow-enrollment @priority-high @mobile
Feature: Grower Enrollment Submission
  As a tree mentor
  I want to submit grower enrollments digitally
  So that enrollment data is captured accurately and linked to growers

  Background:
    Given I am a tree mentor logged into the system
    And I am at a grower's household

  # ─── ENROLLMENT LIST ────────────────────────────────────────────────────────

  Scenario: View enrollment list with status summary dashboard
    Given I navigate to the enrollments list
    Then I should see four summary cards:
      | Card     | Colour  |
      | Pending  | Yellow  |
      | Approved | Green   |
      | Rejected | Red     |
      | Total    | Blue    |
    And each card should display the count of enrollments in that state

  Scenario: Enrollment list shows both enrollment status and grower status
    Given enrollments exist with varying states
    When I view the enrollment list
    Then each row should show an "Enrollment Status" badge (Pending / Approved / Rejected)
    And each row should show a "Grower Status" badge (Active / Inactive / Exited)
    And rows with Pending enrollment status should be highlighted in yellow
    And rows with Rejected enrollment status should be highlighted in red

  Scenario: Filter enrollment list by enrollment status
    Given I am viewing the enrollment list
    When I select status filter "Pending"
    Then I should only see enrollments with status "Pending"
    When I select status filter "Approved"
    Then I should only see enrollments with status "Approved"

  Scenario: Filter enrollment list by village
    Given I am viewing the enrollment list as admin
    When I select village "Orpen Gate Village" from the village filter
    Then I should only see enrollments from "Orpen Gate Village"

  Scenario: Clear filters to restore full enrollment list
    Given I have applied a status filter and a village filter
    When I click "Clear Filters"
    Then both filters should reset
    And all enrollments should be visible again

  # ─── ENROLLMENT CREATION ────────────────────────────────────────────────────

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

  # ─── COHORT SELECTION AT ENROLLMENT ─────────────────────────────────────────

  Scenario: Cohort picker appears when active cohorts exist for the selected village
    Given cohort "Roebuck 1 2026" is "Active" for village "Roebuck"
    When I select village "Roebuck" during enrollment
    Then a cohort picker should appear
    And "Roebuck 1 2026" should be listed as an option
    And a "none" option should also be available

  Scenario: Cohort picker is hidden when no active cohorts exist for the village
    Given no active cohorts exist for village "Londelozzi"
    When I select village "Londelozzi" during enrollment
    Then the cohort picker should not be shown

  Scenario: Enrolling without selecting a cohort is permitted
    Given I am completing a new enrollment for village "Roebuck"
    And cohort "Roebuck 1 2026" is available
    When I leave the cohort picker on "none"
    And I complete and submit the enrollment
    Then the enrollment should be saved
    And no cohort membership should be created for the grower

  Scenario: Grower is added to selected cohort when enrollment is approved
    Given I enrolled "Nomsa Dlamini" with cohort "Roebuck 1 2026" selected
    When an admin approves the enrollment
    Then a GrowerCohort record should be created for "Nomsa Dlamini" in "Roebuck 1 2026"

  # ─── ADMIN MAINTENANCE ──────────────────────────────────────────────────────

  Scenario: Admin backfills grower records from existing enrollments
    Given I am logged in as administrator
    And some approved enrollments do not yet have linked Grower records
    When I trigger "Backfill Growers" from the admin panel
    Then a Grower record should be created for each approved enrollment that lacked one
    And the count of newly created Grower records should be returned
