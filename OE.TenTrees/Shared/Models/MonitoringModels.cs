using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Oqtane.Models;

namespace OE.TenTrees.Models
{
    public class MonitoringSession : IAuditable
    {
        [Key]
        public int MonitoringSessionId { get; set; }
        public int ApplicationId { get; set; }
        public int ModuleId { get; set; } // For module scoping in Oqtane
        public DateTime SessionDate { get; set; }
        public string ObserverUserId { get; set; }
        public string EvaluatorName { get; set; }
        public string BeneficiaryName { get; set; }
        public string HouseNumber { get; set; }
        public string IdOrBirthdate { get; set; }
        public string WeatherConditions { get; set; }
        public string Notes { get; set; }

        // Mapping Information
        public bool? HasWaterInPlot { get; set; }
        public bool? HasWaterCatchmentSystem { get; set; }

        // Tree Counting
        public int? NumberOfExistingTrees { get; set; }
        public int? NumberOfIndigenousTrees { get; set; }
        public int? NumberOfFruitNutTrees { get; set; }

        // Space and Infrastructure
        public bool? HasSpaceForMoreTrees { get; set; }
        public bool? IsPropertyFenced { get; set; }
        public bool? HasCompostOrMulchResources { get; set; }

        // Tree Health Monitoring (from feature requirements)
        public int? TreesPlanted { get; set; }
        public int? TreesAlive { get; set; }
        public bool? TreesLookingHealthy { get; set; }
        public bool? UsingChemicalFertilizers { get; set; }
        public bool? UsingPesticides { get; set; }
        public bool? TreesBeingMulched { get; set; }
        public bool? MakingCompost { get; set; }
        public bool? CollectingWater { get; set; }

        // Problem Reporting
        public string ProblemsIdentified { get; set; }
        public string InterventionNeeded { get; set; }
        public double? MortalityRate { get; set; }
        public string MortalityReason { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }

    public class MonitoringMetric : IAuditable
    {
        [Key]
        public int MonitoringMetricId { get; set; }
        public int MonitoringSessionId { get; set; }
        public MonitoringMetricType MetricType { get; set; }
        public double? Value { get; set; }
        public string Unit { get; set; }
        public string Notes { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
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
        public int FileSize { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }

    // Enhanced View Models (no navigation properties, just data)
    public class MonitoringSessionVm
    {
        public MonitoringSession Session { get; set; }
        public IEnumerable<MonitoringMetric> Metrics { get; set; }
        public IEnumerable<MonitoringPhoto> Photos { get; set; }
        public TreePlantingApplication Application { get; set; }
        public double CalculatedMortalityRate => CalculateMortalityRate();
        public bool RequiresIntervention => CheckInterventionNeeded();

        private double CalculateMortalityRate()
        {
            if (Session?.TreesPlanted > 0 && Session?.TreesAlive.HasValue == true)
            {
                var dead = Session.TreesPlanted.Value - Session.TreesAlive.Value;
                return (double)dead / Session.TreesPlanted.Value * 100;
            }
            return 0;
        }

        private bool CheckInterventionNeeded()
        {
            return CalculatedMortalityRate > 30 || // High mortality rate
                   Session?.TreesLookingHealthy == false || // Trees not healthy
                   !string.IsNullOrEmpty(Session?.ProblemsIdentified); // Problems identified
        }
    }

    public class MonitoringListItemVm
    {
        public int MonitoringSessionId { get; set; }
        public int ApplicationId { get; set; }
        public string BeneficiaryName { get; set; }
        public string EvaluatorName { get; set; }
        public DateTime SessionDate { get; set; }
        public int? TreesAlive { get; set; }
        public int? TreesPlanted { get; set; }
        public double MortalityRate { get; set; }
        public bool RequiresIntervention { get; set; }
        public string Village { get; set; } // From Application
        public string Summary => $"{BeneficiaryName} - {SessionDate:MM/dd/yyyy} - {TreesAlive}/{TreesPlanted} trees alive";
    }
}
