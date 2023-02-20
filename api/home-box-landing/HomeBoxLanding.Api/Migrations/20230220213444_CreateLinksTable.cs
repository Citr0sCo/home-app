using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HomeBoxLanding.Api.Migrations
{
    /// <inheritdoc />
    public partial class CreateLinksTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Links",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    Host = table.Column<string>(type: "text", nullable: false),
                    Port = table.Column<int>(type: "integer", nullable: false),
                    IsSecure = table.Column<bool>(type: "boolean", nullable: false),
                    IconUrl = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Links", x => x.Identifier);
                });

            migrationBuilder.InsertData(
                table: "Links",
                columns: new[] { "Identifier", "Category", "Host", "IconUrl", "IsSecure", "Name", "Port", "SortOrder", "Url" },
                values: new object[,]
                {
                    { new Guid("1d6c91f4-c00a-4b3f-ba54-abf54034f8af"), "system", "192.168.1.25", "./assets/prowlarr-logo.png", false, "Prowlarr", 9696, 2, "http://192.168.1.25:9696/" },
                    { new Guid("20622552-2f72-4b9b-8f95-7eca979bbe9d"), "system", "192.168.1.161", "./assets/tautulli-logo.png", false, "Tautulli", 8181, 7, "http://192.168.1.161:8181/" },
                    { new Guid("24b0fa51-101a-4858-898c-3f4f23844fd6"), "tools", "192.168.1.254", "./assets/home-assistant-logo.png", false, "Home Assistant", 8123, 4, "http://192.168.1.254:8123/" },
                    { new Guid("24f6048f-3727-4e89-acda-f76355306b42"), "tools", "192.168.1.161", "./assets/uptime-kuma-logo.png", false, "Uptime Kuma", 3001, 6, "http://192.168.1.161:3001/" },
                    { new Guid("26dcb458-6b5e-4a5d-98c7-62c2a0e443f3"), "system", "192.168.1.1", "./assets/unifi-logo.png", true, "UniFi Controller", 80, 0, "http://192.168.1.1/" },
                    { new Guid("2f46896f-445d-41df-9b57-f6ebe5c8dceb"), "tools", "192.168.1.25", "./assets/flood-logo.png", false, "Flood", 8081, 1, "http://192.168.1.25:8081/" },
                    { new Guid("590ffe1a-5dd9-441b-81b7-5abdf4eb18fc"), "productivity", "192.168.1.35", "./assets/octopi-logo.png", false, "OctoPi", 80, 3, "http://192.168.1.35/" },
                    { new Guid("5926fbfa-b344-4a51-8f62-db40f2cce704"), "productivity", "192.168.1.161", "./assets/trilium-logo.png", false, "Trilium", 9095, 2, "http://192.168.1.161:9095/" },
                    { new Guid("5d555bdd-d4b3-4b23-be78-e7ecbf25c9c0"), "media", "192.168.1.74", "./assets/plex-logo.png", false, "Plex", 32400, 0, "http://192.168.1.74:32400/" },
                    { new Guid("60585a7e-5c00-4854-ae1e-31ff8e13d75f"), "system", "192.168.1.48", "./assets/synology-logo.png", false, "Synology Dashboard", 5000, 1, "http://192.168.1.48:5000/" },
                    { new Guid("70e2f925-3b89-403f-8f8d-cbd50c6ad828"), "tools", "192.168.1.25", "./assets/tdarr-logo.png", false, "Tdarr", 8265, 7, "http://192.168.1.25:8265/" },
                    { new Guid("7f0074b0-f382-4351-a0de-00182c8bccc2"), "system", "192.168.1.50", "./assets/pihole-logo.png", false, "PiHole", 80, 3, "http://192.168.1.50/admin/" },
                    { new Guid("85b9a3bd-cc19-425f-9d94-db06e1674e87"), "tools", "192.168.1.161", "./assets/nginx-proxy-manager-logo.png", false, "Nginx Proxy Manager", 81, 2, "http://192.168.1.161:81/" },
                    { new Guid("a037667b-23cb-4bcb-ae4d-c2cd0db99fa3"), "tools", "192.168.1.50", "./assets/phoscon-logo.jpeg", false, "Phoscon", 8080, 5, "http://192.168.1.50:8080/" },
                    { new Guid("a0495abb-58e9-41a4-b8db-23466f8dac95"), "productivity", "192.168.1.161", "./assets/firefly-iii-logo.jpeg", false, "Firefly III", 8282, 5, "http://192.168.1.161:8282/" },
                    { new Guid("a2b5b22a-972b-4fc0-9057-3426c492e35b"), "productivity", "192.168.1.161", "./assets/planka-logo.png", false, "Planka", 9096, 1, "http://192.168.1.161:9096/" },
                    { new Guid("a75321f8-c057-428f-917f-e933083c86cf"), "productivity", "192.168.1.161", "./assets/mealie-logo.png", false, "Mealie", 9091, 0, "http://192.168.1.161:9091/" },
                    { new Guid("a8867ce6-7e89-4d4f-a37f-17c3866b2be3"), "media", "192.168.1.25", "./assets/xteve-logo.png", false, "xTeve", 34400, 4, "http://192.168.1.25:34400/web/" },
                    { new Guid("aa394ded-2fb1-46f8-a344-da3776500f7f"), "media", "192.168.1.25", "./assets/radarr-logo.png", false, "Radarr", 7878, 2, "http://192.168.1.25:7878/" },
                    { new Guid("b02eee59-5908-4388-a084-81e1c9e8c57d"), "system", "192.168.1.161", "./assets/grafana-logo.png", false, "Grafana", 3000, 6, "http://192.168.1.161:3000/" },
                    { new Guid("b3b2e79d-fc39-487e-898f-eee7c04d26d9"), "media", "192.168.1.161", "./assets/overseerr-logo.png", false, "Overseerr", 5055, 1, "http://192.168.1.161:5055/" },
                    { new Guid("b53ba90f-4f9c-49d1-b1b4-f6180f384866"), "productivity", "192.168.1.142", "./assets/mattermost-logo.png", false, "Mattermost", 8065, 4, "http://192.168.1.142:8065/" },
                    { new Guid("b6d36cca-8427-40cd-8c3e-b6934f79253d"), "system", "192.168.1.15", "./assets/proxmox-logo.png", true, "Proxmox", 8006, 4, "https://192.168.1.15:8006/" },
                    { new Guid("b96e6d78-13d3-474f-9d3f-367b8a316f45"), "media", "192.168.1.25", "./assets/sonarr-logo.png", false, "Sonarr", 8989, 3, "http://192.168.1.25:8989/" },
                    { new Guid("ddbd1742-9c1d-4ad0-8299-433cf147f202"), "tools", "192.168.1.161", "./assets/portainer-logo.png", true, "Portainer", 9443, 0, "https://192.168.1.161:9443/" },
                    { new Guid("f0faf07d-5c47-4a3b-b3c0-1e7d982fe444"), "tools", "192.168.1.205", "./assets/homebridge-logo.png", false, "Homebridge", 8581, 3, "http://192.168.1.205:8581/" },
                    { new Guid("f90af342-7d97-4cc5-85ae-2ed55c8423f9"), "system", "192.168.1.161", "./assets/influxdb-logo.png", false, "InfluxDB", 8086, 5, "http://192.168.1.161:8086/" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Links");
        }
    }
}
