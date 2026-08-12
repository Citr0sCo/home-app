using HomeBoxLanding.Api.Data;
using HomeBoxLanding.Api.Features.Settings.Types;
using Microsoft.EntityFrameworkCore;

namespace HomeBoxLanding.Api.Features.Settings;

public class SettingsService
{
    private static readonly IReadOnlyList<SettingDefinition> Definitions = new List<SettingDefinition>
    {
        new() { Key = "weatherApiKey", EnvironmentVariable = "ASPNETCORE_WEATHER_API_KEY", Label = "Weather API key", Description = "OpenWeatherMap API key used for the weather widget.", IsSecret = true },
        new() { Key = "mapsApiKey", EnvironmentVariable = "ASPNETCORE_MAPS_API_KEY", Label = "Maps API key", Description = "Mapbox access token used for map widgets.", IsSecret = true },
        new() { Key = "tautulliApiKey", EnvironmentVariable = "ASPNETCORE_TAUTULLI_API_KEY", Label = "Tautulli API key", Description = "API key shared by the Tautulli and Plex widgets.", IsSecret = true },
        new() { Key = "radarrApiKey", EnvironmentVariable = "ASPNETCORE_RADARR_API_KEY", Label = "Radarr API key", Description = "API key used by the Radarr widget.", IsSecret = true },
        new() { Key = "sonarrApiKey", EnvironmentVariable = "ASPNETCORE_SONARR_API_KEY", Label = "Sonarr API key", Description = "API key used by the Sonarr widget.", IsSecret = true },
        new() { Key = "lidarrApiKey", EnvironmentVariable = "ASPNETCORE_LIDARR_API_KEY", Label = "Lidarr API key", Description = "API key used by the Lidarr widget.", IsSecret = true },
        new() { Key = "readarrApiKey", EnvironmentVariable = "ASPNETCORE_READARR_API_KEY", Label = "Readarr API key", Description = "API key used by the Readarr widget.", IsSecret = true },
        new() { Key = "piHoleApiKey", EnvironmentVariable = "ASPNETCORE_PIHOLE_API_KEY", Label = "Pi-hole API key", Description = "Password or API key used by the Pi-hole widget.", IsSecret = true },
        new() { Key = "uptimeKumaApiKey", EnvironmentVariable = "ASPNETCORE_UPTIME_KUMA_API_KEY", Label = "Uptime Kuma API key", Description = "API key used to authenticate with Uptime Kuma metrics.", IsSecret = true },
        new() { Key = "fuelFinderClientId", EnvironmentVariable = "ASPNETCORE_FUEL_FINDER_CLIENT_ID", Label = "Fuel Finder client ID", Description = "Client ID used to retrieve UK fuel prices.", IsSecret = true },
        new() { Key = "fuelFinderClientSecret", EnvironmentVariable = "ASPNETCORE_FUEL_FINDER_CLIENT_SECRET", Label = "Fuel Finder client secret", Description = "Client secret used to retrieve UK fuel prices.", IsSecret = true },
        new() { Key = "updateScriptRoot", EnvironmentVariable = "ASPNETCORE_UPDATE_SCRIPT_ROOT", Label = "Update script root", Description = "Host path containing the Docker update script.", IsSecret = false },
        new() { Key = "minioEndpoint", EnvironmentVariable = "ASPNETCORE_MINIO_ENDPOINT", Label = "MinIO endpoint", Description = "MinIO host and port used for uploaded link icons.", IsSecret = false },
        new() { Key = "minioAccessKey", EnvironmentVariable = "ASPNETCORE_MINIO_ACCESS_KEY", Label = "MinIO access key", Description = "Access key used for uploaded link icons.", IsSecret = true },
        new() { Key = "minioSecretKey", EnvironmentVariable = "ASPNETCORE_MINIO_SECRET_KEY", Label = "MinIO secret key", Description = "Secret key used for uploaded link icons.", IsSecret = true },
        new() { Key = "minioCdnUrl", EnvironmentVariable = "ASPNETCORE_MINIO_CDN_URL", Label = "MinIO CDN URL", Description = "Public CDN URL for uploaded link icons.", IsSecret = false },
        new() { Key = "minioBucketName", EnvironmentVariable = "ASPNETCORE_MINIO_BUCKET_NAME", Label = "MinIO bucket name", Description = "Bucket used for uploaded link icons.", IsSecret = false }
    };

    private readonly DatabaseContext _databaseContext;

    public SettingsService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public string? Resolve(string environmentVariable)
    {
        var storedValue = _databaseContext.Set<SettingRecord>()
            .AsNoTracking()
            .FirstOrDefault(setting => setting.Key == environmentVariable)?.Value;

        return string.IsNullOrWhiteSpace(storedValue)
            ? Environment.GetEnvironmentVariable(environmentVariable)
            : storedValue;
    }

    public static string? ResolveValue(string environmentVariable)
    {
        using var databaseContext = new DatabaseContext();
        return new SettingsService(databaseContext).Resolve(environmentVariable);
    }

    public SettingsResponse GetAll()
    {
        var storedSettings = _databaseContext.Set<SettingRecord>()
            .AsNoTracking()
            .ToDictionary(setting => setting.Key, setting => setting.Value);

        return new SettingsResponse
        {
            Settings = Definitions.Select(definition =>
            {
                var value = storedSettings.TryGetValue(definition.EnvironmentVariable, out var storedValue) && !string.IsNullOrWhiteSpace(storedValue)
                    ? storedValue
                    : Environment.GetEnvironmentVariable(definition.EnvironmentVariable) ?? string.Empty;

                return new Setting
                {
                    Key = definition.Key,
                    EnvironmentVariable = definition.EnvironmentVariable,
                    Label = definition.Label,
                    Description = definition.Description,
                    Value = value,
                    IsSecret = definition.IsSecret,
                    IsConfigured = !string.IsNullOrWhiteSpace(value)
                };
            }).ToList()
        };
    }

    public SettingsResponse Update(UpdateSettingsRequest request)
    {
        var validKeys = Definitions.Select(definition => definition.Key).ToHashSet(StringComparer.Ordinal);
        var definitionsByKey = Definitions.ToDictionary(definition => definition.Key, StringComparer.Ordinal);

        foreach (var setting in request.Settings.Where(setting => validKeys.Contains(setting.Key)))
        {
            var environmentVariable = definitionsByKey[setting.Key].EnvironmentVariable;
            var record = _databaseContext.Set<SettingRecord>().Find(environmentVariable);
            var value = setting.Value?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(value))
            {
                if (record is not null)
                    _databaseContext.Set<SettingRecord>().Remove(record);
            }
            else if (record is null)
            {
                _databaseContext.Set<SettingRecord>().Add(new SettingRecord { Key = environmentVariable, Value = value });
            }
            else
            {
                record.Value = value;
            }
        }

        _databaseContext.SaveChanges();
        return GetAll();
    }
}
