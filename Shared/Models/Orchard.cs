using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace OpenEug.TenTrees.Models
{
    [Table("Orchard")]
    public class Orchard : ModelBase
    {
        [Key]
        public int OrchardId { get; set; }

        [Required]
        public int GrowerId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }
    }
}
