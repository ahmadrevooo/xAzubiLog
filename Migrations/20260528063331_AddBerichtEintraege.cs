using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace xAzubiLog.Migrations
{
    /// <inheritdoc />
    public partial class AddBerichtEintraege : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BerichtEintraege",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Titel = table.Column<string>(nullable: false),
                    Beschreibung = table.Column<string>(nullable: false),
                    Datum = table.Column<DateTime>(nullable: false),
                    Startzeit = table.Column<DateTime>(nullable: false),
                    Endzeit = table.Column<DateTime>(nullable: false),
                    Tagestyp = table.Column<string>(nullable: false),
                    Status = table.Column<string>(nullable: false),
                    Notiz = table.Column<string>(nullable: false),
                    BenutzerId = table.Column<int>(nullable: false),
                    WochenberichtId = table.Column<int>(nullable: false),
                    AusbilderId = table.Column<int>(nullable: true),
                    KategorieId = table.Column<int>(nullable: true),
                    ErstelltAm = table.Column<DateTime>(nullable: false),
                    GeändertAm = table.Column<DateTime>(nullable: false),
                    Auftragsnummer = table.Column<string>(nullable: true),
                    Dauer = table.Column<decimal>(nullable: true),
                    Fach = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BerichtEintraege", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
