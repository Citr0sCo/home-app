using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeBoxLanding.Api.Migrations
{
    /// <inheritdoc />
    public partial class CreateDeploysTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Deploys",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "uuid", nullable: false),
                    CommitId = table.Column<string>(type: "text", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FinishedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deploys", x => x.Identifier);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deploys");
        }
    }
}
