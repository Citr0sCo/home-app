using System.Diagnostics;
using System.Net;
using System.Security.Authentication;
using HomeBoxLanding.Api.Features.HealthCheck.Types;

namespace HomeBoxLanding.Api.Features.HealthCheck;

public class HealthCheckService
{
    private const string AcceptHeader = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7";
    private readonly HttpClient _httpClient;
    private readonly IHealthCheckHistoryRepository? _historyRepository;

    public HealthCheckService(
        IHttpClientFactory httpClientFactory,
        IHealthCheckHistoryRepository? historyRepository = null)
    {
        _httpClient = httpClientFactory.CreateClient("IgnoreSslClient");
        _httpClient.Timeout = TimeSpan.FromSeconds(10);
        _historyRepository = historyRepository;
    }

    public async Task<HealthCheckResponse> PerformHealthCheck(
        string url,
        bool isSecure,
        Guid? linkReference = null)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        if (environment == "Development")
        {
            var response = new HealthCheckResponse
            {
                StatusCode = HttpStatusCode.OK,
                StatusDescription = "Development",
                DurationInMilliseconds = 10,
            };
            await PersistAsync(linkReference, response).ConfigureAwait(false);
            return response;
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

            var response = new HealthCheckResponse
            {
                StatusCode = result.StatusCode,
                StatusDescription = result.ReasonPhrase,
                DurationInMilliseconds = stopwatch.ElapsedMilliseconds,
            };
            await PersistAsync(linkReference, response).ConfigureAwait(false);
            return response;
        }
        catch (Exception e)
        {
            var response = new HealthCheckResponse
            {
                StatusCode = IsSslFailure(e) ? HttpStatusCode.BadRequest : HttpStatusCode.InternalServerError,
                StatusDescription = e.Message,
                DurationInMilliseconds = stopwatch.ElapsedMilliseconds,
            };
            await PersistAsync(linkReference, response).ConfigureAwait(false);
            return response;
        }
        finally
        {
            stopwatch.Stop();
        }
    }

    private static bool IsSslFailure(Exception exception)
    {
        for (var current = exception; current is not null; current = current.InnerException)
        {
            if (current is AuthenticationException ||
                current.Message.Contains("SSL connection", StringComparison.OrdinalIgnoreCase) ||
                current.Message.Contains("TLS", StringComparison.OrdinalIgnoreCase) ||
                current.Message.Contains("certificate", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
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

    private async Task PersistAsync(Guid? linkReference, HealthCheckResponse response)
    {
        if (linkReference is not Guid linkIdentifier || _historyRepository is null)
            return;

        try
        {
            await _historyRepository.SaveAsync(new HealthCheckHistoryRecord
            {
                Identifier = Guid.NewGuid(),
                LinkIdentifier = linkIdentifier,
                RecordedAt = DateTime.UtcNow,
                DurationInMilliseconds = response.DurationInMilliseconds,
                StatusCode = (int)response.StatusCode,
                StatusDescription = response.StatusDescription
            }).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Failed to persist health check history: {exception.Message}");
        }
    }
}
