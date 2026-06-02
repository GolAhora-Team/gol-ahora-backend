using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAptoMedicoAClientes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "AptoMedicoArchivo",
                table: "Personas",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AptoMedicoFechaFin",
                table: "Personas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AptoMedicoFechaInicio",
                table: "Personas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Personas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AptoMedicoArchivo", "AptoMedicoFechaFin", "AptoMedicoFechaInicio" },
                values: new object[] { null, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AptoMedicoArchivo",
                table: "Personas");

            migrationBuilder.DropColumn(
                name: "AptoMedicoFechaFin",
                table: "Personas");

            migrationBuilder.DropColumn(
                name: "AptoMedicoFechaInicio",
                table: "Personas");
        }
    }
}
