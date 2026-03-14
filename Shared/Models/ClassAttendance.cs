using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace OpenEug.TenTrees.Models
{
    [Table("ClassAttendance")]
    public class ClassAttendance : ModelBase
    {
        [Key]
        public int ClassAttendanceId { get; set; }

        [Required]
        public int TrainingClassId { get; set; }

        [Required]
        public int GrowerId { get; set; }

        public bool IsPresent { get; set; }

        public int? MarkedByUserId { get; set; }
    }
}
