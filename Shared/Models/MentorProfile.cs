using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace OpenEug.TenTrees.Models
{
    [Table("MentorProfile")]
    public class MentorProfile : ModelBase
    {
        [Key]
        public string MentorId { get; set; }

        [Required]
        public int VillageId { get; set; }
    }
}
