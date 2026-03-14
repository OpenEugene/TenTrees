---
priority: medium
tags: []
---

# Add mobile device compatibility and low-bandwidth performance requirements

## Context

Per the Feb 3, 2026 meeting, mobile device testing and low-bandwidth performance were major discussion points:

- Mark wants to see the app working on **Nokia phones sold in Africa** (~7:00)
- The app is hosted on a **European data center** for better connectivity to South Africa (~4:56)
- Kimberlee tested on her phone during the meeting (~8:07)
- Mark noted **screen resolutions** are a concern (~6:58)
- The UI is intentionally "big and simple" with large buttons for mobile use (~6:04)
- Rebecca's team will test on actual field devices via Quinton and TryGive (~9:34)

## What's Missing

No feature file captures non-functional requirements for mobile device compatibility or performance. The `@mobile` tag exists on several features but only indicates the feature should work on mobile — there are no specific acceptance criteria for device types, screen sizes, bandwidth, or load times.

## Proposed Changes

Consider adding acceptance criteria (either as a new NFR feature file or as scenarios added to existing `@mobile` features):

- [ ] App renders correctly on small-screen smartphones (320px–480px width)
- [ ] Touch targets meet minimum size requirements (44x44px per README)
- [ ] Forms are usable on Nokia and off-brand Android phones common in rural South Africa
- [ ] Page load performance is acceptable on 3G/EDGE connections
- [ ] App functions correctly when hosted from European data center with SA users
- [ ] PWA/home screen icon installation works on target devices
- [ ] Font sizes remain readable on small screens (16px minimum per README)

## Priority

Medium — field testing with Quinton/TryGive before Rebecca's April trip to SA will surface real issues, but having documented criteria helps define "done."

## Source

Feb 3, 2026 meeting transcript (~4:48–8:34 timestamps)
