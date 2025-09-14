using System;
using System.ComponentModel.DataAnnotations;
using Oqtane.Models;

namespace OE.TenTrees.Models
{
    public class TreeMonitoring : IAuditable
    {
        [Key]
        public int TreeMonitoringId { get; set; }
        
        public int ModuleId { get; set; }
        
        [Required]
        public int TreeId { get; set; }
        
        public Tree? Tree { get; set; }
        
        [Required]
        public DateTime MonitoringDate { get; set; }
        
        [Required]
        [StringLength(50)]
        public string HealthStatus { get; set; } = "Healthy";
        
        public double? Height { get; set; }
        
        public double? Diameter { get; set; }
        
        [StringLength(500)]
        public string? Observations { get; set; }
        
        [StringLength(100)]
        public string? MonitoredBy { get; set; }
        
        [StringLength(200)]
        public string? PhotoUrl { get; set; }
        
        public bool RequiresAttention { get; set; } = false;
        
        [StringLength(300)]
        public string? RecommendedActions { get; set; }

        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; } = string.Empty;
        public DateTime ModifiedOn { get; set; }
    }
}