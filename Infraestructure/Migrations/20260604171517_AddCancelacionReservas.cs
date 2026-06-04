using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCancelacionReservas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FacturaId",
                table: "Reservas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AccionRequerida",
                table: "Notificaciones",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "EquipoId",
                table: "Notificaciones",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EstadoAccion",
                table: "Notificaciones",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InvitadoPorUsuarioId",
                table: "Notificaciones",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Concepto",
                table: "Facturas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Facturas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreadoPorClienteId",
                table: "Equipos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PrecioInscripcion",
                table: "Competiciones",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "ConfiguracionCancelaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HorasAntelacionMinima = table.Column<int>(type: "int", nullable: false),
                    PorcentajePenalizacion = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracionCancelaciones", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ConfiguracionCancelaciones",
                columns: new[] { "Id", "HorasAntelacionMinima", "PorcentajePenalizacion" },
                values: new object[] { 1, 24, 50m });

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_FacturaId",
                table: "Reservas",
                column: "FacturaId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipos_CreadoPorClienteId",
                table: "Equipos",
                column: "CreadoPorClienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipos_Personas_CreadoPorClienteId",
                table: "Equipos",
                column: "CreadoPorClienteId",
                principalTable: "Personas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Facturas_FacturaId",
                table: "Reservas",
                column: "FacturaId",
                principalTable: "Facturas",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipos_Personas_CreadoPorClienteId",
                table: "Equipos");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Facturas_FacturaId",
                table: "Reservas");

            migrationBuilder.DropTable(
                name: "ConfiguracionCancelaciones");

            migrationBuilder.DropIndex(
                name: "IX_Reservas_FacturaId",
                table: "Reservas");

            migrationBuilder.DropIndex(
                name: "IX_Equipos_CreadoPorClienteId",
                table: "Equipos");

            migrationBuilder.DropColumn(
                name: "FacturaId",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "AccionRequerida",
                table: "Notificaciones");

            migrationBuilder.DropColumn(
                name: "EquipoId",
                table: "Notificaciones");

            migrationBuilder.DropColumn(
                name: "EstadoAccion",
                table: "Notificaciones");

            migrationBuilder.DropColumn(
                name: "InvitadoPorUsuarioId",
                table: "Notificaciones");

            migrationBuilder.DropColumn(
                name: "Concepto",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "CreadoPorClienteId",
                table: "Equipos");

            migrationBuilder.DropColumn(
                name: "PrecioInscripcion",
                table: "Competiciones");
        }
    }
}
