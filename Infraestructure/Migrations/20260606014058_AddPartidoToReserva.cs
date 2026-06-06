using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPartidoToReserva : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ClienteId",
                table: "Reservas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PartidoId",
                table: "Reservas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CanchaId",
                table: "Partidos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_PartidoId",
                table: "Reservas",
                column: "PartidoId");

            migrationBuilder.CreateIndex(
                name: "IX_Partidos_CanchaId",
                table: "Partidos",
                column: "CanchaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Partidos_Canchas_CanchaId",
                table: "Partidos",
                column: "CanchaId",
                principalTable: "Canchas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Partidos_PartidoId",
                table: "Reservas",
                column: "PartidoId",
                principalTable: "Partidos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Partidos_Canchas_CanchaId",
                table: "Partidos");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Partidos_PartidoId",
                table: "Reservas");

            migrationBuilder.DropIndex(
                name: "IX_Reservas_PartidoId",
                table: "Reservas");

            migrationBuilder.DropIndex(
                name: "IX_Partidos_CanchaId",
                table: "Partidos");

            migrationBuilder.DropColumn(
                name: "PartidoId",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "CanchaId",
                table: "Partidos");

            migrationBuilder.AlterColumn<int>(
                name: "ClienteId",
                table: "Reservas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
