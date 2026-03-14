/*
 * Migration Script: Rename Enrollment columns (Issue #19)
 * BeneficiaryName → GrowerName
 * EvaluatorName → TreeMentorName
 * 
 * This script safely renames columns while preserving all existing data.
 */

-- Check if old column names exist before renaming
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Enrollment') AND name = 'BeneficiaryName')
BEGIN
    PRINT 'Renaming BeneficiaryName to GrowerName...';
    EXEC sp_rename 'dbo.Enrollment.BeneficiaryName', 'GrowerName', 'COLUMN';
    PRINT 'BeneficiaryName renamed successfully.';
END
ELSE
BEGIN
    PRINT 'BeneficiaryName column not found or already renamed.';
END

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Enrollment') AND name = 'EvaluatorName')
BEGIN
    PRINT 'Renaming EvaluatorName to TreeMentorName...';
    EXEC sp_rename 'dbo.Enrollment.EvaluatorName', 'TreeMentorName', 'COLUMN';
    PRINT 'EvaluatorName renamed successfully.';
END
ELSE
BEGIN
    PRINT 'EvaluatorName column not found or already renamed.';
END

PRINT 'Migration completed. No data was lost.';
GO
