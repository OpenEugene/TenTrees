using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace OpenEug.TenTrees.Models
{
    public static class AssessmentProblemTypes
    {
        public const string YellowLeaves = "YellowLeaves";
        public const string BrokenBranches = "BrokenBranches";
        public const string LosingLeaves = "LosingLeaves";
        public const string LookDry = "LookDry";
        public const string Pests = "Pests";

        public static readonly string[] All =
        [
            YellowLeaves,
            BrokenBranches,
            LosingLeaves,
            LookDry,
            Pests
        ];

        public static string DisplayName(string type) => type switch
        {
            YellowLeaves => "The trees have yellow leaves",
            BrokenBranches => "The trees have broken branches",
            LosingLeaves => "The trees are losing their leaves",
            LookDry => "The trees look dry",
            Pests => "Pests eating the plant",
            _ => type
        };
    }

    [Table("AssessmentProblem")]
    public class AssessmentProblem : ModelBase
    {
        [Key]
        public int AssessmentProblemId { get; set; }

        [Required]
        public int AssessmentId { get; set; }

        [Required]
        public int TreeId { get; set; }

        [Required]
        [MaxLength(50)]
        public string ProblemType { get; set; }

        public bool NeedsHelp { get; set; }
    }
}
