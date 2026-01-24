-- ============================================
-- 10 Trees Custom Roles Setup Script
-- ============================================
-- This script creates the custom roles needed for the 10 Trees application.
-- Run this script AFTER Oqtane installation and site creation.
-- Replace @SiteId with your actual Site ID (usually 1 for the default site)
--
-- To find your SiteId, run:  SELECT * FROM [dbo].[Site]
-- ============================================

DECLARE @SiteId INT = 1  -- CHANGE THIS to your actual SiteId

-- Mentor Role
IF NOT EXISTS (SELECT 1 FROM [dbo].[Role] WHERE [Name] = 'Mentor' AND [SiteId] = @SiteId)
BEGIN
    INSERT INTO [dbo].[Role] ([SiteId], [Name], [Description], [IsAutoAssigned], [IsSystem], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
    VALUES (
        @SiteId,
        'Mentor',
        'Field mentor who submits forms and views assigned village data only',
        0,
        1,
        'Admin',
        GETDATE(),
        'Admin',
        GETDATE()
    )
    PRINT 'Created Mentor role'
END
ELSE
    PRINT 'Mentor role already exists'

-- Educator Role
IF NOT EXISTS (SELECT 1 FROM [dbo].[Role] WHERE [Name] = 'Educator' AND [SiteId] = @SiteId)
BEGIN
    INSERT INTO [dbo].[Role] ([SiteId], [Name], [Description], [IsAutoAssigned], [IsSystem], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
    VALUES (
        @SiteId,
        'Educator',
        'Educator who submits forms, views all villages, and exports data',
        0,
        1,
        'Admin',
        GETDATE(),
        'Admin',
        GETDATE()
    )
    PRINT 'Created Educator role'
END
ELSE
    PRINT 'Educator role already exists'

-- Project Manager Role
IF NOT EXISTS (SELECT 1 FROM [dbo].[Role] WHERE [Name] = 'Project Manager' AND [SiteId] = @SiteId)
BEGIN
    INSERT INTO [dbo].[Role] ([SiteId], [Name], [Description], [IsAutoAssigned], [IsSystem], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
    VALUES (
        @SiteId,
        'Project Manager',
        'Project manager with same permissions as Educator',
        0,
        1,
        'Admin',
        GETDATE(),
        'Admin',
        GETDATE()
    )
    PRINT 'Created Project Manager role'
END
ELSE
    PRINT 'Project Manager role already exists'

-- Executive Director Role
IF NOT EXISTS (SELECT 1 FROM [dbo].[Role] WHERE [Name] = 'Executive Director' AND [SiteId] = @SiteId)
BEGIN
    INSERT INTO [dbo].[Role] ([SiteId], [Name], [Description], [IsAutoAssigned], [IsSystem], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn])
    VALUES (
        @SiteId,
        'Executive Director',
        'Executive director with full permissions including user management',
        0,
        1,
        'Admin',
        GETDATE(),
        'Admin',
        GETDATE()
    )
    PRINT 'Created Executive Director role'
END
ELSE
    PRINT 'Executive Director role already exists'

GO

-- Verify roles were created
SELECT [RoleId], [SiteId], [Name], [Description], [IsSystem]
FROM [dbo].[Role]
WHERE [Name] IN ('Mentor', 'Educator', 'Project Manager', 'Executive Director')
ORDER BY [Name]
