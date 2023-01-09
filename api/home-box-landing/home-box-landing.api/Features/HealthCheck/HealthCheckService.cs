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

        public HealthCheckResponse PerformHealthCheck(string url)
        {
            
            var result = _httpClient.GetAsync($"http://{url}").Result;
            var responseMessage = result.Content.ReadAsStringAsync().Result;
                       
            return new HealthCheckResponse
            {
                StatusCode = result.StatusCode,
                StatusDescription = result.ReasonPhrase
            };
        }
    }
}