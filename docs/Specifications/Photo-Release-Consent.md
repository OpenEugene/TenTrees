# Photo Release Consent

Tree mentors capture photo release preferences so the organization has proper consent for using grower photos. Translated from the retired feature file `Specs/Features/PhotoReleaseConsent.feature` (high priority, mobile).

## Capturing consent

The release form is available for growers with an **approved enrollment**, and the completed form is linked to that enrollment. The mentor selects one of three consent options and captures the grower's signature; the option chosen determines the stored consent level:

| Option selected | Consent level |
|---|---|
| "You may use my photo with my name identified" | Full |
| "You may use my picture in group photos without my name" | Limited |
| "You may not use my photo at all" | None |

## Related pages

- [Grower Enrollment](/Specifications/Grower-Enrollment) — the approved enrollment the release form links to.

## Scenarios (Gherkin)

The original scenarios, preserved verbatim from the retired `Specs/Features/PhotoReleaseConsent.feature` as the precise acceptance criteria for the behaviour described above.

```gherkin
@workflow-release @priority-high @mobile
Feature: Photo Release Consent
  As a tree mentor
  I want to capture photo release preferences
  So that the organization has proper consent for using grower photos

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
