[Home](../Home.md) / [Specifications](../Specifications.md) / Grower Enrollment <!-- wikidown:breadcrumb -->

# Grower Enrollment

Tree mentors submit grower enrollments digitally at the household so that enrollment data is captured accurately and linked to growers. This is a high-priority, mobile workflow: the mentor is logged in and standing at the grower's household, usually working on a touch-screen phone.

## Enrollment list

The enrollments list opens with a status summary dashboard of four cards, each showing the count of enrollments in that state: **Pending** (yellow), **Approved** (green), **Rejected** (red), and **Total** (blue).

Each row in the list carries two badges: an **Enrollment Status** badge (Pending / Approved / Rejected) and a **Grower Status** badge (Active / Inactive / Exited). Rows with Pending enrollments are highlighted yellow; Rejected rows are highlighted red.

The list can be filtered by enrollment status (e.g. choosing "Pending" shows only pending enrollments, then choosing "Approved" shows only approved ones) and — for admins — by village (e.g. choosing "Orpen Gate Village" shows only that village's enrollments). A **Clear Filters** button resets both filters and restores the full list.

## Creating an enrollment

A new enrollment captures, in order:

1. Village, grower name, house number, ID number or birthdate, and household size (e.g. village "Orpen Gate Village", grower "Mary Nkuna", house number 42, household of 5).
2. Home ownership ("Do they own their home?").
3. Preferred criteria questions.
4. Commitment acknowledgments.
5. A finger-drawn signature plus a confirmation checkbox.

On save, the signature is stored as an SVG image and the mentor sees "Enrollment saved successfully". Submitting without a grower name is blocked with the validation error "Grower name is required".

### Mentor defaulting

The tree mentor dropdown is pre-selected with the logged-in user (storing the username as the mentor ID — for example, mentor "Bondi" is shown by name and stored as "bondi") and the date defaults to today. The dropdown lists all registered site users, so a different mentor can be selected — e.g. an educator entering data can choose the actual mentor, and the chosen mentor's username is stored in the same way.

### Preferred criteria

Each of these is answered Yes/No and saved individually:

- Are they currently enrolled in PE with a garden growing?
- Are they a graduate of PE in the past?
- If so, is their garden planted and tended?
- Child headed household?
- Woman headed household?
- Empty or nearly empty yard?

### Commitments

Each commitment is acknowledged and recorded:

- Not using chemicals or pesticides
- Attending five permaculture training classes
- Not cutting trees
- Standing for women and children with no abuse
- Caring for trees while away from home
- Giving permission for the mentor to enter the yard

## Signature capture

The signature step shows a canvas with a "Sign here" prompt and a **Clear** button. The mentor draws with a finger; on a touch device the line follows the finger and the page does not scroll while drawing. Once something has been drawn the canvas visibly stops being blank. After checking the confirmation checkbox and submitting:

- The signature is saved as an SVG string in `SignatureData`.
- `SignatureCollected` is set to true and `SignatureDate` to today.

A blank signature blocks submission with the error "Signature is required", even if the confirmation checkbox has been ticked. **Clear** empties the canvas so the signature can be redrawn.

## Cohort selection at enrollment

When the selected village has one or more Active cohorts, a cohort picker appears listing them plus a "none" option (e.g. selecting village "Roebuck" offers "Roebuck 1 2026"); it is hidden entirely when the village has no active cohorts (e.g. "Londelozzi"). Selecting a cohort is optional — enrolling with "none" saves normally and creates no cohort membership. When an admin approves an enrollment that had a cohort selected, a `GrowerCohort` record is created for that grower in that cohort. See [Cohort Management](/Specifications/Cohort-Management).

## Admin maintenance

An administrator can trigger **Backfill Growers** from the admin panel: a Grower record is created for each approved enrollment that lacks one, and the count of newly created records is returned.
