using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class SupportMultipleFormations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EsCapitan",
                table: "Jugadores");

            migrationBuilder.DropColumn(
                name: "EsTitular",
                table: "Jugadores");

            migrationBuilder.DropColumn(
                name: "Posicion",
                table: "Jugadores");

            migrationBuilder.DropColumn(
                name: "FormacionDefecto",
                table: "Equipos");

            migrationBuilder.DropColumn(
                name: "TipoCancha",
                table: "Equipos");

            migrationBuilder.CreateTable(
                name: "EquipoFormaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipoId = table.Column<int>(type: "int", nullable: false),
                    TipoCancha = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormacionDefecto = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipoFormaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipoFormaciones_Equipos_EquipoId",
                        column: x => x.EquipoId,
                        principalTable: "Equipos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JugadorFormaciones",
                columns: table => new
                {
                    FormacionId = table.Column<int>(type: "int", nullable: false),
                    JugadorId = table.Column<int>(type: "int", nullable: false),
                    EsTitular = table.Column<bool>(type: "bit", nullable: false),
                    Posicion = table.Column<int>(type: "int", nullable: false),
                    EsCapitan = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JugadorFormaciones", x => new { x.FormacionId, x.JugadorId });
                    table.ForeignKey(
                        name: "FK_JugadorFormaciones_EquipoFormaciones_FormacionId",
                        column: x => x.FormacionId,
                        principalTable: "EquipoFormaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JugadorFormaciones_Jugadores_JugadorId",
                        column: x => x.JugadorId,
                        principalTable: "Jugadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipoFormaciones_EquipoId",
                table: "EquipoFormaciones",
                column: "EquipoId");

            migrationBuilder.CreateIndex(
                name: "IX_JugadorFormaciones_JugadorId",
                table: "JugadorFormaciones",
                column: "JugadorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JugadorFormaciones");

            migrationBuilder.DropTable(
                name: "EquipoFormaciones");

            migrationBuilder.AddColumn<bool>(
                name: "EsCapitan",
                table: "Jugadores",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EsTitular",
                table: "Jugadores",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Posicion",
                table: "Jugadores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FormacionDefecto",
                table: "Equipos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipoCancha",
                table: "Equipos",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
