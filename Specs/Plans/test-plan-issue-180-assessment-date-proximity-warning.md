# Browser-Based Test Plan: Issue #180 — Assessment Date Proximity Warning

## 1. Scope

This plan covers the change on `feature/issue-180-assessment-date-proximity-warning`
(commit `c75a8aa`), which replaces the server-side assessment-frequency **hard stop**
with a **per-cohort configurable, non-blocking warning** shown on the Assessment Edit
screen.

| Area | What changed |
|---|---|
| `Server/Services/AssessmentService.cs` | `CanSubmitAssessmentAsync` no longer checks days-since-last-assessment. It now only blocks submission when the grower is missing or not `Active` (exited/inactive-grower hard stop is unchanged). |
| `Shared/Models/Cohort.cs` / `Sql/dbo/Tables/Cohort.sql` | New `AssessmentFrequencyDays` (int, default 30) on Cohort. |
| `Sql/Scripts/Migration_AddAssessmentFrequencyDaysToCohort.sql` | Idempotent migration adding the column to existing databases. |
| `Client/Modules/Cohort/Edit.razor` | New required numeric field "Assessment Frequency (days)", min 1, with standard `is-invalid`/`aria-invalid` validation. |
| `Client/Modules/Assessment/Edit.razor` | On grower selection / date change, loads the grower's assessment history and the frequency from their most-recently-activated cohort (default 30 if none), then shows a non-blocking `alert-warning` when the entered Assessment Date is within that many days of another existing assessment for the same grower. A duplicate-date (0-day) match gets stronger wording. Editing an assessment excludes its own record from the comparison. |
| Resx (`Edit.resx` / `Edit.ts-ZA.resx` for Assessment and Cohort) | `Message.FrequencyError` replaced by `Message.GrowerNotEligibleError`; new `Message.AssessmentDateProximityWarning`, `Message.AssessmentDateDuplicateWarning`, `Field.AssessmentFrequencyDays`. |

**Out of scope**: no change to eligibility rules for exited/inactive growers beyond the
message text; no change to how cohorts are activated.

---

## 2. Test Accounts / Data Prerequisites

- A mentor account with at least one grower assigned, and access to a village/cohort.
- **Two cohorts** with different `AssessmentFrequencyDays` values, e.g.:
  - Cohort A: `AssessmentFrequencyDays = 14`, `ActivatedOn` within the last 365 days.
  - Cohort B: `AssessmentFrequencyDays = 30`, `ActivatedOn` more than 365 days ago.
- **Grower 1** (member of Cohort A) with one existing assessment.
- **Grower 2** (member of Cohort B) with one existing assessment.
- **Grower 3** with no cohort membership and no existing assessments.
- **Grower 4** with `Status = Exited` (or `Inactive`, if that status exists) for the
  hard-stop regression check.
- If the target database predates this branch, the migration script must be run first
  (see TC-DB-001).

---

## 3. Test Cases

### Phase 1 — Database Migration (TC-DB)

**TC-DB-001: Migration adds column with correct default**

1. On a database from before this branch, run
   `Sql/Scripts/Migration_AddAssessmentFrequencyDaysToCohort.sql`.
2. **Expected**: `Cohort.AssessmentFrequencyDays` column exists, `NOT NULL`, and all
   existing rows read `30`.
3. Run the script a second time.
4. **Expected**: No error — the `IF NOT EXISTS` guard makes it a no-op the second time.

---

### Phase 2 — Cohort Edit Form (TC-COHORT)

**TC-COHORT-001: New field renders and defaults to 30 for a new cohort**

1. Log in, navigate to Cohort → Add.
2. **Expected**: "Assessment Frequency (days)" field is present and pre-filled with `30`.

**TC-COHORT-002: Field is required and validated**

1. On Cohort Add/Edit, clear the Assessment Frequency field (or set to `0`).
2. Click Save.
3. **Expected**: Save is blocked, the field gets `is-invalid` styling, and "Assessment
   Frequency (days)" appears in the missing-fields message.

**TC-COHORT-003: Existing cohort loads and saves its configured value**

1. Open an existing cohort for edit.
2. Change Assessment Frequency to `21`, save.
3. Reopen the cohort.
4. **Expected**: Value persists as `21`.

---

### Phase 3 — Assessment Date Proximity Warning (TC-PROX)

These map directly to the scenarios added in `Specs/Features/GardenAssessment.feature`.

**TC-PROX-001: No warning when dates are far apart**

1. Log in, start a new assessment for Grower 1 (Cohort A, 14-day frequency).
2. Enter an Assessment Date more than 14 days from Grower 1's existing assessment date.
3. **Expected**: No warning shown. Save succeeds.

**TC-PROX-002: Warning shown inside the frequency window**

1. New assessment for Grower 1.
2. Enter an Assessment Date exactly 10 days from the existing assessment.
3. **Expected**: Yellow `alert-warning` appears below the date field reading the
   proximity message with the nearby date and day count. Save is **not** blocked —
   confirm the assessment still saves successfully.

**TC-PROX-003: Boundary — day diff exactly equal to the cohort's frequency**

