using HomeBoxLanding.Api.Core.Events;
using HomeBoxLanding.Api.Core.Shell;
using HomeBoxLanding.Api.Data;
using HomeBoxLanding.Api.Features.Builds;
using HomeBoxLanding.Api.Features.Deploys;
using HomeBoxLanding.Api.Features.Stats;
using HomeBoxLanding.Api.Features.WebSockets;
using Microsoft.EntityFrameworkCore;
using WebSocketManager = HomeBoxLanding.Api.Features.WebSockets.WebSocketManager;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DatabaseContext>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
EventBus.Register(new DeployService(ShellService.Instance(), new DeployRepository(), new BuildsService(new BuildsRepository())));
EventBus.Register(WebSocketManager.Instance());
EventBus.Register(new StatsService(ShellService.Instance()));
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
app.UseAuthorization();
app.MapControllers();
app.UseWebSockets();

app.UseCors(setup => setup
    .SetIsOriginAllowed(_ => true)
    .AllowCredentials()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.Run();