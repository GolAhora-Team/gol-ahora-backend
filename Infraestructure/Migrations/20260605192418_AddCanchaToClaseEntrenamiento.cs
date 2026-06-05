using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCanchaToClaseEntrenamiento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DiasSemana",
                table: "Entrenamientos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "HoraFin",
                table: "Entrenamientos",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "HoraInicio",
                table: "Entrenamientos",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<int>(
                name: "CanchaId",
                table: "Clases",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiasSemana",
                table: "Clases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Clases_CanchaId",
                table: "Clases",
                column: "CanchaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clases_Canchas_CanchaId",
                table: "Clases",
                column: "CanchaId",
                principalTable: "Canchas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clases_Canchas_CanchaId",
                table: "Clases");

            migrationBuilder.DropIndex(
                name: "IX_Clases_CanchaId",
                table: "Clases");

            migrationBuilder.DropColumn(
                name: "DiasSemana",
                table: "Entrenamientos");

            migrationBuilder.DropColumn(
                name: "HoraFin",
                table: "Entrenamientos");

            migrationBuilder.DropColumn(
                name: "HoraInicio",
                table: "Entrenamientos");

            migrationBuilder.DropColumn(
                name: "CanchaId",
                table: "Clases");

            migrationBuilder.DropColumn(
                name: "DiasSemana",
                table: "Clases");
        }
    }
}
