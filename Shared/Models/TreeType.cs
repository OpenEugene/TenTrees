using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace OpenEug.TenTrees.Models
{
    [Table("TreeType")]
    public class TreeType : ModelBase
    {
        [Key]
        public int TreeTypeId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        [MaxLength(200)]
        public string Category { get; set; }

        public int SortOrder { get; set; }

        public bool IsActive { get; set; }
    }
}
