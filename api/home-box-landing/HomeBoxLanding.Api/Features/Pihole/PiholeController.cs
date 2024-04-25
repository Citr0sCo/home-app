using HomeBoxLanding.Api.Features.Links;
using HomeBoxLanding.Api.Features.Pihole.Types;
using Microsoft.AspNetCore.Mvc;
using Minio;

namespace HomeBoxLanding.Api.Features.Pihole;

[ApiController]
[Route("api/[controller]")]
public class PiholeController : ControllerBase
{
    private readonly PiholeService _service;

    public PiholeController()
    {
        _service = new PiholeService(new LinksService(new LinksRepository(), new MinioClient()));
    }

    [HttpGet("activity")]
    public PiholeActivityResponse GetActivity([FromQuery] Guid identifier)
    {
        return _service.GetActivity(identifier);
    }
}