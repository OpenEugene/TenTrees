using System;

namespace OE.TenTrees.Models
{
    // Core enumerations inferred from feature specifications
    public enum ApplicationStatus
    {
        Draft = 0,
        PendingReview = 1, // renamed from Submitted to align with feature wording "Pending Review"
        UnderReview = 2,
        Approved = 3,
        Rejected = 4,
        Withdrawn = 5
    }

    public enum ReviewDecision
    {
        Pending = 0,
        Approve = 1,
        Reject = 2,
        NeedMoreInformation = 3
    }

    public enum AssessmentOutcome
    {
        Unknown = 0,
        Suitable = 1,
        PartiallySuitable = 2,
        NotSuitable = 3
    }

    public enum MonitoringMetricType
    {
        SurvivalRate = 0,
        GrowthHeight = 1,
        SoilMoisture = 2,
        PestIncidence = 3,
        CanopyCover = 4,
        TreesPlanted = 5,
        TreesAlive = 6,
        TreeHealth = 7,
        WaterAvailability = 8,
        CompostUsage = 9,
        MulchingStatus = 10,
        ChemicalUsage = 11
    }

    public enum DocumentType
    {
        Other = 0,
        ApplicationForm = 1,
        SitePhoto = 2,
        AssessmentForm = 3,
        MonitoringPhoto = 4,
        SupportingDocument = 5
    }
}
