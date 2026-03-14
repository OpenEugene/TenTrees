namespace OpenEug.TenTrees.Shared
{
    /// <summary>
    /// Constants for role names used in the 10 Trees application.
    /// Custom roles are defined in the AddCustomRoles migration.
    /// Built-in Oqtane roles are included for reference.
    /// </summary>
    public static class RoleNames
    {
        // ========== Custom 10 Trees Roles ==========
        
        /// <summary>
        /// Mentor - Field mentor who submits forms and views assigned village data only
        /// Permissions: Submit forms, view assigned village only
        /// </summary>
        public const string Mentor = "Mentor";

        /// <summary>
        /// Educator - Educator who submits forms, views all villages, and exports data
        /// Permissions: Submit forms, view all villages, export data
        /// </summary>
        public const string Educator = "Educator";

        /// <summary>
        /// Project Manager - Project manager with same permissions as Educator
        /// Permissions: Submit forms, view all villages, export data
        /// </summary>
        public const string ProjectManager = "Project Manager";

        /// <summary>
        /// Executive Director - Executive director with full permissions including user management
        /// Permissions: Submit forms, view all villages, export data, manage users
        /// </summary>
        public const string ExecutiveDirector = "Executive Director";

        // ========== Built-in Oqtane Roles ==========
        
        /// <summary>
        /// Administrators - Built-in Oqtane administrator role with full system access
        /// Permissions: Full access including user management and system configuration
        /// </summary>
        public const string Admin = "Administrators";

        /// <summary>
        /// Registered Users - Built-in Oqtane role for all authenticated users
        /// </summary>
        public const string RegisteredUsers = "Registered Users";

        /// <summary>
        /// All Users - Built-in Oqtane role that includes both authenticated and unauthenticated users
        /// </summary>
        public const string AllUsers = "All Users";

        /// <summary>
        /// Host Users - Built-in Oqtane role for multi-tenant host administrators
        /// </summary>
        public const string HostUsers = "Host Users";

        /// <summary>
        /// Checks if a role can view all villages (not restricted to a single village)
        /// </summary>
        public static bool CanViewAllVillages(string roleName)
        {
            return roleName == Educator 
                || roleName == ProjectManager 
                || roleName == ExecutiveDirector 
                || roleName == Admin;
        }

        /// <summary>
        /// Checks if a role can export data
        /// </summary>
        public static bool CanExportData(string roleName)
        {
            return roleName == Educator 
                || roleName == ProjectManager 
                || roleName == ExecutiveDirector 
                || roleName == Admin;
        }

        /// <summary>
        /// Checks if a role can manage users
        /// </summary>
        public static bool CanManageUsers(string roleName)
        {
            return roleName == ExecutiveDirector 
                || roleName == Admin;
        }
    }
}
