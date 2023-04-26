using HomeBoxLanding.Api.Features.Builds.Types;
using HomeBoxLanding.Api.Features.Deploys.Types;
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
            },
            new LinkRecord
            {
                Identifier = Guid.NewGuid(),
                Name = "Overseerr",
                Url = "http://192.168.1.161:5055/",
                Host = "192.168.1.161",
                Port = 5055,
                IconUrl = "./assets/overseerr-logo.png",
                IsSecure = false,
                Category = "media",
                SortOrder = "B"
            },
            new LinkRecord
            {
                Identifier = Guid.NewGuid(),
                Name = "Radarr",
                Url = "http://192.168.1.25:7878/",
                Host = "192.168.1.25",
                Port = 7878,
                IconUrl = "./assets/radarr-logo.png",
                IsSecure = false,
                Category = "media",
                SortOrder = "C"
            },
            new LinkRecord
            {
                Identifier = Guid.NewGuid(),
                Name = "Sonarr",
                Url = "http://192.168.1.25:8989/",
                Host = "192.168.1.25",
                Port = 8989,
                IconUrl = "./assets/sonarr-logo.png",
                IsSecure = false,
                Category = "media",
                SortOrder = "D"
            },
            new LinkRecord
            {
                Identifier = Guid.NewGuid(),
                Name = "xTeve",
                Url = "http://192.168.1.25:34400/web/",
                Host = "192.168.1.25",
                Port = 34400,
                IconUrl = "./assets/xteve-logo.png",
                IsSecure = false,
                Category = "media",
                SortOrder = "E"
            },
            new LinkRecord
            {
                Identifier = Guid.NewGuid(),
                Name = "Mealie",
                Url = "http://192.168.1.161:9091/",
                Host = "192.168.1.161",
                Port = 9091,
                IconUrl = "./assets/mealie-logo.png",
                IsSecure = false,
                Category = "productivity",
                SortOrder = "F"
            },
            new LinkRecord
            {
                Identifier = Guid.NewGuid(),
                Name = "Planka",
                Url = "http://192.168.1.161:9096/",
                Host = "192.168.1.161",
                Port = 9096,
                IconUrl = "./assets/planka-logo.png",
                IsSecure = false,
                Category = "productivity",
                SortOrder = "G"
            },
            new LinkRecord
            {
                Identifier = Guid.NewGuid(),
                Name = "Trilium",
                Url = "http://192.168.1.161:9095/",
                Host = "192.168.1.161",
                Port = 9095,
                IconUrl = "./assets/trilium-logo.png",
                IsSecure = false,
                Category = "productivity",
                SortOrder = "H"
            },
            new LinkRecord
            {
                Identifier = Guid.NewGuid(),
                Name = "OctoPi",
                Url = "http://192.168.1.35/",
                Host = "192.168.1.35",
                Port = 80,
                IconUrl = "./assets/octopi-logo.png",
                IsSecure = false,
                Category = "productivity",
                SortOrder = "I"
            },
            new LinkRecord
            {
                Identifier = Guid.NewGuid(),
                Name = "Mattermost",
                Url = "http://192.168.1.142:8065/",
                Host = "192.168.1.142",
                Port = 8065,
                IconUrl = "./assets/mattermost-logo.png",
                IsSecure = false,
                Category = "productivity",
                SortOrder = "J"
            },
            new LinkRecord
            {
                Identifier = Guid.NewGuid(),
                Name = "Firefly III",
                Url = "http://192.168.1.161:8282/",
                Host = "192.168.1.161",
                Port = 8282,
                IconUrl = "./assets/firefly-iii-logo.jpeg",
                IsSecure = false,
                Category = "productivity",
                SortOrder = "K"
            },
            new LinkRecord
            {
                Identifier = Guid.NewGuid(),
                Name = "UniFi Controller",
                Url = "http://192.168.1.1/",
                Host = "192.168.1.1",
                Port = 80,
                IconUrl = "./assets/unifi-logo.png",
                IsSecure = true,
                Category = "system",
                SortOrder = "L"
            },
            new LinkRecord
            {
                Identifier = Guid.NewGuid(),
                Name = "Synology Dashboard",
                Url = "http://192.168.1.48:5000/",
                Host = "192.168.1.48",
                Port = 5000,
                IconUrl = "./assets/synology-logo.png",
                IsSecure = false,
                Category = "system",
                SortOrder = "M"
            },
            new LinkRecord
            {
                Identifier = Guid.NewGuid(),
                Name = "Prowlarr",
                Url = "http://192.168.1.25:9696/",
                Host = "192.168.1.25",
                Port = 9696,
                IconUrl = "./assets/prowlarr-logo.png",
                IsSecure = false,
                Category = "system",
                SortOrder = "N"
            },
            new LinkRecord
            {
                Identifier = Guid.NewGuid(),
                Name = "PiHole",
                Url = "http://192.168.1.50/admin/",
                Host = "192.168.1.50",
                Port = 80,
                IconUrl = "./assets/pihole-logo.png",
                IsSecure = false,
                Category = "system",
                SortOrder = "O"
            },
            new LinkRecord
            {
                Identifier = Guid.NewGuid(),
                Name = "Proxmox",
                Url = "https://192.168.1.15:8006/",
                Host = "192.168.1.15",
                Port = 8006,
                IconUrl = "./assets/proxmox-logo.png",
                IsSecure = true,
                Category = "system",
                SortOrder = "P"
            },
            new LinkRecord
            {
                Identifier = Guid.NewGuid(),
                Name = "InfluxDB",
                Url = "http://192.168.1.161:8086/",
                Host = "192.168.1.161",
                Port = 8086,
                IconUrl = "./assets/influxdb-logo.png",
                IsSecure = false,
                Category = "system",
                SortOrder = "Q"
            },
            new LinkRecord
            {
                Identifier = Guid.NewGuid(),
                Name = "Grafana",
                Url = "http://192.168.1.161:3000/",
                Host = "192.168.1.161",
                Port = 3000,
                IconUrl = "./assets/grafana-logo.png",
                IsSecure = false,
                Category = "system",
                SortOrder = "R"
            },
            new LinkRecord
            {
                Identifier = Guid.NewGuid(),
                Name = "Tautulli",
                Url = "http://192.168.1.161:8181/",
                Host = "192.168.1.161",
                Port = 8181,
                IconUrl = "./assets/tautulli-logo.png",
                IsSecure = false,
                Category = "system",
                SortOrder = "S"
            },
            new LinkRecord
            {
                Identifier = Guid.NewGuid(),
                Name = "Portainer",
                Url = "https://192.168.1.161:9443/",
                Host = "192.168.1.161",
                Port = 9443,
                IconUrl = "./assets/portainer-logo.png",
                IsSecure = true,
                Category = "tools",
                SortOrder = "T"
            },
            new LinkRecord
            {
                Identifier = Guid.NewGuid(),
                Name = "Flood",
                Url = "http://192.168.1.25:8081/",
                Host = "192.168.1.25",
                Port = 8081,
                IconUrl = "./assets/flood-logo.png",
                IsSecure = false,
                Category = "tools",
                SortOrder = "U"
            },
            new LinkRecord
            {
                Identifier = Guid.NewGuid(),
                Name = "Nginx Proxy Manager",
                Url = "http://192.168.1.161:81/",
                Host = "192.168.1.161",
                Port = 81,
                IconUrl = "./assets/nginx-proxy-manager-logo.png",
                IsSecure = false,
                Category = "tools",
                SortOrder = "V"
            },
            new LinkRecord
            {
                Identifier = Guid.NewGuid(),
                Name = "Homebridge",
                Url = "http://192.168.1.205:8581/",
                Host = "192.168.1.205",
                Port = 8581,
                IconUrl = "./assets/homebridge-logo.png",
                IsSecure = false,
                Category = "tools",
                SortOrder = "X"
            },
            new LinkRecord
            {
                Identifier = Guid.NewGuid(),
                Name = "Home Assistant",
                Url = "http://192.168.1.254:8123/",
                Host = "192.168.1.254",
                Port = 8123,
                IconUrl = "./assets/home-assistant-logo.png",
                IsSecure = false,
                Category = "tools",
                SortOrder = "Y"
            },
            new LinkRecord
            {
                Identifier = Guid.NewGuid(),
                Name = "Phoscon",
                Url = "http://192.168.1.50:8080/",
                Host = "192.168.1.50",
                Port = 8080,
                IconUrl = "./assets/phoscon-logo.jpeg",
                IsSecure = false,
                Category = "tools",
                SortOrder = "Z"
            },
            new LinkRecord
            {
                Identifier = Guid.NewGuid(),
                Name = "Uptime Kuma",
                Url = "http://192.168.1.161:3001/",
                Host = "192.168.1.161",
                Port = 3001,
                IconUrl = "./assets/uptime-kuma-logo.png",
                IsSecure = false,
                Category = "tools",
                SortOrder = "ZA"
            },
            new LinkRecord
            {
                Identifier = Guid.NewGuid(),
                Name = "Tdarr",
                Url = "http://192.168.1.25:8265/",
                Host = "192.168.1.25",
                Port = 8265,
                IconUrl = "./assets/tdarr-logo.png",
                IsSecure = false,
                Category = "tools",
                SortOrder = "ZB"
            }
        );
    }
}