using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorCertificados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CertificadoArchivo",
                table: "Personas");

            migrationBuilder.RenameColumn(
                name: "CertificadoFechaFin",
                table: "Personas",
                newName: "CertificadoFechaVencimiento");

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
                columns: new[] { "CertificadoNombreArchivo", "CertificadoUrl" },
                values: new object[] { null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CertificadoNombreArchivo",
                table: "Personas");

            migrationBuilder.DropColumn(
                name: "CertificadoUrl",
                table: "Personas");

            migrationBuilder.RenameColumn(
                name: "CertificadoFechaVencimiento",
                table: "Personas",
                newName: "CertificadoFechaFin");

            migrationBuilder.AddColumn<byte[]>(
                name: "CertificadoArchivo",
                table: "Personas",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Personas",
                keyColumn: "Id",
                keyValue: 4,
                column: "CertificadoArchivo",
                value: null);
        }
    }
}
