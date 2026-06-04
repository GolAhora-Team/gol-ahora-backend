using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCertificadosProfesoresTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CertificadoFechaInicio",
                table: "Personas");

            migrationBuilder.DropColumn(
                name: "CertificadoFechaVencimiento",
                table: "Personas");

            migrationBuilder.DropColumn(
                name: "CertificadoNombreArchivo",
                table: "Personas");

            migrationBuilder.DropColumn(
                name: "CertificadoUrl",
                table: "Personas");

            migrationBuilder.CreateTable(
                name: "CertificadosProfesores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfesorId = table.Column<int>(type: "int", nullable: false),
                    CertificadoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CertificadoNombreArchivo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaVencimiento = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificadosProfesores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CertificadosProfesores_Personas_ProfesorId",
                        column: x => x.ProfesorId,
                        principalTable: "Personas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CertificadosProfesores_ProfesorId",
                table: "CertificadosProfesores",
                column: "ProfesorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CertificadosProfesores");

            migrationBuilder.AddColumn<DateTime>(
                name: "CertificadoFechaInicio",
                table: "Personas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CertificadoFechaVencimiento",
                table: "Personas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CertificadoNombreArchivo",
                table: "Personas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CertificadoUrl",
                table: "Personas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Personas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CertificadoFechaInicio", "CertificadoFechaVencimiento", "CertificadoNombreArchivo", "CertificadoUrl" },
                values: new object[] { null, null, null, null });
        }
    }
}
