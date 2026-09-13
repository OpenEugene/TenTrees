using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using Oqtane.Models;

namespace OpenEug.TenTrees.Models
{
    public static class AssessmentPhotoRules
    {
        public const int MaxPhotosPerAssessment = 5;

        /// <summary>
        /// Largest file the upload control accepts. Phone cameras produce 3–12 MB originals and
        /// field users have no way to shrink them, so this is set well above that; the server
        /// resizes every photo after upload (see <see cref="MaxLongEdgePixels"/>).
        /// </summary>
        public const int MaxUploadMegabytes = 25;
        public const long MaxUploadBytes = MaxUploadMegabytes * 1024L * 1024L;

        /// <summary>Longest side a stored photo is reduced to. Plenty for judging a garden problem on a screen.</summary>
        public const int MaxLongEdgePixels = 1600;

        /// <summary>Encoder quality for lossy formats (JPEG, WebP) after resizing.</summary>
        public const int ResizedQuality = 80;

        /// <summary>Sanity cap on what is kept after resizing. A 1600 px photo is normally well under 1 MB.</summary>
        public const int MaxPhotoMegabytes = 5;
        public const long MaxPhotoBytes = MaxPhotoMegabytes * 1024L * 1024L;
        /// <summary>Site-level root folder. Each grower gets a child folder named by GrowerId.</summary>
        public const string FolderPath = "AssessmentPhotos/";

        public static readonly string[] AllowedExtensions = ["jpg", "jpeg", "png", "webp"];

        /// <summary>Oqtane folder path for one grower's photos, e.g. <c>AssessmentPhotos/12/</c>.</summary>
        public static string GrowerFolderPath(int growerId)
        {
            if (growerId <= 0)
            {
                throw new ArgumentException("A positive grower ID is required.", nameof(growerId));
            }

            return $"{FolderPath}{growerId}/";
        }

        public static bool IsAllowedExtension(string extension)
        {
            if (string.IsNullOrWhiteSpace(extension))
            {
                return false;
            }

            return AllowedExtensions.Contains(extension.TrimStart('.'), StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Human-readable storage name for a photo in a grower's folder, e.g. <c>2026-Sep_A.jpg</c>.
        /// The date is the assessment date; the letter distinguishes photos taken in the same month.
        /// The app never resolves photos by name (it uses the Oqtane FileId), so the name only has
        /// to be unique within the grower's folder and easy for a person to scan.
        /// </summary>
        public static string CreateStorageFileName(DateTime date, int index, string extension)
        {
            var normalizedExtension = extension?.Trim().TrimStart('.').ToLowerInvariant();
            if (index <= 0 || !IsAllowedExtension(normalizedExtension))
            {
                throw new ArgumentException("A positive index and a supported image extension are required.");
            }

            return $"{date.ToString("yyyy-MMM", CultureInfo.InvariantCulture)}_{IndexToLetters(index)}.{normalizedExtension}";
        }

        /// <summary>1 → A, 26 → Z, 27 → AA, 52 → AZ, 53 → BA.</summary>
        public static string IndexToLetters(int index)
        {
            if (index <= 0)
            {
                throw new ArgumentException("Index must be positive.", nameof(index));
            }

            var letters = string.Empty;
            while (index > 0)
            {
                index--;
                letters = (char)('A' + index % 26) + letters;
                index /= 26;
            }

            return letters;
        }

        /// <summary>
        /// First storage name for the given date and extension that <paramref name="nameInUse"/> does not
        /// report as taken. Letters are tried in order, so a removed photo's letter can be reused.
        /// </summary>
        public static string NextStorageFileName(DateTime date, string extension, Func<string, bool> nameInUse)
        {
            if (nameInUse == null)
            {
                throw new ArgumentNullException(nameof(nameInUse));
            }

            for (var index = 1; index <= 26 * 27; index++)
            {
                var candidate = CreateStorageFileName(date, index, extension);
                if (!nameInUse(candidate))
                {
                    return candidate;
                }
            }

            throw new InvalidOperationException("No free photo name available for this month.");
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
        public int FileId { get; set; }

        [Required]
        [MaxLength(2048)]
        public string Url { get; set; }
    }
}
