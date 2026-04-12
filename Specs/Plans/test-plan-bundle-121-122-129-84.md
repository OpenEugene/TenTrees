# Browser-Based Test Plan: Release Bundle (#121, #122, #129, #84)

## 1. Scope

This plan covers the four issues addressed by code changes in this release. **Issue #83
(anonymous page redirect) is explicitly out of scope** — the API endpoints were already
protected with `[Authorize]`; making the page itself redirect requires removing anonymous
View permission from the affected Oqtane pages in the admin panel, which is a
configuration step not part of this release. A separate configuration task should be
raised and tested independently.

| Issue | What changed |
|---|---|
| #121 / #120 / #119 / #118 / #130 / #131 | `ServerGrowerService` no longer filters by `MentorUsername`. All authenticated users now see all active growers. |
| #122 | Downstream of #121 — selecting a grower no longer fails because the list was empty. |
| #129 | Enrollment "Add Enrollment" button changed from `SecurityAccessLevel.Edit` to `SecurityAccessLevel.View`. Any user who can view the Enrollment page can now see the button. |
| #84 | `MentorController` restricted to the `"10 Trees Admin"` role. The Mentor management API now returns 403 for Mentor and Educator users. |

---

## 2. Test Accounts Required

| Account | Role(s) | Notes |
|---|---|---|
| `TestAdmin` | **10 Trees Admin** | Must hold this specific custom role. "Administrators" alone is not sufficient — see TC-RBAC-003. |
| `TestMentor` | Mentor | Standard mentor account. |
| `TestEducator` | Educator | Standard educator account. |
| `TestPlatformAdmin` | Administrators (Oqtane built-in only) | Used to verify the role boundary. Must **not** hold "10 Trees Admin". |

## 3. Data Prerequisites

- At least 3 active growers exist in the system (they do not need to have a
  `MentorUsername` set — that was the bug).
- At least one active village and one active cohort exist.
- `TestMentor` must have **Edit** permission on the Assessment module in Oqtane (required
  for the "New Assessment" button to be visible — this is existing Oqtane configuration,
  not changed by this release).

---

## 4. Test Cases

### Phase 1 — Mentor Grower Visibility (#121, #122)

**TC-121-001: Grower dropdown is populated for mentor**

1. Log in as `TestMentor`.
2. Navigate to Assessment → click "New Assessment".
3. Open the Grower dropdown.
4. **Expected**: The dropdown lists all active growers in the system — not just those with
   `MentorUsername` matching the logged-in user. The list is not empty.
5. **Fail signal**: Dropdown is empty or shows only growers explicitly assigned to
   TestMentor.

**TC-121-002: Grower dropdown is populated on Assessment edit**

1. Log in as `TestMentor`.
2. Navigate directly to an existing assessment edit URL
   (e.g. `/assessment/!/[moduleId]/Edit?id=[assessmentId]`).
3. **Expected**: The Grower field is populated and selectable.

**TC-122-001: Notes can be saved once a grower is selected**

1. Log in as `TestMentor`.
2. Navigate to Assessment → click "New Assessment".
3. Select any grower from the (now-populated) dropdown.
4. Fill in Assessment Name, Date, Trees Planted ≥ 1, Trees Alive ≤ Trees Planted.
5. Enter text in the notes field.
6. Click Save.
7. **Expected**: Assessment saves successfully. No "select a grower" or "required field"
   validation error appears.
8. **Fail signal**: Save is blocked with a grower-related validation error.

---

### Phase 2 — New Enrollment Button Visibility (#129)

**TC-129-001: Add Enrollment button visible for mentor**

1. Log in as `TestMentor`.
2. Navigate to `/enrollment`.
3. **Expected**: The "Add Enrollment" button is visible at the top of the page.
4. **Fail signal**: Button is not present. (If this fails, check that `TestMentor` has
   View permission on the Enrollment module in Oqtane — the button now requires View,
   not Edit.)

**TC-129-002: Add Enrollment button visible for educator**

1. Log in as `TestEducator`.
2. Navigate to `/enrollment`.
3. **Expected**: The "Add Enrollment" button is visible.

**TC-129-003: Add Enrollment button still visible for 10 Trees Admin**

