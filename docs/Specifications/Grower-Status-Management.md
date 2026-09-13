[Home](../Home.md) / [Specifications](../Specifications.md) / Grower Status Management <!-- wikidown:breadcrumb -->

# Grower Status Management

Administrators manage grower status across Active, Inactive, and Exited states to keep program statistics and participation records accurate. This is a medium-priority feature with a security dimension: status changes affect who is counted in the programme and must be restricted to administrators.

## Status badges

Grower lists colour-code status: **Active** = green, **Inactive** = yellow, **Exited** = grey.

## Active / Inactive toggle

From a grower's record an admin can click **Mark Inactive** or **Mark Active** to toggle between the two states, with a success confirmation each way. For example, opening Grace Sithole's record while she is Active and clicking "Mark Inactive" changes her status to Inactive and shows a success confirmation; clicking "Mark Active" on her record while she is Inactive returns her to Active with the same confirmation.

## Recording a program exit

Both Active and Inactive growers can be exited. The admin clicks **Record Exit**, enters an exit date, selects a reason, and clicks **Confirm Exit**; the grower's status becomes Exited with the date and reason recorded. For example, recording an exit for Grace Sithole with date 2025-11-15 and reason "Moved away" leaves her status as Exited, with the reason "Moved away" and date 2025-11-15 stored against her record; the same flow works when she is Inactive (e.g. with reason "Non-compliance").

Exit reasons are a fixed dropdown containing exactly these options and no others:

- Moved away
- Deceased
- Voluntary withdrawal
- Non-compliance
- Other

Rules on the exit form:

- **Exit notes** are required when the reason is "Other" (a text area appears and the exit cannot be confirmed without notes); for any other reason the notes area is not shown and notes are not required.
- Exit **date** and **reason** are both required — confirming without either shows a validation error ("Exit date is required" / "Exit reason is required") and nothing is recorded. The reason check applies even when a date has already been entered.
- **Cancel** dismisses the form leaving the status unchanged.

## Post-exit behaviour

- An exited grower's record shows the exit date and reason; the "Record Exit" and "Mark Inactive" buttons are hidden so they cannot be exited again.
- Exited growers are excluded from active-grower queries and lists.
- Exited growers cannot receive new assessments — see [Garden Assessment](/Specifications/Garden-Assessment).

## Access control

Only admins can change grower status. Mentors see the current status and exit details read-only, with no "Mark Inactive", "Mark Active", or "Record Exit" buttons.
