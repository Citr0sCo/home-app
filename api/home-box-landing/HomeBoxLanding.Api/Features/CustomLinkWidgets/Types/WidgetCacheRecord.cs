namespace HomeBoxLanding.Api.Features.CustomLinkWidgets.Types;

public class WidgetCacheRecord
{
    public Guid LinkIdentifier { get; set; }
    public string WidgetType { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
}
