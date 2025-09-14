using System;
using System.ComponentModel.DataAnnotations;
using Oqtane.Models;

namespace OE.TenTrees.Models
{
    public class PlantingEvent : IAuditable
    {
        [Key]
        public int PlantingEventId { get; set; }
        
        public int ModuleId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string EventName { get; set; } = string.Empty;
        
        [Required]
        public DateTime PlantingDate { get; set; }
        
        [Required]
        public double Latitude { get; set; }
        
        [Required]
        public double Longitude { get; set; }
        
        [StringLength(200)]
        public string? Location { get; set; }
        
        [Required]
        public int TreesPlanted { get; set; }
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        [StringLength(100)]
        public string? Organizer { get; set; }
        
        [StringLength(100)]
        public string? Sponsor { get; set; }
        
        public int? ParticipantCount { get; set; }
        
        [StringLength(1000)]
        public string? Notes { get; set; }

        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; } = string.Empty;
        public DateTime ModifiedOn { get; set; }
    }
}