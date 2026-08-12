namespace HomeBoxLanding.Api.Features.Settings.Types;

public class SettingDefinition
{
    public string Key { get; init; } = string.Empty;
    public string EnvironmentVariable { get; init; } = string.Empty;
    public string Label { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public bool IsSecret { get; init; }
}
