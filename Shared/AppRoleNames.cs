namespace OpenEug.TenTrees.Shared
{
    /// <summary>
    /// Constants for role names used in the 10 Trees application.
    /// This class is the source of truth for the role name values used by application code.
    /// Built-in Oqtane roles are included for reference alongside application-specific roles.
    /// Role provisioning and assignment are configured outside this constants file.
    /// </summary>
    public static class AppRoleNames
    {
        // ========== Custom 10 Trees Roles ==========

        /// <summary>
        /// Mentor - Field mentor who submits forms and views assigned village/cohort data only
        /// Permissions: Submit forms, view assigned village/cohort only
        /// </summary>
        public const string Mentor = "Mentor";

        /// <summary>
        /// Educator - Educator who views all villages, adds home-visit notes, and marks class attendance
        /// Permissions: View all villages, add notes, mark attendance
        /// </summary>
        public const string Educator = "Educator";

        /// <summary>
        /// Project Manager - Same as Educator, plus data export and full reporting access
        /// Permissions: View all villages, add notes, mark attendance, export data, view reports
        /// </summary>
        public const string ProjectManager = "Project Manager";

        /// <summary>
        /// 10 Trees Admin - Programme-level administrator with full data access and user management.
        /// Separate from the Oqtane platform Administrators role.
        /// </summary>
        public const string TenTreesAdmin = "10Trees Admin";

        // ========== Built-in Oqtane Roles ==========

        /// <summary>
        /// Administrators - Built-in Oqtane platform role. Use TenTreesAdmin for programme-level
        /// admin checks; reserve this for Oqtane-level permission API calls only.
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
                || roleName == TenTreesAdmin;
        }

        /// <summary>
        /// Checks if a role can export data
        /// </summary>
        public static bool CanExportData(string roleName)
        {
            return roleName == ProjectManager
                || roleName == TenTreesAdmin;
        }

        /// <summary>
        /// Checks if a role can manage users
        /// </summary>
        public static bool CanManageUsers(string roleName)
        {
            return roleName == TenTreesAdmin;
        }
    }
}
