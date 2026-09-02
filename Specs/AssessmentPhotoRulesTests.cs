using OpenEug.TenTrees.Models;
using Xunit;

namespace OpenEug.TenTrees.Specs;

public class AssessmentPhotoRulesTests
{
    [Fact]
    public void DetectContentType_ReturnsJpeg_ForJpegSignature()
    {
        byte[] data = [0xFF, 0xD8, 0xFF, 0xE0];

        Assert.Equal("image/jpeg", AssessmentPhotoRules.DetectContentType(data));
    }

    [Fact]
    public void DetectContentType_ReturnsPng_ForPngSignature()
    {
        byte[] data = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A];

        Assert.Equal("image/png", AssessmentPhotoRules.DetectContentType(data));
    }

    [Fact]
    public void DetectContentType_ReturnsWebp_ForWebpSignature()
    {
        byte[] data = [0x52, 0x49, 0x46, 0x46, 0x10, 0x00, 0x00, 0x00, 0x57, 0x45, 0x42, 0x50];

        Assert.Equal("image/webp", AssessmentPhotoRules.DetectContentType(data));
    }

    [Theory]
    [InlineData(null)]
    [InlineData(new byte[] { })]
    [InlineData(new byte[] { 0x25, 0x50, 0x44, 0x46 })]
    [InlineData(new byte[] { 0x3C, 0x73, 0x63, 0x72, 0x69, 0x70, 0x74, 0x3E })]
    public void DetectContentType_RejectsNonImageContent(byte[]? data)
    {
        Assert.Null(AssessmentPhotoRules.DetectContentType(data!));
    }

    [Fact]
    public void PhotoLimits_AreSuitableForMobileAssessmentUploads()
    {
        Assert.Equal(5, AssessmentPhotoRules.MaxPhotosPerAssessment);
        Assert.Equal(5 * 1024 * 1024, AssessmentPhotoRules.MaxPhotoBytes);
        Assert.Equal(1920, AssessmentPhotoRules.MaxImageDimension);
    }
}
