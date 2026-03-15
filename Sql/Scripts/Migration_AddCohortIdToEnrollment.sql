-- Migration: add CohortId to Enrollment table
-- Run once against the target database.

IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID(N'[dbo].[Enrollment]') AND name = N'CohortId'
)
BEGIN
    ALTER TABLE [dbo].[Enrollment]
        ADD [CohortId] INT NULL;
END
