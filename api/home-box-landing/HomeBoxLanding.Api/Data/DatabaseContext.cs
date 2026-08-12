using HomeBoxLanding.Api.Features.Builds.Types;
using HomeBoxLanding.Api.Features.Columns.Types;
using HomeBoxLanding.Api.Features.Folders.Types;
using HomeBoxLanding.Api.Features.FuelPricePoller.Types;
using HomeBoxLanding.Api.Features.Links.Types;
using HomeBoxLanding.Api.Features.Notepad.Types;
using HomeBoxLanding.Api.Features.Stats.Types;
using HomeBoxLanding.Api.Features.Settings.Types;
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

    public DbSet<ColumnRecord> Columns { get; set; }
    public DbSet<FolderRecord> Folders { get; set; }
    public DbSet<LinkRecord> Links { get; set; }
    public DbSet<FuelPriceRecord> FuelPrices { get; set; }
    public DbSet<DockerBuildRecord> DockerBuilds { get; set; }
    public DbSet<NotepadRecord> Notepads { get; set; }
    public DbSet<ServerStatsHistoryRecord> ServerStatsHistory { get; set; }
    public DbSet<SettingRecord> Settings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        Directory.CreateDirectory("assets");
        optionsBuilder.UseSqlite("Data Source=assets/home-app.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ServerStatsHistoryRecord>()
            .HasIndex(record => record.RecordedAt);

        // Declared explicitly so the optional folder relationship cascades in the database rather than
        // restricting deletes, which would otherwise clash with the cascade a column delete triggers.
        modelBuilder.Entity<LinkRecord>()
            .HasOne(x => x.Folder)
            .WithMany(x => x.Links)
            .HasForeignKey(x => x.FolderIdentifier)
            .OnDelete(DeleteBehavior.Cascade);
    }
}