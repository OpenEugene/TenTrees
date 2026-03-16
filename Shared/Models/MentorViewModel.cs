namespace OpenEug.TenTrees.Models
{
    /// <summary>
    /// Combined view of an Oqtane User in the Mentor role plus their MentorProfile data.
    /// Used for list, create, and edit operations in the Mentor management module.
    /// </summary>
    public class MentorViewModel
    {
        // Oqtane user fields
        public int UserId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public bool IsDeleted { get; set; }

        // From MentorProfile
        public int VillageId { get; set; }

        // Resolved from Village (read-only, populated server-side for lists)
        public string VillageName { get; set; }

        // Resolved from Grower table (read-only, populated server-side for lists)
        public int GrowerCount { get; set; }

        // For create form only — never returned from server
        public string Password { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}
