using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace OpenEug.TenTrees.Models
{
    [Table("TrainingClass")]
    public class TrainingClass : ModelBase
    {
        [Key]
        public int TrainingClassId { get; set; }

        public int ModuleId { get; set; }

        [Required]
        public int VillageId { get; set; }

        [Required]
        public string ClassName { get; set; }

        public DateTime ClassDate { get; set; }

        [Range(1, 5)]
        public int ClassNumber { get; set; }

        public string Notes { get; set; }
    }
}
