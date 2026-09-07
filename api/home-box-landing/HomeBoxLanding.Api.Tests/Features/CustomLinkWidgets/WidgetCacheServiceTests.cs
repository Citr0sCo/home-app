using HomeBoxLanding.Api.Features.CustomLinkWidgets;
using HomeBoxLanding.Api.Features.CustomLinkWidgets.Types;
using NUnit.Framework;

namespace HomeBoxLanding.Api.Tests.Features.CustomLinkWidgets;

[TestFixture]
public class WidgetCacheServiceTests
{
    [Test]
    public void Get_ReturnsTheLatestSavedValueForWidget()
    {
        var repository = new InMemoryWidgetCacheRepository();
        var service = new WidgetCacheService(repository);
        var linkIdentifier = Guid.NewGuid();

        service.Save(linkIdentifier, WidgetTypes.QBitTorrent, new CachedStats { Total = 4 });
        service.Save(linkIdentifier, WidgetTypes.QBitTorrent, new CachedStats { Total = 9 });

        var result = service.Get<CachedStats>(linkIdentifier, WidgetTypes.QBitTorrent);

        Assert.That(result?.Total, Is.EqualTo(9));
        Assert.That(repository.Records, Has.Count.EqualTo(1));
    }

    [Test]
    public void Get_ReturnsNullWhenTheStoredValueIsInvalid()
    {
        var linkIdentifier = Guid.NewGuid();
        var repository = new InMemoryWidgetCacheRepository();
        repository.Records.Add(new WidgetCacheRecord
        {
            LinkIdentifier = linkIdentifier,
            WidgetType = WidgetTypes.Plex,
            Value = "not-json"
        });
        var service = new WidgetCacheService(repository);

        var result = service.Get<CachedStats>(linkIdentifier, WidgetTypes.Plex);

        Assert.That(result, Is.Null);
    }

    private sealed class CachedStats
    {
        public int Total { get; set; }
    }

    private sealed class InMemoryWidgetCacheRepository : IWidgetCacheRepository
    {
        public List<WidgetCacheRecord> Records { get; } = new();

        public WidgetCacheRecord? Get(Guid linkIdentifier, string widgetType)
        {
            return Records.FirstOrDefault(record =>
                record.LinkIdentifier == linkIdentifier && record.WidgetType == widgetType);
        }

        public void Save(WidgetCacheRecord record)
        {
            var existing = Get(record.LinkIdentifier, record.WidgetType);
            if (existing is null)
            {
                Records.Add(record);
                return;
            }

            existing.Value = record.Value;
            existing.UpdatedAt = record.UpdatedAt;
        }
    }
}
