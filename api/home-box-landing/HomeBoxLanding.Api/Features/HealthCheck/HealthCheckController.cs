using HomeBoxLanding.Api.Features.HealthCheck.Types;
using Microsoft.AspNetCore.Mvc;

namespace HomeBoxLanding.Api.Features.HealthCheck;

[ApiController]
[Route("api/[controller]")]
public class HealthCheckController : ControllerBase
{
    private readonly HealthCheckService _service;
    private readonly HealthCheckHistoryService _historyService;

    public HealthCheckController(IHttpClientFactory httpClientFactory)
    {
        var historyRepository = new HealthCheckHistoryRepository();
        _service = new HealthCheckService(httpClientFactory, historyRepository);
        _historyService = new HealthCheckHistoryService(historyRepository);
    }

    [HttpGet]
    public async Task<HealthCheckResponse> Get(
        [FromQuery] string url,
        [FromQuery] bool isSecure,
        [FromQuery] Guid? linkReference = null)
    {
        return await _service.PerformHealthCheck(url, isSecure, linkReference).ConfigureAwait(false);
    }

    [HttpGet("history")]
    public async Task<HealthCheckHistoryResponse> GetHistory(
        [FromQuery] int days = 7,
        CancellationToken cancellationToken = default)
    {
        return await _historyService.GetHistoryAsync(days, cancellationToken).ConfigureAwait(false);
    }
}