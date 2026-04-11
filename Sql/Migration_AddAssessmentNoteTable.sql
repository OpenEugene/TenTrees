-- Migration: add [dbo].[AssessmentNote] table, migrate any existing
-- [dbo].[Assessment].[Notes] content into the new table as a single
-- General-typed log entry, then drop the old Notes column.
--
-- Safe to re-run: every step is guarded with an existence check.
-- Run once against each target database after deploying the code change.

SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRANSACTION;

-- 1. Create the AssessmentNote table if it doesn't already exist.
IF NOT EXISTS (
    SELECT 1 FROM sys.tables
    WHERE object_id = OBJECT_ID(N'[dbo].[AssessmentNote]')
)
BEGIN
    CREATE TABLE [dbo].[AssessmentNote] (
        [AssessmentNoteId] INT            IDENTITY (1, 1) NOT NULL,
        [AssessmentId]     INT            NOT NULL,
        [NoteType]         NVARCHAR (20)  NOT NULL,
        [Text]             NVARCHAR (MAX) NOT NULL,
        [CreatedBy]        NVARCHAR (256) NOT NULL,
        [CreatedOn]        DATETIME2 (7)  NOT NULL,
        [ModifiedBy]       NVARCHAR (256) NOT NULL,
        [ModifiedOn]       DATETIME2 (7)  NOT NULL,
        CONSTRAINT [PK_AssessmentNote] PRIMARY KEY CLUSTERED ([AssessmentNoteId] ASC),
        CONSTRAINT [FK_AssessmentNote_Assessment] FOREIGN KEY ([AssessmentId])
            REFERENCES [dbo].[Assessment] ([AssessmentId]) ON DELETE CASCADE
    );

    CREATE NONCLUSTERED INDEX [IX_AssessmentNote_AssessmentId]
        ON [dbo].[AssessmentNote] ([AssessmentId] ASC);
END;

-- 2. If the legacy Notes column still exists, copy every non-empty value
--    into AssessmentNote as a General-typed entry stamped with the
--    assessment's original CreatedBy / CreatedOn so history is preserved.
IF EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID(N'[dbo].[Assessment]') AND name = N'Notes'
)
BEGIN
    -- Guard against repeat runs: only insert notes for assessments that
    -- don't already have a migrated entry.
    DECLARE @sql NVARCHAR(MAX) = N'
        INSERT INTO [dbo].[AssessmentNote]
            ([AssessmentId], [NoteType], [Text], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
        SELECT
            a.[AssessmentId],
            N''General'',
            a.[Notes],
            a.[CreatedBy],
            a.[CreatedOn],
            a.[ModifiedBy],
            a.[ModifiedOn]
        FROM [dbo].[Assessment] a
        WHERE a.[Notes] IS NOT NULL
          AND LTRIM(RTRIM(a.[Notes])) <> N''''
          AND NOT EXISTS (
              SELECT 1 FROM [dbo].[AssessmentNote] n
              WHERE n.[AssessmentId] = a.[AssessmentId]
          );
    ';
    EXEC sp_executesql @sql;

    -- 3. Drop the legacy Notes column now that its data has been migrated.
    ALTER TABLE [dbo].[Assessment] DROP COLUMN [Notes];
END;

COMMIT TRANSACTION;
