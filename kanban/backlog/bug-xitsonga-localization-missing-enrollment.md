---
priority: medium
tags: [bug]
---

# [Bug] Xitsonga localization missing for Enrollment form labels

GitHub issue: #82

Enrollment wizard form labels remain in English when UI language is switched to Xitsonga. The culture cookie is correctly set but the UI doesn't reflect localized text. "Tree Mentor Name" label still displays in English.

## Checklist

- [ ] Add missing Xitsonga localization entries in .resx files for Enrollment module
- [ ] Verify "Tree Mentor Name" label translates correctly
