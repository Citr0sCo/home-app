using System.Diagnostics;
using System.Net;
using HomeBoxLanding.Api.Features.HealthCheck.Types;

namespace HomeBoxLanding.Api.Features.HealthCheck;

public class HealthCheckService
{
    private const string AcceptHeader = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7";
    private readonly HttpClient _httpClient;

    public HealthCheckService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("IgnoreSslClient");
        _httpClient.Timeout = TimeSpan.FromSeconds(10);
    }

    public async Task<HealthCheckResponse> PerformHealthCheck(string url, bool isSecure)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        if (environment == "Development")
        {
            return new HealthCheckResponse
            {
                StatusCode = HttpStatusCode.OK,
                StatusDescription = "Development",
                DurationInMilliseconds = 10,
            };
        }

        var stopwatch = Stopwatch.StartNew();

        try
        {
            var target = BuildTargetUri(url, isSecure);
            using var request = new HttpRequestMessage(HttpMethod.Get, target);
            request.Headers.Accept.ParseAdd(AcceptHeader);
            request.Headers.AcceptLanguage.ParseAdd("en-GB,en-US;q=0.9,en;q=0.8");
            request.Headers.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

            using var result = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            return new HealthCheckResponse
            {
                StatusCode = result.StatusCode,
                StatusDescription = result.ReasonPhrase,
                DurationInMilliseconds = stopwatch.ElapsedMilliseconds,
            };
        }
        catch (Exception e)
        {
            return new HealthCheckResponse
            {
                StatusCode = HttpStatusCode.InternalServerError,
                StatusDescription = e.Message,
                DurationInMilliseconds = stopwatch.ElapsedMilliseconds,
            };
        }
        finally
        {
            stopwatch.Stop();
        }
    }

    private static Uri BuildTargetUri(string url, bool isSecure)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException("A host is required for a health check.", nameof(url));
        }

        var scheme = isSecure ? Uri.UriSchemeHttps : Uri.UriSchemeHttp;
        var address = url.Trim();

        if (!address.Contains("://", StringComparison.Ordinal))
        {
            address = $"{scheme}://{address}";
        }

        if (!Uri.TryCreate(address, UriKind.Absolute, out var target) ||
            !string.Equals(target.Scheme, scheme, StringComparison.OrdinalIgnoreCase))
        {
            throw new UriFormatException($"The health check target '{url}' is invalid.");
        }

        return target;
    }
}
