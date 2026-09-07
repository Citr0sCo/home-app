using HomeBoxLanding.Api.Features.CustomLinkWidgets.Types;
using Newtonsoft.Json;

namespace HomeBoxLanding.Api.Features.CustomLinkWidgets;

public static class WidgetTypes
{
    public const string Plex = "Plex";
    public const string PiHole = "PiHole";
    public const string QBitTorrent = "QBitTorrent";
    public const string Tautulli = "Tautulli";
    public const string UptimeKuma = "UptimeKuma";
    public const string Radarr = "Radarr";
    public const string Sonarr = "Sonarr";
    public const string Lidarr = "Lidarr";
    public const string Readarr = "Readarr";
}

public class WidgetCacheService
{
    private readonly IWidgetCacheRepository _repository;

    public WidgetCacheService(IWidgetCacheRepository? repository = null)
    {
        _repository = repository ?? new WidgetCacheRepository();
    }

    public T? Get<T>(Guid linkIdentifier, string widgetType)
    {
        var record = _repository.Get(linkIdentifier, widgetType);
        if (record is null || string.IsNullOrWhiteSpace(record.Value))
            return default;

        try
        {
            return JsonConvert.DeserializeObject<T>(record.Value);
        }
        catch (JsonException)
        {
            return default;
        }
    }

    public void Save<T>(Guid linkIdentifier, string widgetType, T value)
    {
        try
        {
            _repository.Save(new WidgetCacheRecord
            {
                LinkIdentifier = linkIdentifier,
                WidgetType = widgetType,
                Value = JsonConvert.SerializeObject(value),
                UpdatedAt = DateTime.UtcNow
            });
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Failed to persist {widgetType} widget cache: {exception.Message}");
        }
    }
}
