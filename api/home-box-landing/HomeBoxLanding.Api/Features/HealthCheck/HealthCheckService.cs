using System.Net;
using HomeBoxLanding.Api.Features.HealthCheck.Types;

namespace HomeBoxLanding.Api.Features.HealthCheck;

public class HealthCheckService
{
    private readonly HttpClient _httpClient;

    public HealthCheckService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HealthCheckResponse> PerformHealthCheck(string url, bool isSecure)
    {
        var prefix = isSecure ? "https" : "http";

        try
        {
            _httpClient.Timeout = TimeSpan.FromSeconds(10);
            var result = await _httpClient.GetAsync($"{prefix}://{url}").ConfigureAwait(false);
            var responseMessage = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

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