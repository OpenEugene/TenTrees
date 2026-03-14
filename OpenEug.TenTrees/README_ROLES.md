# 10 Trees Custom Roles Setup

## Overview
The 10 Trees application requires four custom roles beyond the standard Oqtane roles. These roles must be created manually through the Oqtane admin UI or by running the provided SQL script.

## Required Custom Roles

| Role | Description | Permissions |
|------|-------------|-------------|
| **Mentor** | Field mentor who submits forms and views assigned village data only | Submit forms, view assigned village only |
| **Educator** | Educator who submits forms, views all villages, and exports data | Submit forms, view all villages, export data |
| **Project Manager** | Project manager with same permissions as Educator | Submit forms, view all villages, export data |
| **Executive Director** | Executive director with full permissions including user management | Submit forms, view all villages, export data, manage users |

## Setup Methods

### Method 1: Manual Creation (Recommended for Production)

1. Log in to Oqtane as an Administrator
2. Navigate to **Control Panel** ? **User Management** ? **Roles**
3. Click **Add Role** and create each role with these settings:
   - **Name**: (See table above)
   - **Description**: (See table above)
   - **Is Auto Assigned**: No
   - **Is System**: Yes (recommended to prevent accidental deletion)

4. Repeat for all four roles

### Method 2: SQL Script (Quick Setup for Development)

1. Locate your SiteId:
   ```sql
   SELECT SiteId, Name FROM [dbo].[Site]
   ```

2. Open `Server/Migrations/Scripts/CreateCustomRoles.sql`

3. Update the `@SiteId` variable at the top of the script:
   ```sql
   DECLARE @SiteId INT = 1  -- CHANGE THIS to your actual SiteId
   ```

4. Execute the script in SQL Server Management Studio or Azure Data Studio

5. Verify the roles were created:
   ```sql
   SELECT [RoleId], [SiteId], [Name], [Description], [IsSystem]
   FROM [dbo].[Role]
   WHERE [Name] IN ('Mentor', 'Educator', 'Project Manager', 'Executive Director')
   ORDER BY [Name]
   ```

## Using Roles in Code

The `Shared/RoleNames.cs` class provides type-safe constants for all roles:

```csharp
using OpenEug.TenTrees.Shared;

// Check specific role
if (User.IsInRole(RoleNames.Mentor))
{
    // Mentor-specific logic
}

// Check permissions
if (RoleNames.CanViewAllVillages(userRole))
{
    // Show all village data
}

if (RoleNames.CanExportData(userRole))
{
    // Enable export functionality
}

// Controller authorization
[Authorize(Roles = RoleNames.Educator + "," + RoleNames.ProjectManager + "," + RoleNames.Admin)]
public async Task<ActionResult> GetAllVillages()
{
    // ...
}
```

## Assigning Users to Roles

1. Navigate to **Control Panel** ? **User Management** ? **Users**
2. Select a user
3. Click **Edit Roles**
4. Select the appropriate role(s)
5. Click **Save**

## Village Assignment for Mentors

Mentors must also be assigned to a specific village:

1. After assigning the Mentor role
2. Navigate to the Village module
3. Edit the user's profile to set their assigned village
4. The mentor will only see data for that village

## Troubleshooting

**Roles not appearing in the UI:**
- Verify the roles were created in the correct Site (check SiteId)
- Refresh the browser cache
- Check that IsSystem is set to 1 (true)

**Permission denied errors:**
- Verify the user has the correct role assigned
- Check that the role name exactly matches the constants in `RoleNames.cs`
- Ensure the user is logged in and authenticated

## Related Files
- `Shared/RoleNames.cs` - Role name constants and permission helpers
- `Server/Migrations/Scripts/CreateCustomRoles.sql` - SQL script for role creation
- `Specs/Features/UserAdministration.feature` - BDD scenarios for role behavior
