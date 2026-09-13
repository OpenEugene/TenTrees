[Home](../Home.md) / [Specifications](../Specifications.md) / Assessment Photo Storage <!-- wikidown:breadcrumb -->

# Assessment Photo Storage

Mentors can attach photos of a garden problem to an assessment so that centre staff can judge the issue before travelling to the garden. Photos are optional; an assessment is complete without them.

## Adding photos

On a new assessment the photo uploader appears as soon as a grower is selected, so photos can be added before the assessment is saved. Until Save, they are shown as "not saved yet"; Save attaches them to the new assessment. Cancelling the form, or changing the grower, discards them.

On a saved assessment, photos can be added and removed at any time.

Limits:

- Up to five photos per assessment.
- JPG, JPEG, PNG and WebP only.
- A photo can be uploaded straight from the phone camera; files up to 25 MB are accepted. Mentors in the field have no way to shrink a photo themselves, so the application reduces each one after upload to at most 1600 pixels on its longest side, corrects the rotation the camera recorded, and keeps the result small. Anything that is not actually an image is rejected.

The phone's normal file chooser is used, which on most phones offers the camera as one option. A dedicated in-app camera is not a feature.

Photos are not part of the offline draft. A connection is needed to upload a photo; the field draft itself still works without one.

## Where photos live and who can see them

Each grower has one private photo folder in the site's file storage, so all photos of a garden sit together. This is intended to support a per-garden gallery later.

- The assigned mentor can view, add and remove photos for their own growers.
- Centre staff (10Trees Admin, Educator, Project Manager) can view all photos; admins can also manage them.
- A mentor cannot see another mentor's growers' photos.
- When a grower is reassigned to a new mentor, access follows the assignment.

Files are named for people browsing the folder: year, month and a letter taken from the assessment date, for example `2026-Sep_A.jpg`. The application does not depend on the name, so files can be renamed or reorganised in the site's file manager without breaking anything.

Removing a photo, or deleting an assessment, removes the file as well. An upload that was never attached to a saved assessment (for example because the tab was closed) is cleaned up automatically after a day.

## Deployment prerequisites

Publish the SQL project schema to the target database before deploying an application update that includes this feature, so the photo table exists. The site's allowed upload file types must include jpg, jpeg, png and webp, otherwise uploads are rejected before they reach the assessment form.

## Related pages

- [Garden Assessment](/Specifications/Garden-Assessment) — the assessment workflow the photos belong to.
- [Role-Based Data Visibility](/Specifications/Role-Based-Data-Visibility) — programme role expectations.
- [Mentor Management](/Specifications/Mentor-Management) — mentor-to-grower assignment, which governs photo access.
