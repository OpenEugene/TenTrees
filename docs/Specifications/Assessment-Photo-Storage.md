[Home](../Home.md) / [Specifications](../Specifications.md) / Assessment Photo Storage <!-- wikidown:breadcrumb -->

# Assessment Photo Storage

This page specifies the developer implementation for optional assessment problem photos introduced by [Garden Assessment](/Specifications/Garden-Assessment). Photos use Oqtane’s native file manager, folder permissions, file metadata, and file URLs; the TenTrees database never stores image bytes.

## Scope and user flow

A mentor first saves a new assessment. The form then opens the saved assessment in Edit mode, where the native Oqtane `FileManager` appears in the **Problem Photos** section. The mentor selects one JPG, JPEG, PNG, or WebP image and uses the Oqtane upload action. Each completed native upload is associated with the assessment, displayed through the Oqtane URL, and may be removed from the assessment.

The native browser picker may offer a device camera option on supported phones. A dedicated capture experience is a **stretch goal** and is not part of this release. Photo bytes are not stored in offline drafts; a connection to the Oqtane file service is required to upload a photo.

## Architecture

| Layer | Responsibility | Key implementation |
| --- | --- | --- |
| Razor form | Shows saved photos and the native upload control after an assessment has an ID. | `Client/Modules/Assessment/Edit.razor` |
| Native upload | Applies Oqtane’s configured file validation, chunking, progress, and folder permissions. | `Oqtane.Modules.Controls.FileManager` |
| Oqtane storage | Stores private physical files and Oqtane `File` metadata. | `/files/AssessmentPhotos/...` via Oqtane |
| Association API | Resolves the private folder, validates the uploaded Oqtane file, records the association, and removes both association and file. | `AssessmentController` and `ServerAssessmentService` |
| TenTrees SQL | Stores only assessment linkage, Oqtane file ID, stable Oqtane URL, and audit data. | `dbo.AssessmentPhoto` |

## Private Oqtane folder

The first request for a saved assessment provisions the system folder at `AssessmentPhotos/` for the current Oqtane site. The folder is private, system-managed, unlimited in capacity, and has `Cache-Control: no-store`. It is not a public static-assets folder. Oqtane resolves the stored URL through its `/files/` handler and applies folder-view permissions before returning the file.

All uploaded assessment photos share this folder. Oqtane permissions grant Browse, View, and Edit access to Mentors and 10Trees/Oqtane administrators. Educators and Project Managers have Browse and View access for review. The application’s assessment API continues to scope which records are displayed to a mentor; Oqtane’s private-folder permission protects the physical file endpoint.

## File naming

A client upload initially receives Oqtane’s normal temporary upload name. Once a database association exists, the server renames the Oqtane file using the database-generated association ID:

```text
assessment-{AssessmentId}-{AssessmentPhotoId}.{extension}
```

For example, assessment 42 with association 314 is stored as `assessment-42-314.jpg`. This creates deterministic, unique names without exposing grower names or requiring a GUID.

## Data model

| Column | Type | Purpose |
| --- | --- | --- |
| `AssessmentPhotoId` | `INT IDENTITY` | Primary key and unique filename suffix. |
| `AssessmentId` | `INT` | Parent garden assessment. |
| `FileId` | `INT` | Oqtane `File.FileId` for the uploaded physical file. |
| `Url` | `NVARCHAR(2048)` | Resolved Oqtane private-file URL used by the UI. |
| `CreatedBy`, `CreatedOn`, `ModifiedBy`, `ModifiedOn` | Oqtane audit types | Standard record audit data. |

The schema has a unique constraint on `(AssessmentId, FileId)` and an index on `AssessmentId`. It deliberately has no cross-database foreign key to Oqtane’s framework-owned file table.

## API contract

The Assessment module API is authenticated. Standard Oqtane module routing supplies the API root.

| Method | Route | Request | Result |
| --- | --- | --- | --- |
| `GET` | `/{assessmentId}/photo-folder` | None | The private Oqtane folder ID for the saved assessment. |
| `GET` | `/{assessmentId}/photos` | None | `AssessmentPhotoDto[]` with `FileId`, `Url`, filename, size, and audit metadata. |
| `POST` | `/{assessmentId}/photos` | `{ "fileId": 123 }` | Creates association, renames Oqtane file, stores URL, and returns metadata. |
| `DELETE` | `/photos/{assessmentPhotoId}` | None | Removes physical Oqtane file and association; returns `204 No Content`. |

The Oqtane `FileManager` performs the actual multipart upload to Oqtane’s file API. The Assessment API never accepts image bytes or returns image bytes.

## Validation and lifecycle

The native `FileManager` is configured with `Filter="jpg,jpeg,png,webp"`, `UploadMultiple="false"`, `MaxUploadFileSize="5"`, progress display, and the private folder ID. Oqtane also enforces the site’s `UploadableFiles` setting and folder capacity. The Assessment service independently validates that the referenced Oqtane file is in `AssessmentPhotos/`, has an allowed extension, is no larger than 5 MB, and that the assessment has fewer than five photos.

After a successful native upload, the form obtains the Oqtane `FileId`, posts the association, reloads metadata, and renders `<img src="@photo.Url">`. If linking fails, the client removes the just-uploaded Oqtane file so it does not remain orphaned. If an assessment or an individual photo is deleted, the service removes the physical Oqtane file—including generated thumbnail variants—before removing the association row.

## Access control

All Assessment endpoints require authentication. The existing mentor scoping is retained: when the requester has the Mentor role, the service permits operations only for assessments whose grower is assigned to that mentor. Centre staff use the normal broader assessment access path. The Oqtane file URL itself is served only when the current user has View permission on the private `AssessmentPhotos` folder.

## Deployment and versioning

Assessment module version **2.9.0** contains this feature. Before deploying application binaries, run `Sql/Migration_AddAssessmentPhotoTable.sql` using the approved SQL-project publishing workflow. The migration is re-runnable for the Oqtane-reference schema. It intentionally stops if it detects the retired `PhotoData` binary column, preventing accidental mixed storage.

After application deployment, restart or recycle Oqtane as required by the target environment. Verify the site’s Oqtane `UploadableFiles` setting includes `jpg,jpeg,png,webp`; otherwise the framework will reject the selected image before the module association endpoint runs.

## Verification

Automated rules tests cover allowed extensions, rejected extensions, the deterministic `assessment-{AssessmentId}-{AssessmentPhotoId}` naming convention, invalid naming input, and the five-photo/5-MB limits. Manual acceptance testing should save a new assessment, reopen it, upload a supported image, verify the photo appears through its Oqtane URL, delete it, verify both database association and physical file are removed, and confirm a sixth photo and an oversized file are rejected.

## Related pages

- [Garden Assessment](/Specifications/Garden-Assessment) — user-facing workflow and acceptance scenarios.
- [Role-Based Data Visibility](/Specifications/Role-Based-Data-Visibility) — program role expectations.
- [Localization](/Specifications/Localization) — English and Xitsonga form text.
