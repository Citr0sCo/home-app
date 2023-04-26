using HomeBoxLanding.Api.Features.Plex.Types;
using Microsoft.AspNetCore.Mvc;

namespace HomeBoxLanding.Api.Features.Plex;

[ApiController]
[Route("api/[controller]")]
public class PlexController : ControllerBase
{
    private readonly PlexService _service;

    public PlexController()
    {
        _service = new PlexService();
    }

    [HttpGet("activity")]
    public PlexActivityResponse GetActivity()
    {
        return _service.GetActivity();
    }
}