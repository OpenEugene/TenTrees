using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenEug.TenTrees.Models
{
    [Table("CohortClass")]
    public class CohortClass
    {
        [Key]
        public int CohortClassId { get; set; }

        public int? CohortId { get; set; }

        public int? TrainingClassId { get; set; }
    }
}
