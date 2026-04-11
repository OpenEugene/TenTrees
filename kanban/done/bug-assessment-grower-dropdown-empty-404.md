---
priority: high
tags: [bug, blocker]
---

# [Bug][Blocker] Assessment Grower dropdown is empty — /api/Assessment/growers returns 404

GitHub issue: #92

The Grower dropdown on Assessment Create and Edit forms displays no options. The API endpoint `GET /api/Assessment/growers` returns 404, blocking all new assessment creation.

## Checklist

- [ ] Verify API route registration for the endpoint
- [ ] Ensure the controller action is correctly mapped
- [ ] Confirm the Blazor component properly calls and binds the response
