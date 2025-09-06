using HomeBoxLanding.Api.Features.Links;
using HomeBoxLanding.Api.Features.Readarr.Types;
using Microsoft.AspNetCore.Mvc;

namespace HomeBoxLanding.Api.Features.Readarr;

[ApiController]
[Route("api/[controller]")]
public class ReadarrController : ControllerBase
{
    private readonly ReadarrService _service;

    public ReadarrController()
    {
        _service = new ReadarrService(new LinksService(new LinksRepository()));
    }

    [HttpGet("activity")]
    public ReadarrActivityResponse GetActivity()
    {
        return _service.GetActivity();
    }
}