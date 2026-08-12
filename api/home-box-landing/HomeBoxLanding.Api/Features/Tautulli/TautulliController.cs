using HomeBoxLanding.Api.Features.Links;
using HomeBoxLanding.Api.Features.Tautulli.Types;
using Microsoft.AspNetCore.Mvc;

namespace HomeBoxLanding.Api.Features.Tautulli;

[ApiController]
[Route("api/[controller]")]
public class TautulliController : ControllerBase
{
    private readonly TautulliService _service;

    public TautulliController()
    {
        _service = new TautulliService(new LinksService(new LinksRepository()));
    }

    [HttpGet("stats")]
    public TautulliStatsResponse GetStats([FromQuery] Guid identifier)
    {
        return _service.GetStats(identifier);
    }
}
