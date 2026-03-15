-- Migration: Add MentorProfile table
-- Stores village assignment for mentor users (by Oqtane username).
-- Idempotent: safe to run multiple times.

IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.TABLES
    WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'MentorProfile'
)
BEGIN
    CREATE TABLE [dbo].[MentorProfile] (
        [MentorId]   NVARCHAR (256) NOT NULL,
        [VillageId]  INT            NOT NULL,
        [CreatedBy]  NVARCHAR (256) NOT NULL,
        [CreatedOn]  DATETIME2 (7)  NOT NULL,
        [ModifiedBy] NVARCHAR (256) NOT NULL,
        [ModifiedOn] DATETIME2 (7)  NOT NULL,
        CONSTRAINT [PK_MentorProfile] PRIMARY KEY CLUSTERED ([MentorId] ASC)
    );
    PRINT 'MentorProfile table created.';
END
ELSE
BEGIN
    PRINT 'MentorProfile table already exists — skipped.';
END
