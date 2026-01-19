using System;
using OpenEug.TenTrees.Models;

namespace OpenEug.TenTrees.Module.Enrollment.Services
{
    public interface IEnrollmentStateService
    {
        EnrollmentDraft CurrentDraft { get; set; }
        void InitializeDraft(int moduleId);
        void ClearDraft();
    }

    public class EnrollmentStateService : IEnrollmentStateService
    {
        public EnrollmentDraft CurrentDraft { get; set; }

        public void InitializeDraft(int moduleId)
        {
            CurrentDraft = new EnrollmentDraft
            {
                ModuleId = moduleId
            };
        }

        public void ClearDraft()
        {
            CurrentDraft = null;
        }
    }

    public class EnrollmentDraft
    {
        public int ModuleId { get; set; }
        public int? EnrollmentId { get; set; }
        
        // Basic Information
        public string EvaluatorName { get; set; }
        public string BeneficiaryName { get; set; }
        public int VillageId { get; set; }
        public string HouseNumber { get; set; }
        public string IdNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool OwnsHome { get; set; }
        public int HouseholdSize { get; set; } = 1;
        
        // Preferred Criteria
        public bool EnrolledInPE { get; set; }
        public bool PEGraduate { get; set; }
        public bool GardenPlantedAndTended { get; set; }
        public bool ChildHeadedHousehold { get; set; }
        public bool WomanHeadedHousehold { get; set; }
        public bool EmptyYard { get; set; }
        
        // Commitments
        public bool CommitNoChemicals { get; set; }
        public bool CommitAttendClasses { get; set; }
        public bool CommitNoCuttingTrees { get; set; }
        public bool CommitStandForWomenChildren { get; set; }
        public bool CommitCareWhileAway { get; set; }
        public bool CommitAllowYardAccess { get; set; }
        
        // Signature
        public string SignatureData { get; set; }
    }
}
