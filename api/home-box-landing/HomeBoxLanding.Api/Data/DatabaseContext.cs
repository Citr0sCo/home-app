using HomeBoxLanding.Api.Features.Builds.Types;
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

    public DbSet<LinkRecord> Links { get; set; }
    public DbSet<FuelPriceRecord> FuelPrices { get; set; }
    public DbSet<DockerBuildRecord> DockerBuilds { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=home-app.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}