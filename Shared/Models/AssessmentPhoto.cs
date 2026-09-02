using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace OpenEug.TenTrees.Models
{
    public static class AssessmentPhotoRules
    {
        public const int MaxPhotosPerAssessment = 5;
        public const int MaxPhotoMegabytes = 5;
        public const long MaxPhotoBytes = MaxPhotoMegabytes * 1024L * 1024L;
        public const string FolderPath = "AssessmentPhotos/";

        public static readonly string[] AllowedExtensions = ["jpg", "jpeg", "png", "webp"];

        public static bool IsAllowedExtension(string extension)
        {
            if (string.IsNullOrWhiteSpace(extension))
            {
                return false;
            }

            return AllowedExtensions.Contains(extension.TrimStart('.'), StringComparer.OrdinalIgnoreCase);
        }

        public static string CreateStorageFileName(int assessmentId, int assessmentPhotoId, string extension)
        {
            var normalizedExtension = extension?.Trim().TrimStart('.').ToLowerInvariant();
            if (assessmentId <= 0 || assessmentPhotoId <= 0 || !IsAllowedExtension(normalizedExtension))
            {
                throw new ArgumentException("Positive assessment and assessment-photo IDs plus a supported image extension are required.");
            }

            return $"assessment-{assessmentId}-{assessmentPhotoId}.{normalizedExtension}";
        }
    }

    [Table("AssessmentPhoto")]
    public class AssessmentPhoto : ModelBase
    {
        [Key]
        public int AssessmentPhotoId { get; set; }

        [Required]
        public int AssessmentId { get; set; }

        [Required]
        public int PhotoId { get; set; }

        [Required]
        [MaxLength(2048)]
        public string Url { get; set; }
    }
}
