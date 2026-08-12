using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeBoxLanding.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddHealthCheckHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HealthCheckHistory",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "TEXT", nullable: false),
                    LinkIdentifier = table.Column<Guid>(type: "TEXT", nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DurationInMilliseconds = table.Column<long>(type: "INTEGER", nullable: false),
                    StatusCode = table.Column<int>(type: "INTEGER", nullable: false),
                    StatusDescription = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthCheckHistory", x => x.Identifier);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HealthCheckHistory_LinkIdentifier",
                table: "HealthCheckHistory",
                column: "LinkIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_HealthCheckHistory_RecordedAt",
                table: "HealthCheckHistory",
                column: "RecordedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HealthCheckHistory");
        }
    }
}
