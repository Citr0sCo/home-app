using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeBoxLanding.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddingFoldersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FolderIdentifier",
                table: "Links",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Folders",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Icon = table.Column<string>(type: "TEXT", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    ColumnIdentifier = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folders", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_Folders_Columns_ColumnIdentifier",
                        column: x => x.ColumnIdentifier,
                        principalTable: "Columns",
                        principalColumn: "Identifier",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Links_FolderIdentifier",
                table: "Links",
                column: "FolderIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Folders_ColumnIdentifier",
                table: "Folders",
                column: "ColumnIdentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Links_Folders_FolderIdentifier",
                table: "Links",
                column: "FolderIdentifier",
                principalTable: "Folders",
                principalColumn: "Identifier",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Links_Folders_FolderIdentifier",
                table: "Links");

            migrationBuilder.DropTable(
                name: "Folders");

            migrationBuilder.DropIndex(
                name: "IX_Links_FolderIdentifier",
                table: "Links");

            migrationBuilder.DropColumn(
                name: "FolderIdentifier",
                table: "Links");
        }
    }
}
