using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace OpenEug.TenTrees.Module.Enrollment.Models
{
    [Table("OpenEug.TenTreesParticipant")]
    public class Participant : ModelBase
    {
        [Key]
        public int ParticipantId { get; set; }
        
        [Required]
        public int VillageId { get; set; }
        
        public int? MentorId { get; set; }
        
        [Required]
        public string BeneficiaryName { get; set; }
        
        public string HouseNumber { get; set; }
        
        public string IdNumber { get; set; }
        
        public DateTime? BirthDate { get; set; }
        
        public int HouseholdSize { get; set; }
        
        public bool OwnsHome { get; set; }
        
        public ParticipantStatus Status { get; set; }
        
        public DateTime? ExitDate { get; set; }
        
        public string ExitReason { get; set; }
    }
    
    public enum ParticipantStatus
    {
        Active = 0,
        Exited = 1,
        Suspended = 2
    }
}
