using HomeBoxLanding.Api.Features.Builds.Types;
using HomeBoxLanding.Api.Features.Deploys.Types;
using HomeBoxLanding.Api.Features.FuelPricePoller.Types;
using HomeBoxLanding.Api.Features.Links.Types;
using Microsoft.EntityFrameworkCore;

namespace HomeBoxLanding.Api.Data;

public class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    public DbSet<DeployRecord> Deploys { get; set; }
    public DbSet<LinkRecord> Links { get; set; }
    public DbSet<BuildRecord> Builds { get; set; }
    public DbSet<FuelPriceRecord> FuelPrices { get; set; }
    public DbSet<DockerBuildRecord> DockerBuilds { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = "server=localhost;database=home_app;username=postgres;password=postgres";

        var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        if (envName == "Production")
            connectionString = Environment.GetEnvironmentVariable("ConnectionString");

        optionsBuilder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}