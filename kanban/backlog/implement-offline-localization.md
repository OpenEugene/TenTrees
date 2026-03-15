---
priority: high
tags: [localization, mobile, offline]
---

# Implement Offline Localization

## Feature: Offline Localization

Language resources must be cached on first load so the app remains fully usable in the selected language without internet connectivity.

**Tags:** `@workflow-localization`, `@priority-high`, `@mobile`, `@offline`

## What's Missing

No kanban card existed for this feature. The app already supports offline draft saving, but there is no explicit mechanism confirmed to cache `.resx` / localization resources for offline use, and language switching while offline has not been verified.

## Behavior Checklist

#### Resource Caching
- [ ] On first online load, both English and Xitsonga resources are cached (service worker or Blazor WASM caching)
- [ ] Cached resources are sufficient to render all forms in the selected language without internet

#### Offline Language Use
- [ ] App displays in selected language when offline (no fallback to English due to missing resource)
- [ ] All form labels, buttons, and messages render in Xitsonga while offline

#### Language Switching Offline
- [ ] User can switch between English and Xitsonga while offline
- [ ] Switch takes effect immediately without a network request

#### Data Submission Language Neutrality
- [ ] Forms submitted offline in Xitsonga store data in language-neutral format (keys, not translated strings)
- [ ] On sync, staff can view the data correctly in English

## Related Feature File

`Specs/Features/OfflineLocalization.feature`
