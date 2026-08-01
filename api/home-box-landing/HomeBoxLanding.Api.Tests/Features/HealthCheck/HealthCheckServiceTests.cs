using System.Net;
using System.Net.Http;
using HomeBoxLanding.Api.Features.HealthCheck;
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
    public async Task PerformHealthCheck_ReturnsServerErrorWhenTargetCannotBeReached()
    {
        var handler = new RecordingHandler(new HttpRequestException("certificate validation failed"));
        var service = CreateService(handler);

        var response = await service.PerformHealthCheck("example.com:443", true);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
    }

    private static HealthCheckService CreateService(HttpMessageHandler handler)
    {
        var httpClient = new HttpClient(handler);
        var factory = new TestHttpClientFactory(httpClient);
        return new HealthCheckService(factory);
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
