using Aplication.DTOs;
using Aplication.DTOs.Request.Admin;
using Aplication.DTOs.Response.Admin;
using Aplication.Interfaces.IAdmin;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Mappers
{
    public class AdminMapper : IAdminMapper
    {
        public Administrador CreateAdminToRequest(CreateAdminRequest request)
        {
            if (request == null) return null;
            return new Administrador
            {
                Identificador = request.Identificador,
                PuedeFacturar = request.PuedeFacturar,
                Dni = request.Dni,
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Genero = request.Genero,
                FechaNacimiento = request.FechaNacimiento,
                Telefono = request.Telefono,
                Direccion = request.Direccion,
                Localidad = request.Localidad,
                CodigoPostal = request.CodigoPostal,
                Provincia = request.Provincia,
                Pais = request.Pais,
                ContactoEmergencia = request.ContactoEmergencia,
                Email = request.Email,
                FechaRegistro = DateTime.UtcNow,
                FechaAlta = DateTime.UtcNow
            };
        }
        public AdministradorResponse CreateAdministradorResponse(Administrador admin)
        {
            if (admin == null) return null;

            return new AdministradorResponse
            {
                Id = admin.Id,
                Identificador = admin.Identificador,
                FechaAlta = admin.FechaAlta,
                PuedeFacturar = admin.PuedeFacturar,
                Dni = admin.Dni,
                Nombre = admin.Nombre,
                Apellido = admin.Apellido,
                Genero = admin.Genero,
                FechaNacimiento = admin.FechaNacimiento,
                Telefono = admin.Telefono,
                Direccion = admin.Direccion,
                Localidad = admin.Localidad,
                CodigoPostal = admin.CodigoPostal,
                Provincia = admin.Provincia,
                Pais = admin.Pais,
                ContactoEmergencia = admin.ContactoEmergencia,
                Email = admin.Email,
                FechaRegistro = admin.FechaRegistro
            };
        }

        public AdministradorResponse CreateAdministradorResponseFromDto(AdministradorDto dto)
        {
            if (dto == null) return null;

            return new AdministradorResponse
            {
                Id = dto.Id,
                Identificador = dto.Identificador,
                FechaAlta = dto.FechaAlta,
                PuedeFacturar = dto.PuedeFacturar,
                Dni = dto.Dni,
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Genero = dto.Genero,
                FechaNacimiento = dto.FechaNacimiento,
                Telefono = dto.Telefono,
                Direccion = dto.Direccion,
                Localidad = dto.Localidad,
                CodigoPostal = dto.CodigoPostal,
                Provincia = dto.Provincia,
                Pais = dto.Pais,
                ContactoEmergencia = dto.ContactoEmergencia,
                Email = dto.Email,
                FechaRegistro = dto.FechaRegistro
            };
        }
    }
}
