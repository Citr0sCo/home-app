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

        [HttpPost]
        public HealthCheckResponse Get(HealthCheckRequest request)
        {
            return _service.PerformHealthCheck(request.Url);
        }
    }
}