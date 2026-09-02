-- Migration: add [dbo].[AssessmentPhoto] for optional photos attached to
-- the Problems section of a garden assessment.
--
-- Safe to re-run: table and index creation are guarded by existence checks.
-- Run once against each target database before deploying the application update.

SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRANSACTION;

IF NOT EXISTS (
    SELECT 1 FROM sys.tables
    WHERE object_id = OBJECT_ID(N'[dbo].[AssessmentPhoto]')
)
BEGIN
    CREATE TABLE [dbo].[AssessmentPhoto] (
        [AssessmentPhotoId] INT             IDENTITY (1, 1) NOT NULL,
        [AssessmentId]      INT             NOT NULL,
        [FileName]          NVARCHAR (255)  NOT NULL,
        [ContentType]       NVARCHAR (50)   NOT NULL,
        [FileSize]          BIGINT          NOT NULL,
        [PhotoData]         VARBINARY (MAX) NOT NULL,
        [CreatedBy]         NVARCHAR (256)  NOT NULL,
        [CreatedOn]         DATETIME2 (7)   NOT NULL,
        [ModifiedBy]        NVARCHAR (256)  NOT NULL,
        [ModifiedOn]        DATETIME2 (7)   NOT NULL,
        CONSTRAINT [PK_AssessmentPhoto] PRIMARY KEY CLUSTERED ([AssessmentPhotoId] ASC)
    );
END;

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'[dbo].[AssessmentPhoto]')
      AND name = N'IX_AssessmentPhoto_AssessmentId'
)
BEGIN
    CREATE NONCLUSTERED INDEX [IX_AssessmentPhoto_AssessmentId]
        ON [dbo].[AssessmentPhoto] ([AssessmentId] ASC);
END;

COMMIT TRANSACTION;
