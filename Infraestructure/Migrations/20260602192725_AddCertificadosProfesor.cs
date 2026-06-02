using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCertificadosProfesor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "CertificadoArchivo",
                table: "Personas",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CertificadoFechaFin",
                table: "Personas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CertificadoFechaInicio",
                table: "Personas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Personas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CertificadoArchivo", "CertificadoFechaFin", "CertificadoFechaInicio" },
                values: new object[] { null, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CertificadoArchivo",
                table: "Personas");

            migrationBuilder.DropColumn(
                name: "CertificadoFechaFin",
                table: "Personas");

            migrationBuilder.DropColumn(
                name: "CertificadoFechaInicio",
                table: "Personas");
        }
    }
}
