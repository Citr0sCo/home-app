using HomeBoxLanding.Api.Core.Events;
using HomeBoxLanding.Api.Core.Shell;
using HomeBoxLanding.Api.Data;
using HomeBoxLanding.Api.Features.Deploys;
using Microsoft.EntityFrameworkCore;

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
EventBus.Register(new DeployService(ShellService.Instance(), new DeployRepository()));
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

app.UseCors(setup => setup
    .SetIsOriginAllowed(_ => true)
    .AllowCredentials()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.Run();