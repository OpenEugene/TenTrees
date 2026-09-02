# Garden Location Mapping

Tree mentors document garden locations and existing resources so the program can track garden sites and plan tree distribution. Translated from the retired feature file `Specs/Features/GardenLocationMapping.feature` (high priority, mobile, GPS).

Mapping is done at the grower's garden site by a mentor with an approved enrollment on file.

## Completing a mapping

Opening a mapping for a grower auto-fills their information from the enrollment. The mentor then records:

- **GPS coordinates**, captured on the device.
- **Water availability** — whether there is water in the plot and whether a water catchment system (e.g. Jojo tank) exists.
- **Existing trees**, counted by category: existing trees/productive plants, indigenous trees, and fruit and nut trees.
- **Site questions** — is there space for more trees, is the property fenced, and are resources like compost or mulch available.

The saved mapping is linked to the grower's enrollment.

## Manual GPS entry by staff

Centre staff can open a mapping record that has no GPS coordinates and enter latitude and longitude manually (e.g. `-24.5271`, `31.1367`) to update the location.

## Linking to an existing enrollment

When starting a new mapping, the mentor can search existing enrollments by name (e.g. "Mary" finds "Mary Nkuna"). Selecting a result auto-fills the grower name, house number, and village from the enrollment.

## Related pages

- [Grower Enrollment](/Specifications/Grower-Enrollment)

## Scenarios (Gherkin)

The original scenarios, preserved verbatim from the retired `Specs/Features/GardenLocationMapping.feature` as the precise acceptance criteria for the behaviour described above.

```gherkin
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
```
