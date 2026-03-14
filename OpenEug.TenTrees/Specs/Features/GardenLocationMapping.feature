@workflow-mapping @priority-high @mobile @gps
Feature: Garden Location and Resource Documentation
  As a tree mentor
  I want to document garden locations and existing resources
  So that the program can track garden sites and plan tree distribution

  Background:
    Given I am a tree mentor with an approved enrollment
    And I am at the grower's garden site

  Scenario: Complete garden mapping with GPS
    Given I navigate to mapping for "Mary Nkuna"
    Then grower information should be auto-filled
    When I capture GPS coordinates
    And I answer water availability questions:
      | Question                                             | Response |
      | Do you have water in the plot?                       | Yes      |
      | Is there any water catchment system (Jojo tank)?     | Yes      |
    And I record existing trees:
      | Type                              | Count |
      | Existing trees/productive plants  | 5     |
      | Indigenous trees                  | 2     |
      | Fruit and nut trees               | 3     |
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
      | Field            | Value              |
      | Grower name      | Mary Nkuna         |
      | House number     | 42                 |
      | Village          | Orpen Gate Village |
