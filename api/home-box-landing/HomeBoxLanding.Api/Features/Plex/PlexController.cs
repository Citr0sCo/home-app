using HomeBoxLanding.Api.Features.Links;
using HomeBoxLanding.Api.Features.Plex.Types;
using Microsoft.AspNetCore.Mvc;
using Minio;

namespace HomeBoxLanding.Api.Features.Plex;

[ApiController]
[Route("api/[controller]")]
public class PlexController : ControllerBase
{
    private readonly PlexService _service;

    public PlexController()
    {
        _service = new PlexService(new LinksService(new LinksRepository(), new MinioClient()));
    }

    [HttpGet("activity")]
    public PlexActivityResponse GetActivity()
    {
        return _service.GetActivity();
    }
}