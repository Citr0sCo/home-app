namespace HomeBoxLanding.Api.Features.Settings.Types;

public class UpdateSettingsRequest
{
    public List<SettingValue> Settings { get; set; } = new();
}

public class SettingValue
{
    public string Key { get; set; } = string.Empty;
    public string? Value { get; set; }
}
