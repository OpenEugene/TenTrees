using System;
using System.ComponentModel.DataAnnotations;

namespace OpenEug.TenTrees.Models
{
    public class StatusToggleRequest
    {
        public int GrowerId { get; set; }
    }

    public class ProgramExitRequest
    {
        public int GrowerId { get; set; }
        public DateTime ExitDate { get; set; }
        public string ExitReason { get; set; }
        public string Notes { get; set; }
    }

    public class GardenLocationCaptureRequest
    {
        public int GrowerId { get; set; }

        [Range(typeof(decimal), "-90", "90")]
        public decimal Latitude { get; set; }

        [Range(typeof(decimal), "-180", "180")]
        public decimal Longitude { get; set; }

        [Range(typeof(decimal), "0", "100000")]
        public decimal? AccuracyMeters { get; set; }
    }

    public class GrowerStatusSummary
    {
        public int Active { get; set; }
        public int Inactive { get; set; }
        public int Exited { get; set; }
        public int Total { get; set; }
    }

    public class EnrollmentStatusSummary
    {
        public int Pending { get; set; }
        public int Approved { get; set; }
        public int Rejected { get; set; }
        public int Total { get; set; }
    }
}
