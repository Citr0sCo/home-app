using System.Net;

namespace home_box_landing.api.Features.HealthCheck.Types
{
    public class HealthCheckResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string? StatusDescription { get; set; }
    }
}