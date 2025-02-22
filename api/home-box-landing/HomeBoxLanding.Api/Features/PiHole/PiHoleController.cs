using HomeBoxLanding.Api.Features.Links;
using HomeBoxLanding.Api.Features.PiHole.Types;
using Microsoft.AspNetCore.Mvc;
using Minio;

namespace HomeBoxLanding.Api.Features.PiHole;

[ApiController]
[Route("api/[controller]")]
public class PiHoleController : ControllerBase
{
    private readonly PiHoleService _service;

    public PiHoleController()
    {
        _service = new PiHoleService(new LinksService(new LinksRepository(), new MinioClient()));
    }

    [HttpGet("activity")]
    public PiHoleActivityResponse GetActivity([FromQuery] Guid identifier)
    {
        return _service.GetActivity(identifier);
    }
}