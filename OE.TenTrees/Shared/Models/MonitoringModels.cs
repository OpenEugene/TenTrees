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
        public DateTime SessionDate { get; set; }
        public string ObserverUserId { get; set; }
        public string WeatherConditions { get; set; }
        public string Notes { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        public ICollection<MonitoringMetric> Metrics { get; set; }
        public ICollection<MonitoringPhoto> Photos { get; set; }
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

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }

    public class MonitoringSessionVm
    {
        public MonitoringSession Session { get; set; }
        public IEnumerable<MonitoringMetric> Metrics { get; set; }
        public IEnumerable<MonitoringPhoto> Photos { get; set; }
    }
}
