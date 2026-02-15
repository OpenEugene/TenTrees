using System;

namespace OpenEug.TenTrees.Models
{
    /// <summary>
    /// ViewModel for displaying Enrollment data with related entity information
    /// </summary>
    public class EnrollmentViewModel
    {
        // Core Enrollment properties
        public int EnrollmentId { get; set; }
        public int ModuleId { get; set; }
        public string GrowerName { get; set; }
        public int VillageId { get; set; }
        public string HouseNumber { get; set; }
        public string IdNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool OwnsHome { get; set; }
        public int HouseholdSize { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public int MentorId { get; set; }
        public string TreeMentorName { get; set; }
        
        // Preferred Criteria
        public bool EnrolledInPE { get; set; }
        public bool PEGraduate { get; set; }
        public bool GardenPlantedAndTended { get; set; }
        public bool ChildHeadedHousehold { get; set; }
        public bool WomanHeadedHousehold { get; set; }
        public bool EmptyYard { get; set; }
        
        // Commitments
        public bool CommitNoChemicals { get; set; }
        public bool CommitAttendClasses { get; set; }
        public bool CommitNoCuttingTrees { get; set; }
        public bool CommitStandForWomenChildren { get; set; }
        public bool CommitCareWhileAway { get; set; }
        public bool CommitAllowYardAccess { get; set; }
        
        // Signature
        public string SignatureData { get; set; }
        public bool SignatureCollected { get; set; }
        public DateTime? SignatureDate { get; set; }
        
        public EnrollmentStatus Status { get; set; }
        
        // Audit fields
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        
        // ViewModel-specific properties (joined/computed data)
        public string VillageName { get; set; }
    }
}
