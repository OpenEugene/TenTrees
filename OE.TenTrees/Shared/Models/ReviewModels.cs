using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Oqtane.Models;

namespace OE.TenTrees.Models
{
    public class ApplicationReview : IAuditable
    {
        [Key]
        public int ReviewId { get; set; }
        public int ApplicationId { get; set; }
        public string ReviewerUserId { get; set; }
        public DateTime ReviewDate { get; set; }
        public ReviewDecision Decision { get; set; }
        public string Summary { get; set; }
        public string Comments { get; set; }
        public bool RequiresFollowUp { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        public ICollection<ReviewChecklistItem> Checklist { get; set; }
    }

    public class ReviewChecklistItem : IAuditable
    {
        [Key]
        public int ChecklistItemId { get; set; }
        public int ReviewId { get; set; }
        public string Item { get; set; }
        public bool IsComplete { get; set; }
        public string Notes { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }

    // View Models
    public class ReviewSummaryVm
    {
        public int ReviewId { get; set; }
        public int ApplicationId { get; set; }
        public DateTime ReviewDate { get; set; }
        public ReviewDecision Decision { get; set; }
        public string Summary { get; set; }
        public bool RequiresFollowUp { get; set; }
    }

    public class ReviewDetailVm
    {
        public ApplicationReview Review { get; set; }
        public IEnumerable<ReviewChecklistItem> Checklist { get; set; }
        public IEnumerable<ApplicationDocument> RelatedDocuments { get; set; }
    }
}
