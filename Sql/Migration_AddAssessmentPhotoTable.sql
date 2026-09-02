-- Adds Oqtane file references for optional assessment problem photos.
-- Oqtane owns the physical file in its private AssessmentPhotos folder; this
-- module stores only the assessment association, Oqtane PhotoId, and file URL.

SET XACT_ABORT ON;
GO

BEGIN TRANSACTION;
GO

IF OBJECT_ID(N'[dbo].[AssessmentPhoto]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[AssessmentPhoto] (
        [AssessmentPhotoId] INT             IDENTITY (1, 1) NOT NULL,
        [AssessmentId]      INT             NOT NULL,
        [PhotoId]           INT             NOT NULL,
        [Url]               NVARCHAR (2048) NOT NULL,
        [CreatedBy]         NVARCHAR (256)  NOT NULL,
        [CreatedOn]         DATETIME2 (7)   NOT NULL,
        [ModifiedBy]        NVARCHAR (256)  NOT NULL,
        [ModifiedOn]        DATETIME2 (7)   NOT NULL,
        CONSTRAINT [PK_AssessmentPhoto] PRIMARY KEY CLUSTERED ([AssessmentPhotoId] ASC),
        CONSTRAINT [UQ_AssessmentPhoto_AssessmentId_PhotoId] UNIQUE ([AssessmentId], [PhotoId])
    );

    CREATE NONCLUSTERED INDEX [IX_AssessmentPhoto_AssessmentId]
        ON [dbo].[AssessmentPhoto] ([AssessmentId] ASC);
END;
GO

IF COL_LENGTH(N'[dbo].[AssessmentPhoto]', N'PhotoData') IS NOT NULL
BEGIN
    THROW 50001, 'AssessmentPhoto uses the retired binary schema. Do not apply this migration until those un-deployed records have been removed or migrated to Oqtane files.', 1;
END;
GO

IF COL_LENGTH(N'[dbo].[AssessmentPhoto]', N'PhotoId') IS NULL
   OR COL_LENGTH(N'[dbo].[AssessmentPhoto]', N'Url') IS NULL
BEGIN
    THROW 50002, 'AssessmentPhoto exists but does not match the required Oqtane PhotoId and Url schema.', 1;
END;
GO

COMMIT TRANSACTION;
GO
