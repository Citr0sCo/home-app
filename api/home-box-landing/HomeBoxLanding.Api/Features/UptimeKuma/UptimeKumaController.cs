using HomeBoxLanding.Api.Features.Links;
using HomeBoxLanding.Api.Features.UptimeKuma.Types;
using Microsoft.AspNetCore.Mvc;

namespace HomeBoxLanding.Api.Features.UptimeKuma;

[ApiController]
[Route("api/[controller]")]
public class UptimeKumaController : ControllerBase
{
    private readonly UptimeKumaService _service;

    public UptimeKumaController()
    {
        _service = new UptimeKumaService(new LinksService(new LinksRepository()));
    }

    [HttpGet("activity")]
    public async Task<UptimeKumaActivityResponse> GetActivity([FromQuery] Guid identifier)
    {
        return await _service.GetActivity(identifier);
    }
}