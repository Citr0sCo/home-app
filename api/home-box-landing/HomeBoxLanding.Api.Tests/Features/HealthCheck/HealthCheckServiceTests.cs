using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using HomeBoxLanding.Api.Features.HealthCheck;
using HomeBoxLanding.Api.Features.HealthCheck.Types;
using Microsoft.Extensions.Http;
using NUnit.Framework;

namespace HomeBoxLanding.Api.Tests.Features.HealthCheck;

[TestFixture]
public class HealthCheckServiceTests
{
    [Test]
    public async Task PerformHealthCheck_UsesHttpsForSecureTargets()
    {
        var handler = new RecordingHandler(HttpStatusCode.OK);
        var service = CreateService(handler);

        var response = await service.PerformHealthCheck("example.com:8443", true);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(handler.Request!.RequestUri!.AbsoluteUri, Is.EqualTo("https://example.com:8443/"));
    }

    [Test]
    public async Task PerformHealthCheck_UsesHttpForInsecureTargets()
    {
        var handler = new RecordingHandler(HttpStatusCode.OK);
        var service = CreateService(handler);

        var response = await service.PerformHealthCheck("example.com:8080", false);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(handler.Request!.RequestUri!.AbsoluteUri, Is.EqualTo("http://example.com:8080/"));
    }

    [Test]
    public async Task PerformHealthCheck_UsesDefaultPortWhenOneIsNotProvided()
    {
        var handler = new RecordingHandler(HttpStatusCode.OK);
        var service = CreateService(handler);

        var response = await service.PerformHealthCheck("example.com", true);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(handler.Request!.RequestUri!.AbsoluteUri, Is.EqualTo("https://example.com/"));
    }

    [Test]
    public async Task PerformHealthCheck_ReturnsWarningWhenSslConnectionCannotBeEstablished()
    {
        var handler = new RecordingHandler(new HttpRequestException(
            "The SSL connection could not be established, see inner exception.",
            new AuthenticationException("The certificate is not trusted.")));
        var service = CreateService(handler);

        var response = await service.PerformHealthCheck("home.lan:443", true);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task PerformHealthCheck_ReturnsServerErrorWhenTargetCannotBeReached()
    {
        var handler = new RecordingHandler(new HttpRequestException("connection refused"));
        var service = CreateService(handler);

        var response = await service.PerformHealthCheck("home.lan:443", true);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
    }

    [Test]
    public async Task GetLastHealthCheck_ReturnsPersistedStatusWithoutMakingAnHttpRequest()
    {
        var handler = new RecordingHandler(HttpStatusCode.OK);
        var linkIdentifier = Guid.NewGuid();
        var repository = new InMemoryHistoryRepository(new HealthCheckHistoryRecord
        {
            Identifier = Guid.NewGuid(),
            LinkIdentifier = linkIdentifier,
            RecordedAt = DateTime.UtcNow,
            DurationInMilliseconds = 42,
            StatusCode = (int)HttpStatusCode.NotFound,
            StatusDescription = "Not Found"
        });
        var service = CreateService(handler, repository);

        var response = await service.GetLastHealthCheckAsync(linkIdentifier);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        Assert.That(response.StatusDescription, Is.EqualTo("Not Found"));
        Assert.That(response.DurationInMilliseconds, Is.EqualTo(42));
        Assert.That(handler.Request, Is.Null);
    }

    [Test]
    public async Task GetLastHealthCheck_ReturnsServiceUnavailableWhenNoStatusHasBeenRecorded()
    {
        var handler = new RecordingHandler(HttpStatusCode.OK);
        var service = CreateService(handler, new InMemoryHistoryRepository(null));

        var response = await service.GetLastHealthCheckAsync(Guid.NewGuid());

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.ServiceUnavailable));
        Assert.That(response.StatusDescription, Is.EqualTo("No health check has been recorded yet."));
        Assert.That(handler.Request, Is.Null);
    }

    private static HealthCheckService CreateService(
        HttpMessageHandler handler,
        IHealthCheckHistoryRepository? historyRepository = null)
    {
        var httpClient = new HttpClient(handler);
        var factory = new TestHttpClientFactory(httpClient);
        return new HealthCheckService(factory, historyRepository);
    }

    private sealed class InMemoryHistoryRepository(HealthCheckHistoryRecord? record) : IHealthCheckHistoryRepository
    {
        private HealthCheckHistoryRecord? _record = record;

        public Task SaveAsync(HealthCheckHistoryRecord record, CancellationToken cancellationToken = default)
        {
            _record = record;
            return Task.CompletedTask;
        }

        public Task<HealthCheckHistoryRecord?> GetLatestAsync(
            Guid linkIdentifier,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_record?.LinkIdentifier == linkIdentifier ? _record : null);
        }

        public Task<List<HealthCheckHistoryRecord>> GetSinceAsync(
            DateTime since,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_record is not null && _record.RecordedAt >= since
                ? new List<HealthCheckHistoryRecord> { _record }
                : new List<HealthCheckHistoryRecord>());
        }

        public Task<int> DeleteOlderThanAsync(
            DateTime cutoff,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(0);
        }
    }

    private sealed class TestHttpClientFactory(HttpClient client) : IHttpClientFactory
    {
        public HttpClient CreateClient(string name) => client;
    }

    private sealed class RecordingHandler : HttpMessageHandler
    {
        private readonly HttpStatusCode? _statusCode;
        private readonly Exception? _exception;

        public RecordingHandler(HttpStatusCode statusCode)
        {
            _statusCode = statusCode;
        }

        public RecordingHandler(Exception exception)
        {
            _exception = exception;
        }

        public HttpRequestMessage? Request { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Request = request;

            if (_exception is not null)
            {
                throw _exception;
            }

            return Task.FromResult(new HttpResponseMessage(_statusCode!.Value)
            {
                RequestMessage = request,
            });
        }
    }
}
