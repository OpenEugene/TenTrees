using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace OpenEug.TenTrees.Models
{
    [Table("Cohort")]
    public class Cohort : ModelBase
    {
        [Key]
        public int CohortId { get; set; }

        [Required]
        public int VillageId { get; set; }

        [Required]
        public string Name { get; set; }

        public CohortStatus Status { get; set; }

        public DateTime? ActivatedOn { get; set; }
    }

    public enum CohortStatus
    {
        Planned = 0,
        Active = 1,
        Completed = 2
    }
}
