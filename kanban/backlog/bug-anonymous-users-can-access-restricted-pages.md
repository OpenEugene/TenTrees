---
priority: high
tags: [bug, security]
---

# [Security/RBAC] Anonymous users can access restricted pages (Assessment, Grower, Cohort, Mentor)

GitHub issue: #83

Unauthenticated users can directly access Assessment, Grower, Cohort, and Mentor pages. Enrollment and Village correctly redirect to login, but these four do not.

## Checklist

- [ ] Apply [Authorize] attributes to Blazor components for these four modules
- [ ] Apply [Authorize] attributes to backend API controllers
- [ ] Verify all restricted routes redirect to /login when unauthenticated
