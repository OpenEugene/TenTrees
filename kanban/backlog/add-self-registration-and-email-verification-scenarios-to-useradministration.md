---
priority: medium
tags: []
---

# Add self-registration and email verification scenarios to UserAdministration

## Context

Per the Feb 3, 2026 meeting, Mark demoed user registration at 10trees.org and noted that the email verification roundtrip *"I don't know that that part actually works"* (~2:42). Rebecca also mentioned needing to register Quinton and TryGive as users for field testing.

`UserAdministration.feature` covers admin creating accounts and role assignment, but does not cover:
- Self-registration flow
- Email verification
- Account activation by admin

## What's Missing

No scenario in `UserAdministration.feature` covers the self-registration workflow that exists in the deployed app. The current feature file only has admin-initiated account creation.

## Proposed Changes

Add to `UserAdministration.feature`:
- [ ] User self-registration scenario (name, email, password)
- [ ] Email verification scenario (or document that it's handled by Oqtane out of the box)
- [ ] Admin activation of pending accounts (Mark mentioned manually activating users)
- [ ] Registration from mobile device scenario (field team will register on phones)

## Notes

This may be largely handled by Oqtane's built-in registration. If so, the feature file should still document the expected behavior even if we don't implement custom code — it serves as acceptance criteria for verifying the Oqtane configuration.

## Source

Feb 3, 2026 meeting transcript (~1:35–3:00 timestamps)
