using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Oqtane.Models;

namespace OE.TenTrees.Models
{
    // Core GardenSite entity created from approved applications
    public class GardenSite : IAuditable
    {
        [Key]
        public int GardenSiteId { get; set; }
        public int ApplicationId { get; set; } // Reference to the approved application
        public int ModuleId { get; set; } // for module scoping in Oqtane
        
        // Basic Information (from form)
        public string EvaluatorName { get; set; }
        public string BeneficiaryName { get; set; }
        public string HouseNumber { get; set; }
        public string IdOrBirthdate { get; set; }
        
        // Mapping Information / Swa ineno
        public bool? HasWaterInPlot { get; set; } // Do you have water in the plot? / Kuna mati ka muli ye lowo?
        public bool? HasWaterCatchmentSystem { get; set; } // Is there any water catchment system? / Jojo tanki? Kuna xo khoma mati ku fana na thanki?
        
        // Tree Information
        public int? NumberOfExistingTrees { get; set; } // Number of existing trees/productive plants in the yard / Nhlayu ya misinva kumbe swi byariwa swa mphayo xihanyeni xa yoch lowi ngaki?
        public int? NumberOfIndigenousTrees { get; set; } // Number of indigenous trees (namety) / Nhlayu ya misinva leyi yi nga foloxekangiki?
        public int? NumberOfFruitNutTrees { get; set; } // Number of fruit and nut trees (namety) / Nhlayu ya misinva ya mihandzu na timanga?
        
        // Site Assessment Questions
        public bool? HasSpaceForMoreTrees { get; set; } // Is there space for more trees? / Kuna ndawu ya ku engeta misinva?
        public bool? IsPropertyFenced { get; set; } // Is the property fenced? / Ndawu yi pfafariwie hi darahi?
        public bool? HasCompostOrMulchResources { get; set; } // Are there any resources like compost or mulch that can be used for tree planting? / Kuna swimani leswi nga pfumelaka hi ku byala misinva, ku fana na manyora?
        
        // Garden Status
        public GardenStatus Status { get; set; } = GardenStatus.Active;
        public DateTime? PlantingDate { get; set; } // When trees were planted
        public DateTime? LastMonitoringDate { get; set; } // Last monitoring visit date
        
        // Location and Contact Info
        public string Village { get; set; }
        public string Address { get; set; }
        public string ContactInformation { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        
        // Notes and observations
        public string Notes { get; set; }
        public string SpecialInstructions { get; set; }
        
        // Audit
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        // No navigation properties - use LINQ joins instead
    }

    // Track individual tree plantings in a garden site
    public class TreePlanting : IAuditable
    {
        [Key]
        public int TreePlantingId { get; set; }
        public int GardenSiteId { get; set; }
        public string TreeSpecies { get; set; }
        public string TreeVariety { get; set; }
        public int Quantity { get; set; }
        public DateTime PlantingDate { get; set; }
        public string PlantingLocation { get; set; } // Area within the garden
        public TreePlantingStatus Status { get; set; } = TreePlantingStatus.Planted;
        public string Notes { get; set; }
        
        // Audit
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        // No navigation properties - use LINQ joins instead
    }

    // Garden-specific photos
    public class GardenPhoto : IAuditable
    {
        [Key]
        public int GardenPhotoId { get; set; }
        public int GardenSiteId { get; set; }
        public string Url { get; set; }
        public string Caption { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public int? FileSize { get; set; }
        public GardenPhotoType PhotoType { get; set; } = GardenPhotoType.General;
        public DateTime PhotoDate { get; set; } = DateTime.UtcNow;
        
        // Audit
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        // No navigation properties - use LINQ joins instead
    }

    // View Models
    public class GardenListItemVm
    {
        public int GardenSiteId { get; set; }
        public string BeneficiaryName { get; set; }
        public string EvaluatorName { get; set; }
        public string Village { get; set; }
        public GardenStatus Status { get; set; }
        public DateTime? PlantingDate { get; set; }
        public DateTime? LastMonitoringDate { get; set; }
        public int MonitoringSessionCount { get; set; }
        public int TreeSpeciesCount { get; set; }
        public int TotalTreesPlanted { get; set; }
        public bool RequiresAttention { get; set; }
        public string Summary => $"{BeneficiaryName} - {Village} ({Status})";
    }

    public class GardenDetailVm
    {
        public GardenSite GardenSite { get; set; }
        public TreePlantingApplication Application { get; set; }
        public IEnumerable<TreePlanting> TreePlantings { get; set; }
        public IEnumerable<MonitoringSession> MonitoringSessions { get; set; }
        public IEnumerable<GardenPhoto> Photos { get; set; }
        public GardenStatistics Statistics { get; set; }
    }

    public class GardenStatistics
    {
        public int TotalTreesPlanted { get; set; }
        public int TreeSpeciesCount { get; set; }
        public int MonitoringSessionCount { get; set; }
        public DateTime? LastMonitoringDate { get; set; }
        public double? AverageSurvivalRate { get; set; }
        public bool RequiresIntervention { get; set; }
        public string InterventionReason { get; set; }
    }

    // Garden creation from approved application
    public class CreateGardenFromApplicationRequest
    {
        public int ApplicationId { get; set; }
        public string EvaluatorName { get; set; }
        public string HouseNumber { get; set; }
        public string IdOrBirthdate { get; set; }
        public bool? HasWaterInPlot { get; set; }
        public bool? HasWaterCatchmentSystem { get; set; }
        public int? NumberOfExistingTrees { get; set; }
        public int? NumberOfIndigenousTrees { get; set; }
        public int? NumberOfFruitNutTrees { get; set; }
        public bool? HasSpaceForMoreTrees { get; set; }
        public bool? IsPropertyFenced { get; set; }
        public bool? HasCompostOrMulchResources { get; set; }
        public string Notes { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
}