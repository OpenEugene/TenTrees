using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenEug.TenTrees.Models
{
    [Table("GrowerCohort")]
    public class GrowerCohort
    {
        [Key]
        public int GrowerCohortId { get; set; }

        public int GrowerId { get; set; }

        public int CohortId { get; set; }

        public DateTime JoinedOn { get; set; }
    }
}
