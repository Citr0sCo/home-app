using System.ComponentModel.DataAnnotations;

namespace HomeBoxLanding.Api.Features.HealthCheck.Types;

public class HealthCheckHistoryRecord
{
    [Key]
    public Guid Identifier { get; set; }
    public Guid LinkIdentifier { get; set; }
    public DateTime RecordedAt { get; set; }
    public long DurationInMilliseconds { get; set; }
    public int StatusCode { get; set; }
    public string? StatusDescription { get; set; }
}
