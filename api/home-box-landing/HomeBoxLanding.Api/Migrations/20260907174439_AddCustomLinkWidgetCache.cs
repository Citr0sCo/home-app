using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeBoxLanding.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomLinkWidgetCache : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WidgetCache",
                columns: table => new
                {
                    LinkIdentifier = table.Column<Guid>(type: "TEXT", nullable: false),
                    WidgetType = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WidgetCache", x => new { x.LinkIdentifier, x.WidgetType });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WidgetCache");
        }
    }
}
