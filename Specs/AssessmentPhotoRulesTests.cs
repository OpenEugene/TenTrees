using System;
using OpenEug.TenTrees.Models;
using Xunit;

namespace OpenEug.TenTrees.Specs;

public class AssessmentPhotoRulesTests
{
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
    public void CreateStorageFileName_UsesAssessmentAndAssessmentPhotoIds()
    {
        var name = AssessmentPhotoRules.CreateStorageFileName(42, 314, ".JPG");

        Assert.Equal("assessment-42-314.jpg", name);
    }

    [Fact]
    public void CreateStorageFileName_RejectsInvalidInputs()
    {
        Assert.Throws<ArgumentException>(() => AssessmentPhotoRules.CreateStorageFileName(0, 314, "jpg"));
        Assert.Throws<ArgumentException>(() => AssessmentPhotoRules.CreateStorageFileName(42, 0, "jpg"));
        Assert.Throws<ArgumentException>(() => AssessmentPhotoRules.CreateStorageFileName(42, 314, "pdf"));
    }

    [Fact]
    public void PhotoLimits_AreSuitableForOqtaneAssessmentUploads()
    {
        Assert.Equal(5, AssessmentPhotoRules.MaxPhotosPerAssessment);
        Assert.Equal(5, AssessmentPhotoRules.MaxPhotoMegabytes);
        Assert.Equal(5 * 1024 * 1024, AssessmentPhotoRules.MaxPhotoBytes);
    }
}
