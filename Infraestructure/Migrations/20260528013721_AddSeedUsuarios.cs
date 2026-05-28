using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedUsuarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "HoraInicio",
                table: "Reservas",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "HoraFin",
                table: "Reservas",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "HoraInicio",
                table: "Canchas",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "HoraFin",
                table: "Canchas",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.InsertData(
                table: "Personas",
                columns: new[] { "Id", "Apellido", "CodigoPostal", "ContactoEmergencia", "Direccion", "Dni", "Email", "Administrador_FechaAlta", "FechaNacimiento", "FechaRegistro", "Genero", "Identificador", "Localidad", "Nombre", "Pais", "Provincia", "PuedeFacturar", "Telefono", "TipoPersona" },
                values: new object[,]
                {
                    { 1, "Principal", "1000", "1100000000", "Calle Falsa 123", 11111111, "admin@golahora.com", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Masculino", 100, "CABA", "Admin", "Argentina", "Buenos Aires", true, "1100000001", "Administrador" },
                    { 2, "Staff", "1000", "1100000000", "Avenida Siempreviva 742", 22222222, "personal@golahora.com", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Femenino", 101, "CABA", "Personal", "Argentina", "Buenos Aires", false, "1100000002", "Administrador" }
                });

            migrationBuilder.InsertData(
                table: "Personas",
                columns: new[] { "Id", "Apellido", "AptoFisico", "CodigoPostal", "ContactoEmergencia", "Direccion", "Dni", "Email", "EsSocioActivo", "FechaAlta", "FechaBaja", "FechaNacimiento", "FechaRegistro", "Genero", "Localidad", "Nombre", "ObraSocial", "Pais", "Provincia", "Telefono", "TipoPersona" },
                values: new object[] { 3, "Cliente", true, "1000", "1100000000", "San Martin 456", 33333333, "cliente@golahora.com", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1995, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Masculino", "CABA", "Juan", "OSDE", "Argentina", "Buenos Aires", "1100000003", "Cliente" });

            migrationBuilder.InsertData(
                table: "Personas",
                columns: new[] { "Id", "Apellido", "Certificacion", "CodigoPostal", "ContactoEmergencia", "Direccion", "Dni", "Email", "Especialidad", "FechaNacimiento", "FechaRegistro", "Genero", "Localidad", "Nombre", "Pais", "Provincia", "Telefono", "TipoPersona" },
                values: new object[] { 4, "Profe", "AFA Nivel 2", "1000", "1100000000", "Belgrano 789", 44444444, "profe@golahora.com", "Fútbol 11 y Preparación Física", new DateTime(1985, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Masculino", "CABA", "Carlos", "Argentina", "Buenos Aires", "1100000004", "Profesor" });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Email", "PasswordHash", "PersonaId", "TipoUsuario" },
                values: new object[,]
                {
                    { 1, "admin", "1234", 1, 3 },
                    { 2, "personal", "1234", 2, 3 },
                    { 3, "cliente", "1234", 3, 1 },
                    { 4, "profe", "1234", 4, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Personas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Personas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Personas",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Personas",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.AlterColumn<DateTime>(
                name: "HoraInicio",
                table: "Reservas",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<DateTime>(
                name: "HoraFin",
                table: "Reservas",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<DateTime>(
                name: "HoraInicio",
                table: "Canchas",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<DateTime>(
                name: "HoraFin",
                table: "Canchas",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");
        }
    }
}
