using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Oqtane.Models;

namespace OpenEug.TenTrees.Models
{
    public static class AssessmentPhotoRules
    {
        public const int MaxPhotosPerAssessment = 5;
        public const long MaxPhotoBytes = 5 * 1024 * 1024;
        public const int MaxImageDimension = 1920;

        public static readonly string[] AllowedContentTypes =
        [
            "image/jpeg",
            "image/png",
            "image/webp"
        ];

        public static string DetectContentType(byte[] data)
        {
            if (data == null)
            {
                return null;
            }

            if (data.Length >= 3 && data[0] == 0xFF && data[1] == 0xD8 && data[2] == 0xFF)
            {
                return "image/jpeg";
            }

            if (data.Length >= 8 &&
                data[0] == 0x89 && data[1] == 0x50 && data[2] == 0x4E && data[3] == 0x47 &&
                data[4] == 0x0D && data[5] == 0x0A && data[6] == 0x1A && data[7] == 0x0A)
            {
                return "image/png";
            }

            if (data.Length >= 12 &&
                data[0] == 0x52 && data[1] == 0x49 && data[2] == 0x46 && data[3] == 0x46 &&
                data[8] == 0x57 && data[9] == 0x45 && data[10] == 0x42 && data[11] == 0x50)
            {
                return "image/webp";
            }

            return null;
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
        [MaxLength(255)]
        public string FileName { get; set; }

        [Required]
        [MaxLength(50)]
        public string ContentType { get; set; }

        public long FileSize { get; set; }

        [Required]
        [JsonIgnore]
        public byte[] PhotoData { get; set; }
    }
}
