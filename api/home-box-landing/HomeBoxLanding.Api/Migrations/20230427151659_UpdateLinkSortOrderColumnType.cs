using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HomeBoxLanding.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLinkSortOrderColumnType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "SortOrder",
                table: "Links",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.InsertData(
                table: "Links",
                columns: new[] { "Identifier", "Category", "Host", "IconUrl", "IsSecure", "Name", "Port", "SortOrder", "Url" },
                values: new object[,]
                {
                    { new Guid("0bcca72c-cc40-4804-92a0-924e716755aa"), "system", "192.168.1.161", "./assets/tautulli-logo.png", false, "Tautulli", 8181, "S", "http://192.168.1.161:8181/" },
                    { new Guid("0c997358-2c7e-49b2-9746-24001d03a349"), "tools", "192.168.1.254", "./assets/home-assistant-logo.png", false, "Home Assistant", 8123, "Y", "http://192.168.1.254:8123/" },
                    { new Guid("2c1b65ca-e740-4879-917f-50bda0fa6254"), "productivity", "192.168.1.161", "./assets/firefly-iii-logo.jpeg", false, "Firefly III", 8282, "K", "http://192.168.1.161:8282/" },
                    { new Guid("3c9dd86a-08fe-469a-a7c9-29a301dd8bc7"), "system", "192.168.1.1", "./assets/unifi-logo.png", true, "UniFi Controller", 80, "L", "http://192.168.1.1/" },
                    { new Guid("5dad71a9-a5c8-4bd7-a643-c4d42554013a"), "tools", "192.168.1.25", "./assets/flood-logo.png", false, "Flood", 8081, "U", "http://192.168.1.25:8081/" },
                    { new Guid("60dd7d27-e471-4748-a4e5-968093be0f46"), "system", "192.168.1.15", "./assets/proxmox-logo.png", true, "Proxmox", 8006, "P", "https://192.168.1.15:8006/" },
                    { new Guid("6a9eaa9b-027e-4417-8ec5-73235571a57e"), "tools", "192.168.1.205", "./assets/homebridge-logo.png", false, "Homebridge", 8581, "X", "http://192.168.1.205:8581/" },
                    { new Guid("6da3a0b8-ca77-44dd-bfb9-66556f9dc875"), "tools", "192.168.1.161", "./assets/nginx-proxy-manager-logo.png", false, "Nginx Proxy Manager", 81, "V", "http://192.168.1.161:81/" },
                    { new Guid("6f58e028-c5f0-4e62-b8a4-d966444820e5"), "tools", "192.168.1.161", "./assets/portainer-logo.png", true, "Portainer", 9443, "T", "https://192.168.1.161:9443/" },
                    { new Guid("72d955e8-5bbd-4ea4-8cd7-fc8db9ca4add"), "productivity", "192.168.1.142", "./assets/mattermost-logo.png", false, "Mattermost", 8065, "J", "http://192.168.1.142:8065/" },
                    { new Guid("77e45f78-d2f3-48f6-8ea6-dcfd9b461acb"), "productivity", "192.168.1.161", "./assets/planka-logo.png", false, "Planka", 9096, "G", "http://192.168.1.161:9096/" },
                    { new Guid("87e8fca8-0160-4f2a-87aa-dc3b7f5b632e"), "media", "192.168.1.25", "./assets/radarr-logo.png", false, "Radarr", 7878, "C", "http://192.168.1.25:7878/" },
                    { new Guid("9997a82a-04ed-44d4-83a8-b523f472a950"), "media", "192.168.1.74", "./assets/plex-logo.png", false, "Plex", 32400, "A", "http://192.168.1.74:32400/" },
                    { new Guid("9abce3d0-836b-4dfb-b92c-9f2251f33abc"), "tools", "192.168.1.50", "./assets/phoscon-logo.jpeg", false, "Phoscon", 8080, "Z", "http://192.168.1.50:8080/" },
                    { new Guid("a4941178-0918-435a-8b05-994cd610a64f"), "tools", "192.168.1.25", "./assets/tdarr-logo.png", false, "Tdarr", 8265, "ZB", "http://192.168.1.25:8265/" },
                    { new Guid("aa78efae-641d-4a8c-a026-46ada84ebff0"), "system", "192.168.1.48", "./assets/synology-logo.png", false, "Synology Dashboard", 5000, "M", "http://192.168.1.48:5000/" },
                    { new Guid("b71169f0-06eb-4c83-bb46-84ba5e425fe0"), "productivity", "192.168.1.161", "./assets/mealie-logo.png", false, "Mealie", 9091, "F", "http://192.168.1.161:9091/" },
                    { new Guid("b9b1b3de-ecf0-47f9-8ea4-71ed409d8bd2"), "system", "192.168.1.161", "./assets/grafana-logo.png", false, "Grafana", 3000, "R", "http://192.168.1.161:3000/" },
                    { new Guid("bac0adf8-7b47-4e9e-bd16-694db7e77354"), "system", "192.168.1.25", "./assets/prowlarr-logo.png", false, "Prowlarr", 9696, "N", "http://192.168.1.25:9696/" },
                    { new Guid("c2072fea-73a3-449a-8576-666ac1d72047"), "media", "192.168.1.25", "./assets/xteve-logo.png", false, "xTeve", 34400, "E", "http://192.168.1.25:34400/web/" },
                    { new Guid("ddb9464e-5c2a-43b5-b0ef-edf15b03c00d"), "tools", "192.168.1.161", "./assets/uptime-kuma-logo.png", false, "Uptime Kuma", 3001, "ZA", "http://192.168.1.161:3001/" },
                    { new Guid("e0ca234a-4df0-4f19-8b8b-786499dd9a01"), "media", "192.168.1.25", "./assets/sonarr-logo.png", false, "Sonarr", 8989, "D", "http://192.168.1.25:8989/" },
                    { new Guid("e39520d5-a1c3-4ba0-b292-e8b38682e3bd"), "productivity", "192.168.1.161", "./assets/trilium-logo.png", false, "Trilium", 9095, "H", "http://192.168.1.161:9095/" },
                    { new Guid("ea89d062-2d4c-43a8-8291-a4052411da14"), "system", "192.168.1.50", "./assets/pihole-logo.png", false, "PiHole", 80, "O", "http://192.168.1.50/admin/" },
                    { new Guid("f43dcb52-8e93-494c-8823-1f8e8c38f991"), "system", "192.168.1.161", "./assets/influxdb-logo.png", false, "InfluxDB", 8086, "Q", "http://192.168.1.161:8086/" },
                    { new Guid("f63af15c-23bf-485f-a5f5-f68f8ac0af5b"), "media", "192.168.1.161", "./assets/overseerr-logo.png", false, "Overseerr", 5055, "B", "http://192.168.1.161:5055/" },
                    { new Guid("f98a4a5b-b45b-4c7e-9339-7ccec4a2ba11"), "productivity", "192.168.1.35", "./assets/octopi-logo.png", false, "OctoPi", 80, "I", "http://192.168.1.35/" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("0bcca72c-cc40-4804-92a0-924e716755aa"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("0c997358-2c7e-49b2-9746-24001d03a349"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("2c1b65ca-e740-4879-917f-50bda0fa6254"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("3c9dd86a-08fe-469a-a7c9-29a301dd8bc7"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("5dad71a9-a5c8-4bd7-a643-c4d42554013a"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("60dd7d27-e471-4748-a4e5-968093be0f46"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("6a9eaa9b-027e-4417-8ec5-73235571a57e"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("6da3a0b8-ca77-44dd-bfb9-66556f9dc875"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("6f58e028-c5f0-4e62-b8a4-d966444820e5"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("72d955e8-5bbd-4ea4-8cd7-fc8db9ca4add"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("77e45f78-d2f3-48f6-8ea6-dcfd9b461acb"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("87e8fca8-0160-4f2a-87aa-dc3b7f5b632e"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("9997a82a-04ed-44d4-83a8-b523f472a950"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("9abce3d0-836b-4dfb-b92c-9f2251f33abc"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("a4941178-0918-435a-8b05-994cd610a64f"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("aa78efae-641d-4a8c-a026-46ada84ebff0"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("b71169f0-06eb-4c83-bb46-84ba5e425fe0"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("b9b1b3de-ecf0-47f9-8ea4-71ed409d8bd2"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("bac0adf8-7b47-4e9e-bd16-694db7e77354"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("c2072fea-73a3-449a-8576-666ac1d72047"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("ddb9464e-5c2a-43b5-b0ef-edf15b03c00d"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("e0ca234a-4df0-4f19-8b8b-786499dd9a01"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("e39520d5-a1c3-4ba0-b292-e8b38682e3bd"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("ea89d062-2d4c-43a8-8291-a4052411da14"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("f43dcb52-8e93-494c-8823-1f8e8c38f991"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("f63af15c-23bf-485f-a5f5-f68f8ac0af5b"));

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "Identifier",
                keyValue: new Guid("f98a4a5b-b45b-4c7e-9339-7ccec4a2ba11"));

            migrationBuilder.AlterColumn<int>(
                name: "SortOrder",
                table: "Links",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

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
    }
}
