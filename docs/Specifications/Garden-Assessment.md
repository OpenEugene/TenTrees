[Home](../Home.md) / [Specifications](../Specifications.md) / Garden Assessment <!-- wikidown:breadcrumb -->

# Garden Assessment

Tree mentors regularly assess garden health and tree survival so the program can track outcomes and identify problems early.

## Submission eligibility

Before the assessment form loads, the system verifies the grower is eligible:

- A grower with status **Exited** cannot receive a new assessment — the mentor is blocked and told the grower is no longer active in the program.
- An active grower's form loads normally.

The form auto-fills the grower's name and village from their approved application; house number and ID are not re-entered.

## Tree survival

The mentor records trees planted and trees still alive; the survival rate is auto-calculated (e.g. 10 planted, 9 alive → 90%). If any trees died, the mentor answers "If any died, which ones?" either as free text (e.g. "Mango, Avocado") or by selecting tree types from a dropdown.

## Permaculture practices

Nine Yes/No practice questions are each saved individually, and the total count of principles in use is derived (the principles *not* in use remain identifiable from the record):

- Do the trees look healthy?
- Any chemical fertilizers?
- Any pesticides used?
- Are the trees mulched?
- Are they making compost?
- Are they collecting water?
- Any leaky taps visible?
- Is the garden designed to capture water?
- Are they using greywater?

## Problems and help requests

The mentor can select zero, one, or many problems (e.g. broken branches, yellow leaves, losing leaves, trees look dry, pests eating the plant). A separate question — "Do you need someone to help with this problem?" — sets a help request flag. An assessment with no problems and "No" to help saves with zero problems and the flag false.

## Problem photos

A mentor can attach photos of a problem so centre staff can judge the issue before travelling to the garden. Photos are optional; an assessment is complete without them.

On a new assessment the photo uploader appears as soon as a grower is selected, so photos can be added before or after saving. Photos added before Save are shown as "not saved yet" and are attached when the assessment is saved; cancelling, or changing the grower, discards them. On a saved assessment, photos can be added and removed at any time.

Up to five photos per assessment, JPG, JPEG, PNG or WebP, each at most 5 MB. The phone's normal file chooser is used, which usually offers the camera as one option. Photos are not part of the offline draft; a connection is needed to upload them, while the rest of the draft still works offline.

Photos are only visible through the application to people who may see the assessment: the assigned mentor and centre staff. See [Assessment Photo Storage](/Specifications/Assessment-Photo-Storage) for where photos are kept, who can see them, and what a deployer needs to prepare.

## Notes

Free-text narrative notes (e.g. "Grower has started a new compost heap near the fence") are saved with the assessment and visible to Centre staff in the report view.

## Saving and offline sync

- **Save Draft** works without connectivity: the draft is stored locally on the device with a "Draft saved" confirmation.
- When connected to Wi-Fi at the Centre, opening the draft and tapping **Submit** uploads it to the central database ("Assessment submitted").
- Successful submission always shows a confirmation ("Your assessment has been saved").

## Assessment date proximity warning

Submission is **never blocked** by the entered Assessment Date. Each cohort has its own assessment frequency, set in days by centre staff; when the entered date is within that many days of another existing assessment for the same grower, a non-blocking warning is shown so mentors can confirm they aren't duplicating a recent visit. Rules:

- Comparison is against the **entered Assessment Date**, not "today", using the grower's most-recently-activated cohort's frequency.
- Dates outside the window (e.g. 60 days apart with a 14-day frequency) produce no warning.
- Dates inside the window (e.g. 10 days apart with a 14-day frequency, or 20 days with a 30-day frequency) produce a warning but still save.
- A grower with no assessment history never warns.
- An assessment on **exactly the same date** as an existing one shows stronger duplicate-date wording, but still saves.
- When editing an existing assessment, its own original date is excluded from the check.

## Access control

- A mentor can only assess their own assigned households; navigating to another mentor's grower is denied with "You are not assigned to this household".
- A CentreAdmin can impersonate a mentor and submit on their behalf: the record shows the impersonated mentor as the submitter and notes it was entered by an admin.
- Centre staff (e.g. Educator) can view a submitted assessment but see no edit option.

## Home visits

Home visits are recorded separately from regular assessments; funders need a monthly count of visits per staff member.

- Any staff member (e.g. an Educator) can open a grower record, tap **Record Home Visit**, enter notes, and confirm the date; the visit is saved with the visiting staff member's name.
- Multiple staff can record separate visits on the same grower; all appear in the grower's visit history with each visitor's name.
- Monthly visit counts per staff member feed the "Home Visits" report — see [Program Reporting](/Specifications/Program-Reporting).

## Related pages

- [Cohort Management](/Specifications/Cohort-Management) — cohort assessment frequency settings.
- [Grower Status Management](/Specifications/Grower-Status-Management) — the Exited status that blocks assessment.
