using System.ComponentModel.DataAnnotations;

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

        [Required]
        [StringLength(256)]
        public string Username { get; set; }

        [Required]
        [StringLength(256)]
        public string DisplayName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; }

        public bool IsDeleted { get; set; }

        // From MentorProfile
        [Range(1, int.MaxValue, ErrorMessage = "A valid Village must be selected.")]
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
