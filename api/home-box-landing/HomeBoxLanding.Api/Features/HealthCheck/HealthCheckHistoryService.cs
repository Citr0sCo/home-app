using HomeBoxLanding.Api.Features.HealthCheck.Types;
using HomeBoxLanding.Api.Features.Links;

namespace HomeBoxLanding.Api.Features.HealthCheck;

public class HealthCheckHistoryService
{
    private readonly IHealthCheckHistoryRepository _historyRepository;
    private readonly ILinksRepository _linksRepository;

    public HealthCheckHistoryService(
        IHealthCheckHistoryRepository historyRepository,
        ILinksRepository? linksRepository = null)
    {
        _historyRepository = historyRepository;
        _linksRepository = linksRepository ?? new LinksRepository();
    }

    public async Task<HealthCheckHistoryResponse> GetHistoryAsync(
        int days = 7,
        CancellationToken cancellationToken = default)
    {
        var rangeDays = Math.Clamp(days, 1, 7);
        var to = DateTime.UtcNow;
        var from = to.AddDays(-rangeDays);

        try
        {
            var records = await _historyRepository
                .GetSinceAsync(from, cancellationToken)
                .ConfigureAwait(false);
            var recordsByLink = records
                .GroupBy(record => record.LinkIdentifier)
                .ToDictionary(group => group.Key, group => group.ToList());

            var links = _linksRepository.GetAll()
                .OrderBy(link => link.Name)
                .Select(link => new HealthCheckLinkHistory
                {
                    Identifier = link.Identifier,
                    Name = link.Name,
                    Url = link.Url,
                    Samples = recordsByLink.TryGetValue(link.Identifier, out var linkRecords)
                        ? linkRecords.Select(Map).ToList()
                        : new List<HealthCheckHistoryPoint>()
                })
                .ToList();

            return new HealthCheckHistoryResponse
            {
                From = from,
                To = to,
                Links = links
            };
        }
        catch (Exception exception)
        {
            return new HealthCheckHistoryResponse
            {
                From = from,
                To = to,
                HasError = true,
                Error = new Core.Types.Error
                {
                    Code = Core.Types.ErrorCode.DatabaseError,
                    UserMessage = "Failed to load health check history.",
                    TechnicalMessage = exception.Message
                }
            };
        }
    }

    private static HealthCheckHistoryPoint Map(HealthCheckHistoryRecord record)
    {
        return new HealthCheckHistoryPoint
        {
            RecordedAt = record.RecordedAt,
            DurationInMilliseconds = record.DurationInMilliseconds,
            StatusCode = record.StatusCode,
            StatusDescription = record.StatusDescription
        };
    }
}
