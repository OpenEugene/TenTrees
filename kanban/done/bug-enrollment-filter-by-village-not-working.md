---
priority: high
tags: [bug]
---

# [Bug] Enrollment list 'Filter by Village' dropdown does not filter results

GitHub issue: #87

The village filter dropdown on the Enrollment management page is non-functional. Selecting a village and clicking "Filter" does not update the enrollment list. Status counter buttons work correctly — issue is isolated to the village dropdown.

## Checklist

- [ ] Fix event handler and data-binding logic for the village dropdown
- [ ] Ensure filter values are correctly passed to API queries or applied to local state
