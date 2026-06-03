using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificacionesIndividuales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notificaciones_Usuarios_UsuarioDestinoId",
                table: "Notificaciones");

            migrationBuilder.DropColumn(
                name: "RolesDestino",
                table: "Notificaciones");

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioDestinoId",
                table: "Notificaciones",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Leida",
                table: "Notificaciones",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Notificaciones_Usuarios_UsuarioDestinoId",
                table: "Notificaciones",
                column: "UsuarioDestinoId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notificaciones_Usuarios_UsuarioDestinoId",
                table: "Notificaciones");

            migrationBuilder.DropColumn(
                name: "Leida",
                table: "Notificaciones");

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioDestinoId",
                table: "Notificaciones",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "RolesDestino",
                table: "Notificaciones",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Notificaciones_Usuarios_UsuarioDestinoId",
                table: "Notificaciones",
                column: "UsuarioDestinoId",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }
    }
}
