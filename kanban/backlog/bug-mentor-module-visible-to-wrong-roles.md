---
priority: high
tags: [bug, security]
---

# [RBAC] Mentor module is visible to Mentor and Educator roles

GitHub issue: #84

The Mentor management module should be hidden from users with Mentor or Educator roles, but these users can currently view the Mentor page and retrieve data via the API.

## Checklist

- [ ] Modify Oqtane module permissions to revoke View access for Mentor and Educator roles
- [ ] Enforce backend API restrictions on /api/Mentor endpoint
