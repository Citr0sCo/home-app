using System.Net;
using HomeBoxLanding.Api.Features.HealthCheck.Types;

namespace HomeBoxLanding.Api.Features.HealthCheck;

public class HealthCheckService
{
    private readonly HttpClient _httpClient;

    public HealthCheckService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<HealthCheckResponse> PerformHealthCheck(string url, bool isSecure)
    {
        var prefix = isSecure ? "https" : "http";

        try
        {
            _httpClient.Timeout = TimeSpan.FromSeconds(10);
            _httpClient.DefaultRequestHeaders.Add("Accept","text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
            _httpClient.DefaultRequestHeaders.Add("Accept-Language","en-GB,en-US;q=0.9,en;q=0.8");
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
            
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