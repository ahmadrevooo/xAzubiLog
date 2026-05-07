using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace xAzubiLog.Migrations
{
    public partial class AddRolleUndResetToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ausbilder",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Abteilung = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ausbilder", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Vorname = table.Column<string>(type: "TEXT", nullable: false),
                    Nachname = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PasswortHash = table.Column<string>(type: "TEXT", nullable: false),
                    Schule = table.Column<string>(type: "TEXT", nullable: false),
                    Klasse = table.Column<string>(type: "TEXT", nullable: false),
                    Ausbildungsberuf = table.Column<string>(type: "TEXT", nullable: false),
                    Aktiv = table.Column<bool>(type: "INTEGER", nullable: false),
                    Rolle = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PasswortResetTokens",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Token = table.Column<string>(type: "TEXT", nullable: false),
                    Ablauf = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Verwendet = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswortResetTokens", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Kategorien",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BenutzerId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    FarbeHex = table.Column<string>(type: "TEXT", nullable: false),
                    Reihenfolge = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategorien", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Kategorien_Users_BenutzerId",
                        column: x => x.BenutzerId,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wochenberichte",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BenutzerId = table.Column<int>(type: "INTEGER", nullable: false),
                    Kalenderwoche = table.Column<int>(type: "INTEGER", nullable: false),
                    Gesamtstunden = table.Column<double>(type: "REAL", nullable: false),
                    Jahr = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    Kommentar = table.Column<string>(type: "TEXT", nullable: false),
                    ErstelltAm = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wochenberichte", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wochenberichte_Users_BenutzerId",
                        column: x => x.BenutzerId,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "PasswortResetTokens");
            migrationBuilder.DropTable(name: "Kategorien");
            migrationBuilder.DropTable(name: "Wochenberichte");
            migrationBuilder.DropTable(name: "Ausbilder");
            migrationBuilder.DropTable(name: "Users");
        }
    }
}