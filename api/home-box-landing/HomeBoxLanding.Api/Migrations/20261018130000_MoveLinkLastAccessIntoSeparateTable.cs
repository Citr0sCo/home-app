using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeBoxLanding.Api.Migrations;

public partial class MoveLinkLastAccessIntoSeparateTable : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "LinkLastAccesses",
            columns: table => new
            {
                LinkIdentifier = table.Column<Guid>(type: "TEXT", nullable: false),
                LastAccessedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_LinkLastAccesses", x => x.LinkIdentifier);
            });

        migrationBuilder.Sql("""
            INSERT INTO LinkLastAccesses (LinkIdentifier, LastAccessedAt)
            SELECT Identifier, LastClickedAt
            FROM Links
            WHERE LastClickedAt IS NOT NULL;
            """);

        migrationBuilder.DropColumn(name: "LastClickedAt", table: "Links");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTime>(
            name: "LastClickedAt",
            table: "Links",
            type: "TEXT",
            nullable: true);

        migrationBuilder.Sql("""
            UPDATE Links
            SET LastClickedAt = (
                SELECT LastAccessedAt
                FROM LinkLastAccesses
                WHERE LinkLastAccesses.LinkIdentifier = Links.Identifier
            );
            """);

        migrationBuilder.DropTable(name: "LinkLastAccesses");
    }

}
