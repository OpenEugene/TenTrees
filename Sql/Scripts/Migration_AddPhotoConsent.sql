/*
 * Migration Script: Add Photo Release Consent columns to Enrollment table
 * Feature: PhotoReleaseConsent
 *
 * Run once against the live database after deploying the updated application.
 * Safe to re-run — each block checks for column existence first.
 */

IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('dbo.Enrollment') AND name = 'PhotoConsentLevel'
)
BEGIN
    PRINT 'Adding PhotoConsentLevel column...';
    ALTER TABLE [dbo].[Enrollment]
        ADD [PhotoConsentLevel] INT NOT NULL DEFAULT 0;
    PRINT 'PhotoConsentLevel added.';
END
ELSE
    PRINT 'PhotoConsentLevel already exists — skipped.';

IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('dbo.Enrollment') AND name = 'PhotoConsentSignatureCollected'
)
BEGIN
    PRINT 'Adding PhotoConsentSignatureCollected column...';
    ALTER TABLE [dbo].[Enrollment]
        ADD [PhotoConsentSignatureCollected] BIT NOT NULL DEFAULT 0;
    PRINT 'PhotoConsentSignatureCollected added.';
END
ELSE
    PRINT 'PhotoConsentSignatureCollected already exists — skipped.';

IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('dbo.Enrollment') AND name = 'PhotoConsentSignatureDate'
)
BEGIN
    PRINT 'Adding PhotoConsentSignatureDate column...';
    ALTER TABLE [dbo].[Enrollment]
        ADD [PhotoConsentSignatureDate] DATETIME2(7) NULL;
    PRINT 'PhotoConsentSignatureDate added.';
END
ELSE
    PRINT 'PhotoConsentSignatureDate already exists — skipped.';

IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('dbo.Enrollment') AND name = 'PhotoConsentSignatureData'
)
BEGIN
    PRINT 'Adding PhotoConsentSignatureData column...';
    ALTER TABLE [dbo].[Enrollment]
        ADD [PhotoConsentSignatureData] NVARCHAR(MAX) NULL;
    PRINT 'PhotoConsentSignatureData added.';
END
ELSE
    PRINT 'PhotoConsentSignatureData already exists — skipped.';

PRINT 'Photo consent migration complete.';
GO
