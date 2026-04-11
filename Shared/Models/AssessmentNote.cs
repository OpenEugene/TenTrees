using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace OpenEug.TenTrees.Models
{
    public static class AssessmentNoteTypes
    {
        public const string General = "General";
        public const string HomeVisit = "HomeVisit";
    }

    [Table("AssessmentNote")]
    public class AssessmentNote : ModelBase
    {
        [Key]
        public int AssessmentNoteId { get; set; }

        [Required]
        public int AssessmentId { get; set; }

        [Required]
        [MaxLength(20)]
        public string NoteType { get; set; }

        [Required]
        public string Text { get; set; }
    }
}
