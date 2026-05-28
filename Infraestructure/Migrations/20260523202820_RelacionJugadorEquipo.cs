using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class RelacionJugadorEquipo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EquipoId",
                table: "Jugadores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Jugadores_EquipoId",
                table: "Jugadores",
                column: "EquipoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jugadores_Equipos_EquipoId",
                table: "Jugadores",
                column: "EquipoId",
                principalTable: "Equipos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jugadores_Equipos_EquipoId",
                table: "Jugadores");

            migrationBuilder.DropIndex(
                name: "IX_Jugadores_EquipoId",
                table: "Jugadores");

            migrationBuilder.DropColumn(
                name: "EquipoId",
                table: "Jugadores");
        }
    }
}
