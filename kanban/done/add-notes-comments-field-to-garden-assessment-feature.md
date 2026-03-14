---
priority: medium
tags: []
---

# Add notes/comments field to Garden Assessment feature

## Context

Per the Feb 3, 2026 meeting, Kimberlee asked whether notes fields were built into the platform. Rebecca clarified:

- Notes are **more important on the garden assessment** form (~10:58)
- They **don't typically put notes on the enrollment form** (~11:05)

The paper garden assessment forms have a notes section, but the digital feature file does not.

## What's Missing

`GardenAssessment.feature` has no notes/free-text field in any scenario. Mentors currently have no way to record qualitative observations, context about problems, or other freeform information during an assessment.

## Proposed Changes

Add to `GardenAssessment.feature`:
- [ ] Optional notes/comments free-text field on the garden assessment form
- [ ] Notes should be saved with the assessment record
- [ ] Notes should be visible to staff when reviewing assessments
- [ ] Notes should be included in data exports
- [ ] Notes field should support both English and Xitsonga input

## Source

Feb 3, 2026 meeting transcript (~10:50–11:08 timestamps)
