using System.Text.Json;
using HomeBoxLanding.Api.Core.Json;
using NUnit.Framework;

namespace HomeBoxLanding.Api.Tests.Features.Links;

[TestFixture]
public class UtcDateTimeJsonConverterTests
{
    private JsonSerializerOptions _options = null!;

    [SetUp]
    public void SetUp()
    {
        _options = new JsonSerializerOptions();
        _options.Converters.Add(new UtcDateTimeJsonConverter());
    }

    [Test]
    public void Serialize_WritesUtcMarkerForUnspecifiedDatabaseValues()
    {
        var value = new DateTime(2026, 8, 1, 12, 0, 0, DateTimeKind.Unspecified);

        var json = JsonSerializer.Serialize(value, _options);

        Assert.That(json, Is.EqualTo("\"2026-08-01T12:00:00Z\""));
    }

    [Test]
    public void Deserialize_TreatsUnqualifiedValuesAsUtc()
    {
        var value = JsonSerializer.Deserialize<DateTime>("\"2026-08-01T12:00:00\"", _options);

        Assert.That(value.Kind, Is.EqualTo(DateTimeKind.Utc));
        Assert.That(value, Is.EqualTo(new DateTime(2026, 8, 1, 12, 0, 0, DateTimeKind.Utc)));
    }
}
