using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace OpenEug.TenTrees.Models
{
    [Table("OpenEug.TenTreesEnrollment")]
    public class Enrollment : ModelBase
    {
        [Key]
        public int EnrollmentId { get; set; }
        
        public int ModuleId { get; set; }
        
        // Participant Information (creates Participant record)
        public int? ParticipantId { get; set; }
        
        [Required(ErrorMessage = "Beneficiary name is required")]
        public string BeneficiaryName { get; set; }
        
        [Required(ErrorMessage = "Village is required")]
        public int VillageId { get; set; }
        
        public string HouseNumber { get; set; }
        
        public string IdNumber { get; set; }
        
        public DateTime? BirthDate { get; set; }
        
        [Required]
        public int HouseholdSize { get; set; }
        
        public bool OwnsHome { get; set; }
        
        // Enrollment metadata
        [Required]
        public int MentorId { get; set; }
        
        public string EvaluatorName { get; set; }
        
        public DateTime EnrollmentDate { get; set; }
        
        // Preferred Criteria (Yes/No questions)
        public bool EnrolledInPE { get; set; }
        
        public bool PEGraduate { get; set; }
        
        public bool GardenPlantedAndTended { get; set; }
        
        public bool ChildHeadedHousehold { get; set; }
        
        public bool WomanHeadedHousehold { get; set; }
        
        public bool EmptyYard { get; set; }
        
        // Commitments (acknowledgments)
        public bool CommitNoChemicals { get; set; }
        
        public bool CommitAttendClasses { get; set; }
        
        public bool CommitNoCuttingTrees { get; set; }
        
        public bool CommitStandForWomenChildren { get; set; }
        
        public bool CommitCareWhileAway { get; set; }
        
        public bool CommitAllowYardAccess { get; set; }
        
        // E-Signature
        public bool SignatureCollected { get; set; }
        
        public DateTime? SignatureDate { get; set; }
        
        public string SignatureData { get; set; }
        
        // Status
        public EnrollmentStatus Status { get; set; }
    }
    
    public enum EnrollmentStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2
    }
}
