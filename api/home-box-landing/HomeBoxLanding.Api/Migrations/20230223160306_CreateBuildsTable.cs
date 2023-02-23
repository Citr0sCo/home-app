using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HomeBoxLanding.Api.Migrations
{
    /// <inheritdoc />
    public partial class CreateBuildsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("1d6c91f4-c00a-4b3f-ba54-abf54034f8af"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("20622552-2f72-4b9b-8f95-7eca979bbe9d"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("24b0fa51-101a-4858-898c-3f4f23844fd6"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("24f6048f-3727-4e89-acda-f76355306b42"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("26dcb458-6b5e-4a5d-98c7-62c2a0e443f3"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("2f46896f-445d-41df-9b57-f6ebe5c8dceb"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("590ffe1a-5dd9-441b-81b7-5abdf4eb18fc"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("5926fbfa-b344-4a51-8f62-db40f2cce704"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("5d555bdd-d4b3-4b23-be78-e7ecbf25c9c0"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("60585a7e-5c00-4854-ae1e-31ff8e13d75f"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("70e2f925-3b89-403f-8f8d-cbd50c6ad828"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("7f0074b0-f382-4351-a0de-00182c8bccc2"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("85b9a3bd-cc19-425f-9d94-db06e1674e87"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("a037667b-23cb-4bcb-ae4d-c2cd0db99fa3"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("a0495abb-58e9-41a4-b8db-23466f8dac95"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("a2b5b22a-972b-4fc0-9057-3426c492e35b"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("a75321f8-c057-428f-917f-e933083c86cf"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("a8867ce6-7e89-4d4f-a37f-17c3866b2be3"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("aa394ded-2fb1-46f8-a344-da3776500f7f"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("b02eee59-5908-4388-a084-81e1c9e8c57d"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("b3b2e79d-fc39-487e-898f-eee7c04d26d9"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("b53ba90f-4f9c-49d1-b1b4-f6180f384866"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("b6d36cca-8427-40cd-8c3e-b6934f79253d"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("b96e6d78-13d3-474f-9d3f-367b8a316f45"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("ddbd1742-9c1d-4ad0-8299-433cf147f202"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("f0faf07d-5c47-4a3b-b3c0-1e7d982fe444"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("f90af342-7d97-4cc5-85ae-2ed55c8423f9"));

            migrationBuilder.CreateTable(
                name: "Builds",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Conclusion = table.Column<int>(type: "integer", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FinishedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Builds", x => x.Identifier);
                });

            migrationBuilder.InsertData(
                table: "Links",
                columns: new[] { "Identifier", "Category", "Host", "IconUrl", "IsSecure", "Name", "Port", "SortOrder", "Url" },
                values: new object[,]
                {
                    { new Guid("0008c0fa-edcc-4db0-b6b8-74a8ce4835f5"), "system", "192.168.1.48", "./assets/synology-logo.png", false, "Synology Dashboard", 5000, 1, "http://192.168.1.48:5000/" },
                    { new Guid("08116dd9-4fd2-4175-a021-ff149823fb4d"), "tools", "192.168.1.25", "./assets/flood-logo.png", false, "Flood", 8081, 1, "http://192.168.1.25:8081/" },
                    { new Guid("231cd4ff-8f98-4cb3-92ff-d8db8b0f3251"), "tools", "192.168.1.205", "./assets/homebridge-logo.png", false, "Homebridge", 8581, 3, "http://192.168.1.205:8581/" },
                    { new Guid("27c9ec70-f82d-4883-bcee-c55139979ce9"), "productivity", "192.168.1.161", "./assets/planka-logo.png", false, "Planka", 9096, 1, "http://192.168.1.161:9096/" },
                    { new Guid("2ff48161-923a-471c-b6ea-e2a6a905d7ae"), "tools", "192.168.1.161", "./assets/uptime-kuma-logo.png", false, "Uptime Kuma", 3001, 6, "http://192.168.1.161:3001/" },
                    { new Guid("4be021d3-f975-45ca-9550-a19421ea319e"), "tools", "192.168.1.254", "./assets/home-assistant-logo.png", false, "Home Assistant", 8123, 4, "http://192.168.1.254:8123/" },
                    { new Guid("4c50f877-d402-4e2f-9d05-4e5689549c53"), "productivity", "192.168.1.161", "./assets/firefly-iii-logo.jpeg", false, "Firefly III", 8282, 5, "http://192.168.1.161:8282/" },
                    { new Guid("4c6ffa5d-dab8-4511-8c9b-edb3f1c6ffe7"), "tools", "192.168.1.50", "./assets/phoscon-logo.jpeg", false, "Phoscon", 8080, 5, "http://192.168.1.50:8080/" },
                    { new Guid("55ec84c8-98cd-4e9d-a5c5-83366a2c1a79"), "tools", "192.168.1.161", "./assets/nginx-proxy-manager-logo.png", false, "Nginx Proxy Manager", 81, 2, "http://192.168.1.161:81/" },
                    { new Guid("5a5b4fa9-f89f-4b20-a397-d3d7cb90f8cd"), "system", "192.168.1.15", "./assets/proxmox-logo.png", true, "Proxmox", 8006, 4, "https://192.168.1.15:8006/" },
                    { new Guid("64064be1-0eef-40ec-9497-8ba56bf38264"), "productivity", "192.168.1.161", "./assets/mealie-logo.png", false, "Mealie", 9091, 0, "http://192.168.1.161:9091/" },
                    { new Guid("74c61859-0b88-4557-ba82-0b8f865de4af"), "system", "192.168.1.50", "./assets/pihole-logo.png", false, "PiHole", 80, 3, "http://192.168.1.50/admin/" },
                    { new Guid("75bd3e1b-b0d7-46e6-a124-9e7207efffcc"), "media", "192.168.1.25", "./assets/sonarr-logo.png", false, "Sonarr", 8989, 3, "http://192.168.1.25:8989/" },
                    { new Guid("7f2864c8-6554-4e5f-94c0-b4f15e874ad0"), "system", "192.168.1.161", "./assets/tautulli-logo.png", false, "Tautulli", 8181, 7, "http://192.168.1.161:8181/" },
                    { new Guid("8cd5bc38-b560-47e8-97f3-6a4cfb86cf32"), "system", "192.168.1.25", "./assets/prowlarr-logo.png", false, "Prowlarr", 9696, 2, "http://192.168.1.25:9696/" },
                    { new Guid("8cf92cb1-a1e7-4e69-b6e8-6a25c1240eee"), "productivity", "192.168.1.35", "./assets/octopi-logo.png", false, "OctoPi", 80, 3, "http://192.168.1.35/" },
                    { new Guid("916f1b85-2814-4c4b-88d9-a434b9419941"), "system", "192.168.1.161", "./assets/influxdb-logo.png", false, "InfluxDB", 8086, 5, "http://192.168.1.161:8086/" },
                    { new Guid("97dcc1be-3444-4bc1-9e8d-6db637c88dc7"), "productivity", "192.168.1.142", "./assets/mattermost-logo.png", false, "Mattermost", 8065, 4, "http://192.168.1.142:8065/" },
                    { new Guid("a294ab65-c4b1-4c5d-b1e4-ef3b6d4e39f3"), "system", "192.168.1.161", "./assets/grafana-logo.png", false, "Grafana", 3000, 6, "http://192.168.1.161:3000/" },
                    { new Guid("a493f949-3564-4aa4-88ad-ecd58e7e893b"), "tools", "192.168.1.25", "./assets/tdarr-logo.png", false, "Tdarr", 8265, 7, "http://192.168.1.25:8265/" },
                    { new Guid("aa9196f0-a0b4-44f4-a42d-dce5e96f82d5"), "media", "192.168.1.74", "./assets/plex-logo.png", false, "Plex", 32400, 0, "http://192.168.1.74:32400/" },
                    { new Guid("ab5f271c-9ca8-4fae-bce7-171f46820dad"), "system", "192.168.1.1", "./assets/unifi-logo.png", true, "UniFi Controller", 80, 0, "http://192.168.1.1/" },
                    { new Guid("d7f2daa1-5e6f-4578-8428-36a668434d95"), "media", "192.168.1.25", "./assets/xteve-logo.png", false, "xTeve", 34400, 4, "http://192.168.1.25:34400/web/" },
                    { new Guid("dba56b8f-d320-4a75-be13-58c0cb2f5fd4"), "media", "192.168.1.25", "./assets/radarr-logo.png", false, "Radarr", 7878, 2, "http://192.168.1.25:7878/" },
                    { new Guid("e36b711e-5e00-4dfe-a066-ef835d7890d7"), "tools", "192.168.1.161", "./assets/portainer-logo.png", true, "Portainer", 9443, 0, "https://192.168.1.161:9443/" },
                    { new Guid("f2a0022b-9cca-4477-a681-71df29391d4a"), "media", "192.168.1.161", "./assets/overseerr-logo.png", false, "Overseerr", 5055, 1, "http://192.168.1.161:5055/" },
                    { new Guid("fa1bd65e-0868-47b5-9929-8f2876ff2b8f"), "productivity", "192.168.1.161", "./assets/trilium-logo.png", false, "Trilium", 9095, 2, "http://192.168.1.161:9095/" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Builds");

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("0008c0fa-edcc-4db0-b6b8-74a8ce4835f5"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("08116dd9-4fd2-4175-a021-ff149823fb4d"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("231cd4ff-8f98-4cb3-92ff-d8db8b0f3251"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("27c9ec70-f82d-4883-bcee-c55139979ce9"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("2ff48161-923a-471c-b6ea-e2a6a905d7ae"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("4be021d3-f975-45ca-9550-a19421ea319e"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("4c50f877-d402-4e2f-9d05-4e5689549c53"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("4c6ffa5d-dab8-4511-8c9b-edb3f1c6ffe7"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("55ec84c8-98cd-4e9d-a5c5-83366a2c1a79"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("5a5b4fa9-f89f-4b20-a397-d3d7cb90f8cd"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("64064be1-0eef-40ec-9497-8ba56bf38264"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("74c61859-0b88-4557-ba82-0b8f865de4af"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("75bd3e1b-b0d7-46e6-a124-9e7207efffcc"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("7f2864c8-6554-4e5f-94c0-b4f15e874ad0"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("8cd5bc38-b560-47e8-97f3-6a4cfb86cf32"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("8cf92cb1-a1e7-4e69-b6e8-6a25c1240eee"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("916f1b85-2814-4c4b-88d9-a434b9419941"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("97dcc1be-3444-4bc1-9e8d-6db637c88dc7"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("a294ab65-c4b1-4c5d-b1e4-ef3b6d4e39f3"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("a493f949-3564-4aa4-88ad-ecd58e7e893b"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("aa9196f0-a0b4-44f4-a42d-dce5e96f82d5"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("ab5f271c-9ca8-4fae-bce7-171f46820dad"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("d7f2daa1-5e6f-4578-8428-36a668434d95"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("dba56b8f-d320-4a75-be13-58c0cb2f5fd4"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("e36b711e-5e00-4dfe-a066-ef835d7890d7"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("f2a0022b-9cca-4477-a681-71df29391d4a"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("fa1bd65e-0868-47b5-9929-8f2876ff2b8f"));

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
    }
}
