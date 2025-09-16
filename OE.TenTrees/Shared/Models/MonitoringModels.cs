using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Oqtane.Models;

namespace OE.TenTrees.Models
{
    // Core MonitoringSession entity
    public partial class MonitoringSession : IAuditable
    {
        [Key]
        public int MonitoringSessionId { get; set; }
        public int ApplicationId { get; set; }
        public int? GardenSiteId { get; set; } // New field for garden site reference
        public int ModuleId { get; set; }
        public DateTime SessionDate { get; set; }
        public string ObserverUserId { get; set; }
        public string EvaluatorName { get; set; }
        public string BeneficiaryName { get; set; }
        public string HouseNumber { get; set; }
        public string IdOrBirthdate { get; set; }
        public string WeatherConditions { get; set; }
        public string Notes { get; set; }
        
        // Site Assessment Questions
        public bool? HasWaterInPlot { get; set; }
        public bool? HasWaterCatchmentSystem { get; set; }
        public int? NumberOfExistingTrees { get; set; }
        public int? NumberOfIndigenousTrees { get; set; }
        public int? NumberOfFruitNutTrees { get; set; }
        public bool? HasSpaceForMoreTrees { get; set; }
        public bool? IsPropertyFenced { get; set; }
        public bool? HasCompostOrMulchResources { get; set; }
        
        // Monitoring Observations
        public int? TreesPlanted { get; set; }
        public int? TreesAlive { get; set; }
        public bool? TreesLookingHealthy { get; set; }
        public bool? UsingChemicalFertilizers { get; set; }
        public bool? UsingPesticides { get; set; }
        public bool? TreesBeingMulched { get; set; }
        public bool? MakingCompost { get; set; }
        public bool? CollectingWater { get; set; }
        
        // Issues and Interventions
        public string ProblemsIdentified { get; set; }
        public string InterventionNeeded { get; set; }
        public decimal? MortalityRate { get; set; }
        public string MortalityReason { get; set; }
        
        // Audit
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        // Navigation properties
        public TreePlantingApplication Application { get; set; }
        public GardenSite GardenSite { get; set; } // Updated to use GardenSite
        public ICollection<MonitoringMetric> Metrics { get; set; }
        public ICollection<MonitoringPhoto> Photos { get; set; }
    }

    public class MonitoringMetric : IAuditable
    {
        [Key]
        public int MonitoringMetricId { get; set; }
        public int MonitoringSessionId { get; set; }
        public MonitoringMetricType MetricType { get; set; }
        public decimal? Value { get; set; }
        public string Unit { get; set; }
        public string Notes { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        // Navigation
        public MonitoringSession MonitoringSession { get; set; }
    }

    public class MonitoringPhoto : IAuditable
    {
        [Key]
        public int MonitoringPhotoId { get; set; }
        public int MonitoringSessionId { get; set; }
        public string Url { get; set; }
        public string Caption { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public int? FileSize { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        // Navigation
        public MonitoringSession MonitoringSession { get; set; }
    }

    // View Models
    public class MonitoringSessionVm
    {
        public MonitoringSession Session { get; set; }
        public IEnumerable<MonitoringMetric> Metrics { get; set; }
        public IEnumerable<MonitoringPhoto> Photos { get; set; }
        public TreePlantingApplication Application { get; set; }
        public GardenSite GardenSite { get; set; } // Updated to use GardenSite
    }

    public class MonitoringListItemVm
    {
        public int MonitoringSessionId { get; set; }
        public int ApplicationId { get; set; }
        public int? GardenSiteId { get; set; } // Updated field name
        public string BeneficiaryName { get; set; }
        public string EvaluatorName { get; set; }
        public DateTime SessionDate { get; set; }
        public int? TreesPlanted { get; set; }
        public int? TreesAlive { get; set; }
        public bool RequiresAttention { get; set; }

        public int TreesDead => (TreesPlanted ?? 0) - (TreesAlive ?? 0);
        public int SurvivalRate => (TreesPlanted.HasValue && TreesPlanted > 0) 
            ? (int)Math.Round(((decimal)(TreesAlive ?? 0) / TreesPlanted.Value) * 100) 
            : 0;
        public string MortalitySummary => TreesDead > 0 ? $"{TreesDead} dead ({SurvivalRate}%)" : "All alive";
        
        public int MortalityRate => (TreesPlanted.HasValue && TreesPlanted > 0) 
            ? (int)Math.Round(((decimal)TreesDead / TreesPlanted.Value) * 100) 
            : 0;
        public bool RequiresIntervention => MortalityRate > 30; // Example threshold
        public string Summary => $"{BeneficiaryName} - {SessionDate:MM/dd/yyyy} ({TreesAlive}/{TreesPlanted} alive)";
    }
}
