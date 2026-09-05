using HomeBoxLanding.Api.Core.Events;
using HomeBoxLanding.Api.Core.Json;
using HomeBoxLanding.Api.Core.Shell;
using HomeBoxLanding.Api.Data;
using HomeBoxLanding.Api.Features.FuelPricePoller;
using HomeBoxLanding.Api.Features.HealthCheck;
using HomeBoxLanding.Api.Features.Lidarr;
using HomeBoxLanding.Api.Features.Links;
using HomeBoxLanding.Api.Features.PiHole;
using HomeBoxLanding.Api.Features.Plex;
using HomeBoxLanding.Api.Features.QBitTorrent;
using HomeBoxLanding.Api.Features.Radarr;
using HomeBoxLanding.Api.Features.Readarr;
using HomeBoxLanding.Api.Features.Sonarr;
using HomeBoxLanding.Api.Features.Stats;
using HomeBoxLanding.Api.Features.Settings;
using HomeBoxLanding.Api.Features.Tautulli;
using HomeBoxLanding.Api.Features.UptimeKuma;
using Microsoft.EntityFrameworkCore;
using WebSocketManager = HomeBoxLanding.Api.Features.WebSockets.WebSocketManager;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DatabaseContext>();

builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.PropertyNamingPolicy = null;
        opts.JsonSerializerOptions.Converters.Add(new UtcDateTimeJsonConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<SettingsService>();

builder.Services.AddHttpClient("IgnoreSslClient").ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    CheckCertificateRevocationList = false,
    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
});

var app = builder.Build();

Console.WriteLine("Applying migrations...");
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DatabaseContext>();
    context.Database.Migrate();
}

Console.WriteLine("Done");

Console.WriteLine("Registering EventBus...");
EventBus.Register(WebSocketManager.Instance());
EventBus.Register(new PlexService(new LinksService(new LinksRepository())));
EventBus.Register(new PiHoleService(new LinksService(new LinksRepository())));
EventBus.Register(new QBitTorrentService(new LinksService(new LinksRepository())));
EventBus.Register(new TautulliService(new LinksService(new LinksRepository())));
EventBus.Register(new UptimeKumaService(new LinksService(new LinksRepository())));
EventBus.Register(new HealthCheckHistoryCleanupService());
EventBus.Register(new RadarrService(new LinksService(new LinksRepository())));
EventBus.Register(new SonarrService(new LinksService(new LinksRepository())));
EventBus.Register(new LidarrService(new LinksService(new LinksRepository())));
EventBus.Register(new ReadarrService(new LinksService(new LinksRepository())));
EventBus.Register(new StatsService(ShellService.Instance(), StatsServiceCache.Instance()));
EventBus.Register(FuelPricePoller.Instance());
Console.WriteLine("Done");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Lifetime.ApplicationStarted.Register(EventBus.OnStarted);
app.Lifetime.ApplicationStopping.Register(EventBus.OnStopping);
app.Lifetime.ApplicationStopped.Register(EventBus.OnStopped);

//app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
//app.MapStaticAssets();
app.UseAuthorization();
app.MapControllers();
app.UseWebSockets();

app.UseCors(setup => setup
    .SetIsOriginAllowed(_ => true)
    .AllowCredentials()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.Run();