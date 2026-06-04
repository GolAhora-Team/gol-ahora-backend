using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCodigoBarrasYAsistenciaEntrenamiento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodigoBarras",
                table: "ClienteEntrenamientos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodigoBarras",
                table: "Asistencias",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaHoraRegistro",
                table: "Asistencias",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetodoRegistro",
                table: "Asistencias",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AsistenciaEntrenamientos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Presente = table.Column<bool>(type: "bit", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CodigoBarras = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaHoraRegistro = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MetodoRegistro = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntrenamientoId = table.Column<int>(type: "int", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsistenciaEntrenamientos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AsistenciaEntrenamientos_Entrenamientos_EntrenamientoId",
                        column: x => x.EntrenamientoId,
                        principalTable: "Entrenamientos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AsistenciaEntrenamientos_Personas_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Personas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AsistenciaEntrenamientos_ClienteId",
                table: "AsistenciaEntrenamientos",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_AsistenciaEntrenamientos_EntrenamientoId",
                table: "AsistenciaEntrenamientos",
                column: "EntrenamientoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AsistenciaEntrenamientos");

            migrationBuilder.DropColumn(
                name: "CodigoBarras",
                table: "ClienteEntrenamientos");

            migrationBuilder.DropColumn(
                name: "CodigoBarras",
                table: "Asistencias");

            migrationBuilder.DropColumn(
                name: "FechaHoraRegistro",
                table: "Asistencias");

            migrationBuilder.DropColumn(
                name: "MetodoRegistro",
                table: "Asistencias");
        }
    }
}
