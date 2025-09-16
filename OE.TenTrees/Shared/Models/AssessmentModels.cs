using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Oqtane.Models;

namespace OE.TenTrees.Models
{
    public class SiteAssessment : IAuditable
    {
        [Key]
        public int AssessmentId { get; set; }
        public int ApplicationId { get; set; }
        public DateTime AssessmentDate { get; set; }
        public string AssessorUserId { get; set; }
        public AssessmentOutcome Outcome { get; set; }
        public string SoilDescription { get; set; }
        public string SunExposure { get; set; }
        public string WaterAvailability { get; set; }
        public string Constraints { get; set; }
        public string RecommendedSpecies { get; set; }
        public int RecommendedTreeCount { get; set; }
        public string Notes { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        // No navigation properties - use LINQ joins instead
    }

    public class AssessmentPhoto : IAuditable
    {
        [Key]
        public int AssessmentPhotoId { get; set; }
        public int AssessmentId { get; set; }
        public string Url { get; set; }
        public string Caption { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        
        // No navigation properties - use LINQ joins instead
    }

    public class SiteAssessmentVm
    {
        public SiteAssessment Assessment { get; set; }
        public IEnumerable<AssessmentPhoto> Photos { get; set; }
    }
}
