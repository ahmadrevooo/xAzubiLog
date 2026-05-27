using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace xAzubiLog.Migrations
{
    /// <inheritdoc />
    public partial class AddStundenplanTableFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StundenplanEintrag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BenutzerId = table.Column<int>(type: "INTEGER", nullable: false),
                    Wochentag = table.Column<int>(type: "INTEGER", nullable: false),
                    FachName = table.Column<string>(type: "TEXT", nullable: false),
                    DauerMinuten = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StundenplanEintrag", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StundenplanEintrag");
        }
    }
}
