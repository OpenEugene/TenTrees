using System.Collections.Generic;

namespace OpenEug.TenTrees.Models
{
    public class AttendanceSummaryViewModel
    {
        public int GrowerId { get; set; }
        public string GrowerName { get; set; }
        public int VillageId { get; set; }
        public string VillageName { get; set; }
        public int ClassesAttended { get; set; }
        public int TotalRequired { get; set; }
        public bool IsEligible { get; set; }
        public string StatusDisplay { get; set; }
    }

    public class TrainingStatusSummary
    {
        public int Eligible { get; set; }
        public int InProgress { get; set; }
        public int NotStarted { get; set; }
        public int Total { get; set; }
    }

    public class MarkAttendanceRequest
    {
        public int TrainingClassId { get; set; }
        public int ModuleId { get; set; }
        public List<AttendanceEntry> Entries { get; set; } = new();
    }

    public class AttendanceEntry
    {
        public int GrowerId { get; set; }
        public bool IsPresent { get; set; }
    }
}
