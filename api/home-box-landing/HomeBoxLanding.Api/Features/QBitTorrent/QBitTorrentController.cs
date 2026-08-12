using HomeBoxLanding.Api.Features.Links;
using HomeBoxLanding.Api.Features.QBitTorrent.Types;
using Microsoft.AspNetCore.Mvc;

namespace HomeBoxLanding.Api.Features.QBitTorrent;

[ApiController]
[Route("api/[controller]")]
public class QBitTorrentController : ControllerBase
{
    private readonly QBitTorrentService _service;

    public QBitTorrentController()
    {
        _service = new QBitTorrentService(new LinksService(new LinksRepository()));
    }

    [HttpGet("stats")]
    public QBitTorrentStatsResponse GetStats([FromQuery] Guid identifier)
    {
        return _service.GetStats(identifier);
    }
}
