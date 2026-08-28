using HomeBoxLanding.Api.Core.Types;

namespace HomeBoxLanding.Api.Features.HealthCheck.Types;

public class HealthCheckHistoryResponse : CommunicationResponse
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public List<HealthCheckLinkHistory> Links { get; set; } = new();
}

public class HealthCheckLinkHistory
{
    public Guid Identifier { get; set; }
    public string? Name { get; set; }
    public string? Url { get; set; }
    public List<HealthCheckHistoryPoint> Samples { get; set; } = new();
}

public class HealthCheckHistoryPoint
{
    public DateTime RecordedAt { get; set; }
    public long DurationInMilliseconds { get; set; }
    public int StatusCode { get; set; }
    public string? StatusDescription { get; set; }
}
