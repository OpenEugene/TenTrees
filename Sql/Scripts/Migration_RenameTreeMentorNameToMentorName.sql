-- Migration: Rename TreeMentorName → MentorName on Enrollment table
-- Reason: Simplified naming — "Tree" prefix was redundant in this context.

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Enrollment') AND name = 'TreeMentorName')
BEGIN
    PRINT 'Renaming TreeMentorName to MentorName...';
    EXEC sp_rename 'dbo.Enrollment.TreeMentorName', 'MentorName', 'COLUMN';
    PRINT 'Done.';
END
ELSE
BEGIN
    PRINT 'Column TreeMentorName not found — skipping (already renamed or not present).';
END
