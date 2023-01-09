using System.Net;
using System.Security.Authentication;
using home_box_landing.api.Features.HealthCheck.Types;

namespace home_box_landing.api.Features.HealthCheck
{
    public class HealthCheckService
    {
        private readonly HttpClient _httpClient;

        public HealthCheckService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public HealthCheckResponse PerformHealthCheck(string url, bool isSecure)
        {
            var prefix = isSecure ? "https" : "http";

            try
            {
                var result = _httpClient.GetAsync($"{prefix}://{url}").Result;
                var responseMessage = result.Content.ReadAsStringAsync().Result;

                return new HealthCheckResponse
                {
                    StatusCode = result.StatusCode,
                    StatusDescription = result.ReasonPhrase
                };
            }
            catch (Exception e)
            {
                if (isSecure)
                {
                    return new HealthCheckResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        StatusDescription = e.Message
                    };
                }
                
                return new HealthCheckResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    StatusDescription = e.Message
                };
            }
        }
    }
}