using HomeBoxLanding.Api.Core.Shell;
using HomeBoxLanding.Api.Features.Stats.Types;
using Microsoft.AspNetCore.Mvc;

namespace HomeBoxLanding.Api.Features.Stats;

[ApiController]
[Route("api/[controller]")]
public class StatsController : ControllerBase
{
    private readonly StatsService _service;

    public StatsController()
    {
        _service = new StatsService(ShellService.Instance(), StatsServiceCache.Instance());
    }

    [HttpGet]
    public StatsResponse Get()
    {
        return _service.GetServerStats();
    }

    [HttpGet("history")]
    public async Task<ServerStatsHistoryResponse> GetHistory(CancellationToken cancellationToken)
    {
        return await _service.GetServerStatsHistoryAsync(cancellationToken);
    }
}