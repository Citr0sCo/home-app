using HomeBoxLanding.Api.Features.Settings;
using Microsoft.AspNetCore.Mvc;

namespace HomeBoxLanding.Api.Features.Configs;

[ApiController]
[Route("api/configs")]
public class ConfigsController : ControllerBase
{
    private readonly SettingsService _settingsService;

    public ConfigsController(SettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    [HttpGet]
    public GetAllConfigsResponse GetAll()
    {
        return new GetAllConfigsResponse
        {
            WeatherApiKey = _settingsService.Resolve("ASPNETCORE_WEATHER_API_KEY"),
            MapsApiKey = _settingsService.Resolve("ASPNETCORE_MAPS_API_KEY")
        };
    }

    public class GetAllConfigsResponse
    {
        public string? WeatherApiKey { get; set; }
        public string? MapsApiKey { get; set; }
    }
}