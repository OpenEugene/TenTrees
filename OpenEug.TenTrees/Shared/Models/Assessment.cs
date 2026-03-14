using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace OpenEug.TenTrees.Models
{
    [Table("Assessment")]
    public class Assessment : ModelBase
    {
        [Key]
        public int AssessmentId { get; set; }

        public int ModuleId { get; set; }

        [Required]
        public int GrowerId { get; set; }

        public DateTime AssessmentDate { get; set; }

        // Tree Survival
        public int TreesPlanted { get; set; }
        public int TreesAlive { get; set; }
        public string DeceasedTreeTypes { get; set; }

        // Permaculture Practices
        public bool TreesLookHealthy { get; set; }
        public bool HasChemicalFertilizers { get; set; }
        public bool HasPesticides { get; set; }
        public bool IsMulched { get; set; }
        public bool IsMakingCompost { get; set; }
        public bool IsCollectingWater { get; set; }
        public bool HasLeakyTaps { get; set; }
        public bool IsGardenDesignedToCaptureWater { get; set; }
        public bool IsUsingGreywater { get; set; }
        
        public int PermaculturePrinciplesCount { get; set; }

        // Problems
        public bool HasBrokenBranches { get; set; }
        public bool HasYellowLeaves { get; set; }
        public bool HasLosingLeaves { get; set; }
        public bool HasLookDry { get; set; }
        public bool HasPests { get; set; }
        public bool NeedsHelp { get; set; }

        // Notes
        public string Notes { get; set; }

        // Access Control / Submission
        public string SubmittedBy { get; set; }
        public bool EnteredByAdmin { get; set; }
        
        // Frequency Tracking
        public string FrequencyType { get; set; } // e.g., "Twice-Monthly", "Monthly"
    }
}
