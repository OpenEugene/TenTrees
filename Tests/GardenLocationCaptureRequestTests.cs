using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using OpenEug.TenTrees.Models;
using Xunit;

namespace OpenEug.TenTrees.Tests;

public class GardenLocationCaptureRequestTests
{
    [Fact]
    public void ValidSouthAfricanLocation_PassesValidation()
    {
        var request = new GardenLocationCaptureRequest
        {
            GrowerId = 1,
            Latitude = -24.527100m,
            Longitude = 31.136700m,
            AccuracyMeters = 8.5m
        };

        Assert.Empty(Validate(request));
    }

    [Theory]
    [InlineData("-90", "180")]
    [InlineData("90", "-180")]
    public void CoordinateBoundaries_PassValidation(string latitude, string longitude)
    {
        var request = new GardenLocationCaptureRequest
        {
            GrowerId = 1,
            Latitude = decimal.Parse(latitude, CultureInfo.InvariantCulture),
            Longitude = decimal.Parse(longitude, CultureInfo.InvariantCulture),
            AccuracyMeters = 0m
        };

        Assert.Empty(Validate(request));
    }

    [Theory]
    [InlineData("-90.000001", "0", "10")]
    [InlineData("90.000001", "0", "10")]
    [InlineData("0", "-180.000001", "10")]
    [InlineData("0", "180.000001", "10")]
    [InlineData("0", "0", "-0.01")]
    [InlineData("0", "0", "100000.01")]
    public void OutOfRangeLocation_FailsValidation(string latitude, string longitude, string accuracy)
    {
        var request = new GardenLocationCaptureRequest
        {
            GrowerId = 1,
            Latitude = decimal.Parse(latitude, CultureInfo.InvariantCulture),
            Longitude = decimal.Parse(longitude, CultureInfo.InvariantCulture),
            AccuracyMeters = decimal.Parse(accuracy, CultureInfo.InvariantCulture)
        };

        Assert.NotEmpty(Validate(request));
    }

    private static List<ValidationResult> Validate(GardenLocationCaptureRequest request)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(request, new ValidationContext(request), results, true);
        return results;
    }
}
