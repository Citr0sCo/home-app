using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HomeBoxLanding.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddGithubBuildReferenceToBuildsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AlterColumn<DateTime>(
                name: "FinishedAt",
                table: "Builds",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "GithubBuildReference",
                table: "Builds",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Links",
                columns: new[] { "Identifier", "Category", "Host", "IconUrl", "IsSecure", "Name", "Port", "SortOrder", "Url" },
                values: new object[,]
                {
                    { new Guid("080aa5c4-b63c-4f3c-8767-72e4775a7034"), "system", "192.168.1.25", "./assets/prowlarr-logo.png", false, "Prowlarr", 9696, 2, "http://192.168.1.25:9696/" },
                    { new Guid("12e5579b-9f42-45a6-88d1-0e2c82c935c2"), "system", "192.168.1.50", "./assets/pihole-logo.png", false, "PiHole", 80, 3, "http://192.168.1.50/admin/" },
                    { new Guid("39cad5ec-eea5-4114-b67c-e5cfbd7037be"), "tools", "192.168.1.205", "./assets/homebridge-logo.png", false, "Homebridge", 8581, 3, "http://192.168.1.205:8581/" },
                    { new Guid("413560d9-c7f7-4851-9957-433e6470b963"), "system", "192.168.1.161", "./assets/tautulli-logo.png", false, "Tautulli", 8181, 7, "http://192.168.1.161:8181/" },
                    { new Guid("4c4243e7-e116-4d0d-a8fb-26249576ea57"), "productivity", "192.168.1.161", "./assets/mealie-logo.png", false, "Mealie", 9091, 0, "http://192.168.1.161:9091/" },
                    { new Guid("4e234356-b63d-4fa1-a4d9-3324ab9d027c"), "productivity", "192.168.1.142", "./assets/mattermost-logo.png", false, "Mattermost", 8065, 4, "http://192.168.1.142:8065/" },
                    { new Guid("5015a5c8-3c68-4c67-bf8e-a2caa89da4f0"), "tools", "192.168.1.254", "./assets/home-assistant-logo.png", false, "Home Assistant", 8123, 4, "http://192.168.1.254:8123/" },
                    { new Guid("5f9a9237-5d65-4daf-81ed-9b4ebd50c73c"), "productivity", "192.168.1.161", "./assets/planka-logo.png", false, "Planka", 9096, 1, "http://192.168.1.161:9096/" },
                    { new Guid("6930ae90-d6f8-40b9-86a0-740e305ff411"), "media", "192.168.1.161", "./assets/overseerr-logo.png", false, "Overseerr", 5055, 1, "http://192.168.1.161:5055/" },
                    { new Guid("70607e71-c3f4-4356-b90d-2a71c0e0f0fb"), "system", "192.168.1.15", "./assets/proxmox-logo.png", true, "Proxmox", 8006, 4, "https://192.168.1.15:8006/" },
                    { new Guid("81628985-ffc6-4742-b2af-9f83c86ce490"), "tools", "192.168.1.161", "./assets/nginx-proxy-manager-logo.png", false, "Nginx Proxy Manager", 81, 2, "http://192.168.1.161:81/" },
                    { new Guid("81fd41dd-f3d1-4079-aff8-fa855effd88f"), "tools", "192.168.1.50", "./assets/phoscon-logo.jpeg", false, "Phoscon", 8080, 5, "http://192.168.1.50:8080/" },
                    { new Guid("939a9a50-b89f-4d39-bea2-a8bc8655466d"), "tools", "192.168.1.161", "./assets/portainer-logo.png", true, "Portainer", 9443, 0, "https://192.168.1.161:9443/" },
                    { new Guid("99b05a22-777c-43f2-b8fd-f96f3baa9c69"), "media", "192.168.1.25", "./assets/radarr-logo.png", false, "Radarr", 7878, 2, "http://192.168.1.25:7878/" },
                    { new Guid("9ad28ba4-a3ae-4d61-a4d4-cd4316e1813b"), "productivity", "192.168.1.35", "./assets/octopi-logo.png", false, "OctoPi", 80, 3, "http://192.168.1.35/" },
                    { new Guid("a75ee54b-81d2-4878-9e8e-1c5cdacca44b"), "tools", "192.168.1.161", "./assets/uptime-kuma-logo.png", false, "Uptime Kuma", 3001, 6, "http://192.168.1.161:3001/" },
                    { new Guid("b3d8bf34-a31d-403b-a16d-05b8d5bf4532"), "system", "192.168.1.1", "./assets/unifi-logo.png", true, "UniFi Controller", 80, 0, "http://192.168.1.1/" },
                    { new Guid("bfc616da-0a40-4843-a984-c9f28e8dc32c"), "tools", "192.168.1.25", "./assets/flood-logo.png", false, "Flood", 8081, 1, "http://192.168.1.25:8081/" },
                    { new Guid("ce1924d0-b558-4766-a0d0-6fa4eca5e383"), "tools", "192.168.1.25", "./assets/tdarr-logo.png", false, "Tdarr", 8265, 7, "http://192.168.1.25:8265/" },
                    { new Guid("d31d5666-1ff8-415e-a613-51f71ed67512"), "system", "192.168.1.48", "./assets/synology-logo.png", false, "Synology Dashboard", 5000, 1, "http://192.168.1.48:5000/" },
                    { new Guid("d53f711d-a744-4b58-8c91-9eecddab190a"), "productivity", "192.168.1.161", "./assets/firefly-iii-logo.jpeg", false, "Firefly III", 8282, 5, "http://192.168.1.161:8282/" },
                    { new Guid("d6d21105-2e34-448a-b925-ffb954aabe91"), "system", "192.168.1.161", "./assets/grafana-logo.png", false, "Grafana", 3000, 6, "http://192.168.1.161:3000/" },
                    { new Guid("dd8d6898-42e8-499a-8c9f-33e15ef01ca0"), "productivity", "192.168.1.161", "./assets/trilium-logo.png", false, "Trilium", 9095, 2, "http://192.168.1.161:9095/" },
                    { new Guid("e559abdc-c8b9-460f-be48-87f8e77c24dd"), "media", "192.168.1.74", "./assets/plex-logo.png", false, "Plex", 32400, 0, "http://192.168.1.74:32400/" },
                    { new Guid("e96026d8-e698-4a07-8a38-9ba98ae73403"), "media", "192.168.1.25", "./assets/xteve-logo.png", false, "xTeve", 34400, 4, "http://192.168.1.25:34400/web/" },
                    { new Guid("ed29ec31-4e0b-4ce2-9dcb-8b5e440b360d"), "system", "192.168.1.161", "./assets/influxdb-logo.png", false, "InfluxDB", 8086, 5, "http://192.168.1.161:8086/" },
                    { new Guid("f91ff8be-d354-469f-a6a5-fccfaa640cb3"), "media", "192.168.1.25", "./assets/sonarr-logo.png", false, "Sonarr", 8989, 3, "http://192.168.1.25:8989/" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("080aa5c4-b63c-4f3c-8767-72e4775a7034"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("12e5579b-9f42-45a6-88d1-0e2c82c935c2"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("39cad5ec-eea5-4114-b67c-e5cfbd7037be"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("413560d9-c7f7-4851-9957-433e6470b963"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("4c4243e7-e116-4d0d-a8fb-26249576ea57"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("4e234356-b63d-4fa1-a4d9-3324ab9d027c"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("5015a5c8-3c68-4c67-bf8e-a2caa89da4f0"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("5f9a9237-5d65-4daf-81ed-9b4ebd50c73c"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("6930ae90-d6f8-40b9-86a0-740e305ff411"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("70607e71-c3f4-4356-b90d-2a71c0e0f0fb"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("81628985-ffc6-4742-b2af-9f83c86ce490"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("81fd41dd-f3d1-4079-aff8-fa855effd88f"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("939a9a50-b89f-4d39-bea2-a8bc8655466d"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("99b05a22-777c-43f2-b8fd-f96f3baa9c69"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("9ad28ba4-a3ae-4d61-a4d4-cd4316e1813b"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("a75ee54b-81d2-4878-9e8e-1c5cdacca44b"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("b3d8bf34-a31d-403b-a16d-05b8d5bf4532"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("bfc616da-0a40-4843-a984-c9f28e8dc32c"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("ce1924d0-b558-4766-a0d0-6fa4eca5e383"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("d31d5666-1ff8-415e-a613-51f71ed67512"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("d53f711d-a744-4b58-8c91-9eecddab190a"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("d6d21105-2e34-448a-b925-ffb954aabe91"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("dd8d6898-42e8-499a-8c9f-33e15ef01ca0"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("e559abdc-c8b9-460f-be48-87f8e77c24dd"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("e96026d8-e698-4a07-8a38-9ba98ae73403"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("ed29ec31-4e0b-4ce2-9dcb-8b5e440b360d"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("f91ff8be-d354-469f-a6a5-fccfaa640cb3"));

            migrationBuilder.DropColumn(
                name: "GithubBuildReference",
                table: "Builds");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FinishedAt",
                table: "Builds",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

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
    }
}
