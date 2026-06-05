using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeProfesorIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clases_Personas_ProfesorId",
                table: "Clases");

            migrationBuilder.DropForeignKey(
                name: "FK_Entrenamientos_Personas_ProfesorId",
                table: "Entrenamientos");

            migrationBuilder.AlterColumn<int>(
                name: "ProfesorId",
                table: "Entrenamientos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ProfesorId",
                table: "Clases",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Clases_Personas_ProfesorId",
                table: "Clases",
                column: "ProfesorId",
                principalTable: "Personas",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Entrenamientos_Personas_ProfesorId",
                table: "Entrenamientos",
                column: "ProfesorId",
                principalTable: "Personas",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clases_Personas_ProfesorId",
                table: "Clases");

            migrationBuilder.DropForeignKey(
                name: "FK_Entrenamientos_Personas_ProfesorId",
                table: "Entrenamientos");

            migrationBuilder.AlterColumn<int>(
                name: "ProfesorId",
                table: "Entrenamientos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProfesorId",
                table: "Clases",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Clases_Personas_ProfesorId",
                table: "Clases",
                column: "ProfesorId",
                principalTable: "Personas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Entrenamientos_Personas_ProfesorId",
                table: "Entrenamientos",
                column: "ProfesorId",
                principalTable: "Personas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
