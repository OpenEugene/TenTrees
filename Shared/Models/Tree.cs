using System;
using System.ComponentModel.DataAnnotations;
using Oqtane.Models;

namespace OE.TenTrees.Models
{
    public class Tree : IAuditable
    {
        [Key]
        public int TreeId { get; set; }
        
        public int ModuleId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Species { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string CommonName { get; set; } = string.Empty;
        
        [Required]
        public double Latitude { get; set; }
        
        [Required]
        public double Longitude { get; set; }
        
        [StringLength(200)]
        public string? Location { get; set; }
        
        public DateTime PlantedDate { get; set; }
        
        [StringLength(50)]
        public string Status { get; set; } = "Healthy";
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        public double? Height { get; set; }
        
        public double? Diameter { get; set; }
        
        [StringLength(100)]
        public string? PlantedBy { get; set; }

        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; } = string.Empty;
        public DateTime ModifiedOn { get; set; }
    }
}