1. New assessment for Grower 1 (14-day frequency).
2. Enter an Assessment Date exactly 14 days from the existing one.
3. **Expected**: Warning **is** shown (the check is `<=`, so the boundary itself warns).
4. Repeat at 15 days apart — **expected**: no warning.

**TC-PROX-004: Wider window for a longer-frequency cohort**

1. New assessment for Grower 2 (Cohort B, 30-day frequency).
2. Enter an Assessment Date 20 days from Grower 2's existing assessment.
3. **Expected**: Warning shown (20 ≤ 30), even though this would have been *accepted*
   under the old hard-stop rule for a non-Year-1 cohort at a different offset — confirm
   nothing blocks the save.

**TC-PROX-005: No cohort / no history → no warning**

1. New assessment for Grower 3 (no cohort, no history).
2. Enter any Assessment Date.
3. **Expected**: No warning at any date, since there is no prior assessment to compare
   against.

**TC-PROX-006: Duplicate-date wording**

1. New assessment for Grower 1.
2. Enter an Assessment Date that exactly matches an existing assessment's date.
3. **Expected**: The stronger duplicate-specific message
   (`Message.AssessmentDateDuplicateWarning`) is shown, not the generic proximity
   message. Save still succeeds.

**TC-PROX-007: Editing an assessment excludes its own date**

1. Open an existing assessment for Grower 1 for **edit** (not add).
2. Without changing the Assessment Date, observe the warning area.
3. **Expected**: No warning fires purely from comparing the record against itself.
4. Change the date to be close to a *different* existing assessment for the same
   grower (if one exists) and confirm the warning now appears — proves the exclusion
   is by `AssessmentId`, not a blanket suppression on Edit.

**TC-PROX-008: Warning recalculates live without reload**

1. New assessment for Grower 1, no grower selected yet.
2. Select Grower 1, then change the Assessment Date field several times between
   values inside and outside the frequency window.
3. **Expected**: The warning appears/disappears immediately on each date change
   (`@bind:after="OnAssessmentDateChanged"`), with no page reload or save required.

**TC-PROX-009: Switching grower recalculates frequency and history**

1. Start a new assessment, select Grower 1 (14-day cohort), note the frequency
   behavior from TC-PROX-002.
2. Change the Grower dropdown to Grower 2 (30-day cohort) without saving.
3. **Expected**: The proximity check now uses Grower 2's history and 30-day
   frequency, not Grower 1's — old warning state doesn't leak across the grower switch.

---

### Phase 4 — Regression: Hard Stop Still Works (TC-REGRESS)

**TC-REGRESS-001: Exited/inactive grower is still blocked**

1. Attempt to submit a new assessment for Grower 4 (`Status = Exited`).
2. **Expected**: Submission is blocked with the new message text
   ("This grower is not currently active in the program...") — confirm the message
   key is `Message.GrowerNotEligibleError`, not the old `Message.FrequencyError` text.

**TC-REGRESS-002: Old frequency hard-stop no longer applies**

1. Submit two assessments for the same active grower on consecutive days (or same day).
2. **Expected**: Both saves succeed (previously the second would have been rejected
   with "too soon to submit"). Only the non-blocking warning appears, if applicable.

---

### Phase 5 — Accessibility & Localization (TC-A11Y / TC-L10N)

**TC-A11Y-001: Warning alert is announced and icon is decorative**

1. Trigger the proximity warning (TC-PROX-002).
2. Inspect the DOM: `<div class="alert alert-warning ... role="status">` and the
   `oi-warning` icon has `aria-hidden="true"`.
3. **Expected**: matches `accessibility-and-validation` skill conventions — screen
   reader announces the warning text without a redundant "warning" icon name.

**TC-A11Y-002: Cohort frequency field validation is accessible**

1. Trigger TC-COHORT-002.
2. **Expected**: field has `aria-required="true"` and, once invalid,
   `aria-invalid="true"` plus `is-invalid` styling — consistent with other required
   fields on the same form (e.g. Village, Name).

**TC-L10N-001: ts-ZA strings render for the new messages**

1. Switch the site language to `ts-ZA` (Xitsonga).
2. Repeat TC-PROX-002, TC-PROX-006, and TC-REGRESS-001.
3. **Expected**: The Xitsonga translations from `Edit.ts-ZA.resx` are shown, not the
   English fallback, and not raw resource keys.

---

## 4. Notes / Risks Observed During Review

- The proximity check only ever compares against the **selected grower's own**
  assessment history (`GetAssessmentsByGrowerAsync`), so no cross-grower false
  positives are possible — no test needed for that, but worth confirming once in
  TC-PROX-001 by checking the network payload if desired.
- `_assessmentFrequencyDays` defaults to `30` both when a grower has no cohort and
  transiently while `LoadTreesAsync` is running — no observable flicker expected, but
  worth a quick visual check on a slow connection (throttle network in devtools) during
  TC-PROX-009.
- No automated step definitions exist for `Specs/Features/GardenAssessment.feature` in
  this repo (Gherkin files here are living documentation, not an executable suite), so
  this manual pass is the only verification gate before merge.
