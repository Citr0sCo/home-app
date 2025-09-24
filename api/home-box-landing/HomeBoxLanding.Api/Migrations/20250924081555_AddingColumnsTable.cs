using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeBoxLanding.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddingColumnsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ColumnIdentifier",
                table: "Links",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Columns",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    Icon = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Columns", x => x.Identifier);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Links_ColumnIdentifier",
                table: "Links",
                column: "ColumnIdentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Links_Columns_ColumnIdentifier",
                table: "Links",
                column: "ColumnIdentifier",
                principalTable: "Columns",
                principalColumn: "Identifier",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Links_Columns_ColumnIdentifier",
                table: "Links");

            migrationBuilder.DropTable(
                name: "Columns");

            migrationBuilder.DropIndex(
                name: "IX_Links_ColumnIdentifier",
                table: "Links");

            migrationBuilder.DropColumn(
                name: "ColumnIdentifier",
                table: "Links");
        }
    }
}
