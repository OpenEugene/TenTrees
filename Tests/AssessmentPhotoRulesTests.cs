using System;
using System.Collections.Generic;
using OpenEug.TenTrees.Models;
using Xunit;

namespace OpenEug.TenTrees.Tests;

public class AssessmentPhotoRulesTests
{
    private static readonly DateTime September = new(2026, 9, 13);

    [Theory]
    [InlineData("jpg")]
    [InlineData("jpeg")]
    [InlineData("png")]
    [InlineData("webp")]
    public void IsAllowedExtension_AcceptsConfiguredImageTypes(string extension)
    {
        Assert.True(AssessmentPhotoRules.IsAllowedExtension(extension));
    }

    [Theory]
    [InlineData("pdf")]
    [InlineData("exe")]
    [InlineData("")]
    public void IsAllowedExtension_RejectsUnsupportedTypes(string extension)
    {
        Assert.False(AssessmentPhotoRules.IsAllowedExtension(extension));
    }

    [Fact]
    public void GrowerFolderPath_NestsGrowerIdUnderRoot()
    {
        Assert.Equal("AssessmentPhotos/12/", AssessmentPhotoRules.GrowerFolderPath(12));
        Assert.Throws<ArgumentException>(() => AssessmentPhotoRules.GrowerFolderPath(0));
    }

    [Fact]
    public void CreateStorageFileName_IsYearMonthAndLetter()
    {
        Assert.Equal("2026-Sep_A.jpg", AssessmentPhotoRules.CreateStorageFileName(September, 1, ".JPG"));
        Assert.Equal("2026-Sep_C.webp", AssessmentPhotoRules.CreateStorageFileName(September, 3, "webp"));
        Assert.Equal("2027-Jan_A.png", AssessmentPhotoRules.CreateStorageFileName(new DateTime(2027, 1, 2), 1, "png"));
    }

    [Fact]
    public void CreateStorageFileName_RejectsInvalidInputs()
    {
        Assert.Throws<ArgumentException>(() => AssessmentPhotoRules.CreateStorageFileName(September, 0, "jpg"));
        Assert.Throws<ArgumentException>(() => AssessmentPhotoRules.CreateStorageFileName(September, 1, "pdf"));
    }

    [Theory]
    [InlineData(1, "A")]
    [InlineData(26, "Z")]
    [InlineData(27, "AA")]
    [InlineData(52, "AZ")]
    [InlineData(53, "BA")]
    public void IndexToLetters_RollsOverLikeSpreadsheetColumns(int index, string expected)
    {
        Assert.Equal(expected, AssessmentPhotoRules.IndexToLetters(index));
    }

    [Fact]
    public void NextStorageFileName_SkipsNamesAlreadyInUse()
    {
        var inUse = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "2026-Sep_A.jpg", "2026-Sep_B.jpg" };

        var name = AssessmentPhotoRules.NextStorageFileName(September, "jpg", inUse.Contains);

        Assert.Equal("2026-Sep_C.jpg", name);
    }

    [Fact]
    public void NextStorageFileName_ReusesAFreedLetter()
    {
        // B was removed; the next upload takes B rather than skipping to D.
        var inUse = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "2026-Sep_A.jpg", "2026-Sep_C.jpg" };

        Assert.Equal("2026-Sep_B.jpg", AssessmentPhotoRules.NextStorageFileName(September, "jpg", inUse.Contains));
    }

    [Fact]
    public void NextStorageFileName_ExtensionDoesNotShareLetters()
    {
        // A png and a jpg with the same letter are distinct files; only exact names collide.
        var inUse = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "2026-Sep_A.jpg" };

        Assert.Equal("2026-Sep_A.png", AssessmentPhotoRules.NextStorageFileName(September, "png", inUse.Contains));
    }

    [Fact]
    public void PhotoLimits_AreSuitableForOqtaneAssessmentUploads()
    {
        Assert.Equal(5, AssessmentPhotoRules.MaxPhotosPerAssessment);
        Assert.Equal(5, AssessmentPhotoRules.MaxPhotoMegabytes);
        Assert.Equal(5 * 1024 * 1024, AssessmentPhotoRules.MaxPhotoBytes);
    }
}
