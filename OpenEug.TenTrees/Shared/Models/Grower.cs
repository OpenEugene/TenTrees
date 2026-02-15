using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace OpenEug.TenTrees.Models
{
    [Table("Grower")]
    public class Grower : ModelBase
    {
        [Key]
        public int GrowerId { get; set; }

        [Required]
        public int VillageId { get; set; }

        public int? MentorId { get; set; }

        [Required]
        public string GrowerName { get; set; }

        public string HouseNumber { get; set; }

        public string IdNumber { get; set; }

        public DateTime? BirthDate { get; set; }

        public int HouseholdSize { get; set; }

        public bool OwnsHome { get; set; }

        public GrowerStatus Status { get; set; }

        public DateTime? ExitDate { get; set; }

        public string ExitReason { get; set; }
    }

    public enum GrowerStatus
    {
        Active = 0,
        Exited = 1,
        Suspended = 2
    }
}
