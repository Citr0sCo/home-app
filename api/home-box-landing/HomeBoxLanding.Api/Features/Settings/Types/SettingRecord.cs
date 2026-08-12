using System.ComponentModel.DataAnnotations;

namespace HomeBoxLanding.Api.Features.Settings.Types;

public class SettingRecord
{
    [Key]
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