1. Log in as `TestAdmin` (10 Trees Admin role).
2. Navigate to `/enrollment`.
3. **Expected**: The "Add Enrollment" button is visible.

---

### Phase 3 — Mentor Module API Restriction (#84)

> **Note on navigation**: Whether the "Mentor" link appears in the nav bar is controlled
> by Oqtane page permissions, which were not changed in this release. The nav link may
> still be visible. These tests focus on what the API and page actually deliver when
> accessed.

**TC-84-001: Mentor role cannot retrieve mentor list**

1. Log in as `TestMentor`.
2. Navigate directly to `/mentor`.
3. **Expected**: The page loads but displays no mentor records (the API call returns 403,
   so the list is empty). No mentor data is visible.
4. **Not expected**: A browser-level "Access Denied" page or redirect — that requires
   Oqtane page permission configuration.
5. **Fail signal**: A list of mentors is displayed.

**TC-84-002: Educator role cannot retrieve mentor list**

1. Log in as `TestEducator`.
2. Navigate directly to `/mentor`.
3. **Expected**: Same as TC-84-001 — page loads, list is empty, no mentor data visible.
4. **Fail signal**: A list of mentors is displayed.

**TC-84-003: "Administrators" role without "10 Trees Admin" cannot retrieve mentor list**

1. Log in as `TestPlatformAdmin` (holds only the Oqtane "Administrators" role, not
   "10 Trees Admin").
2. Navigate directly to `/mentor`.
3. **Expected**: The page loads but displays no mentor records. The API returns 403.
4. **Rationale**: This is the key boundary test. The Oqtane "Administrators" role is the
   platform role; "10 Trees Admin" is the programme role. They are distinct. This test
   confirms the distinction is enforced.
5. **Fail signal**: Mentor list is populated.

---

### Phase 4 — 10 Trees Admin Baseline (#84, #121)

**TC-ADM-001: 10 Trees Admin can access Mentor management**

1. Log in as `TestAdmin` (must hold "10 Trees Admin" role).
2. Navigate to `/mentor`.
3. **Expected**: The Mentor management page loads and displays the list of mentors.
4. **Fail signal**: Mentor list is empty or a 403 error appears. If this fails, verify
   `TestAdmin` holds the `"10 Trees Admin"` role (not just `"Administrators"`).

**TC-ADM-002: 10 Trees Admin sees all growers in assessment**

1. Log in as `TestAdmin`.
2. Navigate to Assessment → click "New Assessment".
3. Open the Grower dropdown.
4. **Expected**: All active growers in the system are listed, regardless of
   `MentorUsername` assignment.

**TC-ADM-003: Admin-only grower actions still restricted by role**

1. Log in as `TestMentor`.
2. Navigate to a grower's Status page.
3. **Expected**: The "Toggle Active/Inactive" button and "Record Exit" button are not
   visible.
4. Log out, log in as `TestAdmin` (10 Trees Admin).
5. Navigate to the same grower's Status page.
6. **Expected**: Both buttons are visible and functional.
7. **Rationale**: Verifies that the `TenTreesAdmin` role check on write operations in
   `GrowerService` is working correctly.

---

## 5. Out of Scope for This Release

| Item | Reason | Recommended next step |
|---|---|---|
| Anonymous redirect to login (#83) | Requires removing anonymous View permission from Assessment, Grower, Cohort, and Mentor pages in the Oqtane admin panel — no code change was made. | Raise a configuration task; verify by removing anonymous page permissions in Oqtane and re-running TC-SEC tests. |
| "Mentor" link hidden from nav for wrong roles (#84 partial) | Nav link visibility is controlled by Oqtane page View permissions, not changed in this release. | Same configuration task as above — remove View permission from the Mentor page for Mentor and Educator roles. |
| Direct API calls to `toggle-status` / `exit` by non-admin | TC-ADM-003 is a UI visibility test only. If a non-admin bypasses the UI: `toggle-status` throws `UnauthorizedAccessException` which the controller returns as 500; `exit` returns `null` which the controller returns as 404. Neither returns a clean 403. This is a known inconsistency not addressed in this release. | Raise a follow-up to add a specific `UnauthorizedAccessException` catch in `GrowerController` returning 403, and align `RecordProgramExitAsync` to throw rather than return null. |
