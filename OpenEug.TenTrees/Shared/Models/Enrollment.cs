using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace OpenEug.TenTrees.Module.Enrollment.Models
{
    [Table("OpenEug.TenTreesEnrollment")]
    public class Enrollment : ModelBase
    {
        [Key]
        public int EnrollmentId { get; set; }
        public int ModuleId { get; set; }
        public string Name { get; set; }
    }
}
