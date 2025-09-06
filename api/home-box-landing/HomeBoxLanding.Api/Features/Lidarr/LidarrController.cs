using HomeBoxLanding.Api.Features.Lidarr.Types;
using HomeBoxLanding.Api.Features.Links;
using Microsoft.AspNetCore.Mvc;

namespace HomeBoxLanding.Api.Features.Lidarr;

[ApiController]
[Route("api/[controller]")]
public class LidarrController : ControllerBase
{
    private readonly LidarrService _service;

    public LidarrController()
    {
        _service = new LidarrService(new LinksService(new LinksRepository()));
    }

    [HttpGet("activity")]
    public LidarrActivityResponse GetActivity()
    {
        return _service.GetActivity();
    }
}