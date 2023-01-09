using home_box_landing.api.Features.HealthCheck.Types;
using Microsoft.AspNetCore.Mvc;

namespace home_box_landing.api.Features.HealthCheck
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthCheckController : ControllerBase
    {
        private readonly HealthCheckService _service;

        public HealthCheckController()
        {
            _service = new HealthCheckService(new HttpClient());
        }

        [HttpGet]
        public HealthCheckResponse Get([FromQuery] string url)
        {
            return _service.PerformHealthCheck(url);
        }
    }
}