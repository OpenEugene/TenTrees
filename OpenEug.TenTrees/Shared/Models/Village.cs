using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace OpenEug.TenTrees.Models
{
    [Table("OpenEug.TenTreesVillage")]
    public class Village : ModelBase
    {
        [Key]
        public int VillageId { get; set; }
        
        [Required]
        public string VillageName { get; set; }
        
        public string ContactName { get; set; }
        
        public string ContactPhone { get; set; }
        
        public string ContactEmail { get; set; }
        
        public string Notes { get; set; }
        
        public bool IsActive { get; set; }
    }
}
