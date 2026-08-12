using HomeBoxLanding.Api.Features.Settings.Types;
using Microsoft.AspNetCore.Mvc;

namespace HomeBoxLanding.Api.Features.Settings;

[ApiController]
[Route("api/settings")]
public class SettingsController : ControllerBase
{
    private readonly SettingsService _service;

    public SettingsController(SettingsService service)
    {
        _service = service;
    }

    [HttpGet]
    public SettingsResponse GetAll() => _service.GetAll();

    [HttpPut]
    public SettingsResponse Update([FromBody] UpdateSettingsRequest request) => _service.Update(request);
}
