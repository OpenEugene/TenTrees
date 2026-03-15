using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenEug.TenTrees.Models
{
    [Table("MentorCohort")]
    public class MentorCohort
    {
        [Key]
        public int MentorCohortId { get; set; }

        public string MentorId { get; set; }

        public int CohortId { get; set; }

        public DateTime AssignedOn { get; set; }
    }
}
