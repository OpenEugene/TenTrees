using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Oqtane.Models;

namespace OE.TenTrees.Models
{
    // Core Application entity
    public class TreePlantingApplication : IAuditable
    {
        [Key]
        public int ApplicationId { get; set; }
        public int ModuleId { get; set; } // for module scoping in Oqtane
        public string EvaluatorName { get; set; }
        public string ApplicantUserId { get; set; }
        public string BeneficiaryName { get; set; }
        public string HouseholdIdentifier { get; set; } // house number or ID/Birthdate
        public string Address { get; set; }
        public string Village { get; set; }
        public int? HouseholdSize { get; set; }
        public DateTime? SubmissionDate { get; set; }

        // Criteria (preferred)
        public bool HasExistingGardenInterest { get; set; }
        public bool GardenCurrentlyTended { get; set; }
        public bool ChildHeadedHousehold { get; set; }
        public bool WomanHeadedHousehold { get; set; }
        public bool EmptyOrNearlyEmptyYard { get; set; }

        // Commitments
        public bool CommitNoChemicals { get; set; }
        public bool CommitAttendTraining { get; set; }
        public bool CommitNotCutTrees { get; set; }
        public bool CommitStandAgainstAbuse { get; set; }

        public ApplicationStatus Status { get; set; }
        public string RejectionReason { get; set; }
        public string Notes { get; set; }

        // Audit
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        public ICollection<ApplicationDocument> Documents { get; set; }
        public ICollection<ApplicationStatusHistory> History { get; set; }
    }

    public class ApplicationDocument : IAuditable
    {
        [Key]
        public int DocumentId { get; set; }
        public int ApplicationId { get; set; }
        public DocumentType DocumentType { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long FileSize { get; set; }
        public string Url { get; set; }
        public string Caption { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }

    public class ApplicationStatusHistory : IAuditable
    {
        [Key]
        public int HistoryId { get; set; }
        public int ApplicationId { get; set; }
        public ApplicationStatus Status { get; set; }
        public string ChangedByUserId { get; set; }
        public string Comment { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }

    // View Models
    public class ApplicationListItemVm
    {
        public int ApplicationId { get; set; }
        public string BeneficiaryName { get; set; }
        public string EvaluatorName { get; set; }
        public ApplicationStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public string Summary => $"{BeneficiaryName} - {Status}";
    }

    public class ApplicationDetailVm
    {
        public TreePlantingApplication Application { get; set; }
        public IEnumerable<ApplicationDocument> Documents { get; set; }
        public IEnumerable<ApplicationStatusHistory> History { get; set; }
        public IEnumerable<SiteAssessment> Assessments { get; set; }
        public IEnumerable<MonitoringSession> Monitoring { get; set; }
    }
}
