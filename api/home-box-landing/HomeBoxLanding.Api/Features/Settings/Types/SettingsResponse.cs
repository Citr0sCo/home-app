namespace HomeBoxLanding.Api.Features.Settings.Types;

public class SettingsResponse
{
    public List<Setting> Settings { get; set; } = new();
}

public class Setting
{
    public string Key { get; set; } = string.Empty;
    public string EnvironmentVariable { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public bool IsSecret { get; set; }
    public bool IsConfigured { get; set; }
}
