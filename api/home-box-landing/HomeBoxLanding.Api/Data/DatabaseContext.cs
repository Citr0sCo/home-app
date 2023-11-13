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
        /*
        modelBuilder.Entity<LinkRecord>().HasData(
            new LinkRecord
            {
                Identifier = Guid.NewGuid(),
                Name = "Plex",
                Url = "http://192.168.1.74:32400/",
                Host = "192.168.1.74",
                Port = 32400,
                IconUrl = "./assets/plex-logo.png",
                IsSecure = false,
                Category = "media",
                SortOrder = "A"
            }
        );
        */
    }
}