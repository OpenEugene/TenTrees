using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace OpenEug.TenTrees.Models
{
    [Table("Tree")]
    public class Tree : ModelBase
    {
        [Key]
        public int TreeId { get; set; }

        [Required]
        public int OrchardId { get; set; }

        [Required]
        public int TreeTypeId { get; set; }

        public int Quantity { get; set; } = 1;

        [MaxLength(500)]
        public string? Notes { get; set; }
    }
}
