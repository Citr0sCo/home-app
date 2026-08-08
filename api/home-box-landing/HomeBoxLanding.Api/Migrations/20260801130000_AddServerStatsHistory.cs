using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeBoxLanding.Api.Migrations;

public partial class AddServerStatsHistory : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "ServerStatsHistory",
            columns: table => new
            {
                Identifier = table.Column<Guid>(type: "TEXT", nullable: false),
                RecordedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                CpuPercentage = table.Column<double>(type: "REAL", nullable: false),
                MemoryPercentage = table.Column<double>(type: "REAL", nullable: false),
                MemoryUsed = table.Column<double>(type: "REAL", nullable: false),
                MemoryTotal = table.Column<double>(type: "REAL", nullable: false),
                DiskPercentage = table.Column<double>(type: "REAL", nullable: false),
                DiskUsed = table.Column<double>(type: "REAL", nullable: false),
                DiskTotal = table.Column<double>(type: "REAL", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ServerStatsHistory", x => x.Identifier);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ServerStatsHistory_RecordedAt",
            table: "ServerStatsHistory",
            column: "RecordedAt");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ServerStatsHistory");
    }
}
