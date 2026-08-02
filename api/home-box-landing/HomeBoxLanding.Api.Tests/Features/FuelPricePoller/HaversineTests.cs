using HomeBoxLanding.Api.Features.FuelPricePoller;
using NUnit.Framework;

namespace HomeBoxLanding.Api.Tests.Features.FuelPricePoller;

[TestFixture]
public class HaversineTests
{
    [Test]
    public void Calculate_ReturnsZeroForTheSameCoordinates()
    {
        var distance = Haversine.Calculate(51.5074, -0.1278, 51.5074, -0.1278);

        Assert.That(distance, Is.EqualTo(0));
    }

    [Test]
    public void Calculate_ReturnsDistanceInRoundedMeters()
    {
        var distance = Haversine.Calculate(0, 0, 0, 1);

        Assert.That(distance, Is.EqualTo(111195));
    }
}
