using HomeBoxLanding.Api.Features.Links;
using HomeBoxLanding.Api.Features.Links.Types;
using NUnit.Framework;

namespace HomeBoxLanding.Api.Tests.Features.Links;

[TestFixture]
public class LinkMapperTests
{
    [Test]
    public void Map_CarriesLastClickedAtToApiLink()
    {
        var lastClickedAt = DateTime.UtcNow;
        var record = new LinkRecord
        {
            Identifier = Guid.NewGuid(),
            Name = "Dashboard",
            Url = "https://example.com",
            Host = "example.com",
            IconUrl = "icon.png",
            LastClickedAt = lastClickedAt
        };

        var result = LinkMapper.Map(record);

        Assert.That(result.LastClickedAt, Is.EqualTo(lastClickedAt));
    }
}
