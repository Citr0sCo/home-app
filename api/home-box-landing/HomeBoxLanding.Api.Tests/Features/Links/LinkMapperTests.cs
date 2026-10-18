using HomeBoxLanding.Api.Features.Links;
using HomeBoxLanding.Api.Features.Links.Types;
using NUnit.Framework;

namespace HomeBoxLanding.Api.Tests.Features.Links;

[TestFixture]
public class LinkMapperTests
{
    [Test]
    public void Map_MapsLinkConfigurationWithoutAccessTimestamp()
    {
        var identifier = Guid.NewGuid();
        var record = new LinkRecord
        {
            Identifier = identifier,
            Name = "Dashboard",
            Url = "https://example.com",
            Host = "example.com",
            IconUrl = "icon.png"
        };

        var result = LinkMapper.Map(record);

        Assert.That(result.Identifier, Is.EqualTo(identifier));
        Assert.That(result.Url, Is.EqualTo("https://example.com"));
    }
}
