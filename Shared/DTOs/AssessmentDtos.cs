using System;

namespace OpenEug.TenTrees.Models
{
    public class AssessmentListDto
    {
        public int AssessmentId { get; set; }
        public DateTime AssessmentDate { get; set; }
        public int GrowerId { get; set; }
        public string GrowerName { get; set; }
        public int VillageId { get; set; }
        public string VillageName { get; set; }
        public string MentorUsername { get; set; }
        public int TreesPlanted { get; set; }
        public int TreesAlive { get; set; }
        public int PermaculturePrinciplesCount { get; set; }
        public bool NeedsHelp { get; set; }
    }

    public class AssessmentPhotoDto
    {
        public int AssessmentPhotoId { get; set; }
        public int AssessmentId { get; set; }
        public int PhotoId { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string Url { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
