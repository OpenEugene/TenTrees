-- Migration: add AssessmentFrequencyDays to Cohort table
-- Run once against the target database.

IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID(N'[dbo].[Cohort]') AND name = N'AssessmentFrequencyDays'
)
BEGIN
    ALTER TABLE [dbo].[Cohort]
        ADD [AssessmentFrequencyDays] INT NOT NULL DEFAULT 30;
END
