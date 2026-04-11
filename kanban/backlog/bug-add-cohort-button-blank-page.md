---
priority: high
tags: [bug]
---

# [Bug] 'Add Cohort' button navigates to blank page (Oqtane routing conflict)

GitHub issue: #81

Clicking "Add Cohort" navigates to `/cohort/*/39/Add` which renders blank. Oqtane interprets "Add" as a page management action rather than forwarding to the Blazor component.

## Checklist

- [ ] Rename the route action from "Add" to "New" or similar to avoid Oqtane conflict
- [ ] Alternatively, configure Oqtane to bypass its interceptor for this module action
