using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplication.DTOs;
using Aplication.DTOs.Request.Admin;
using Aplication.DTOs.Response.Admin;
using Aplication.Interfaces.IAdmin;
using Domain.Entities;
using Aplication.Interfaces.IAdmin;

namespace Infraestructure.Command
{
    public class AdministradorCommand : IAdminCommand
    {
        private readonly AppDbContext _context;
        private readonly IAdminMapper _mapper;

        public AdministradorCommand(AppDbContext context, IAdminMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AdministradorDto> CreateAsync(Administrador dto)
        {
            _context.Administradores.Add(dto);
            await _context.SaveChangesAsync();
            return new AdministradorDto
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

        public async Task<bool> DeleteAsync(int id)
        {
            var admin = await _context.Administradores.FindAsync(id);
            if (admin == null) return false;

            _context.Administradores.Remove(admin);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<AdministradorDto?> UpdateAsync(int id, AdministradorDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<AdministradorResponse?> UpdateSimpleAsync(int id, UpdateAdministradorSimpleRequest request)
        {
            var admin = await _context.Administradores.FindAsync(id);
            if (admin == null) return null;

            admin.Nombre = string.IsNullOrEmpty(request.Nombre) ? admin.Nombre : request.Nombre;
            admin.Apellido = string.IsNullOrEmpty(request.Apellido) ? admin.Apellido : request.Apellido;
            admin.Dni = request.Dni > 0 ? request.Dni : admin.Dni;
            admin.Genero = string.IsNullOrEmpty(request.Genero) ? admin.Genero : request.Genero;
            admin.FechaNacimiento = request.FechaNacimiento != default ? request.FechaNacimiento : admin.FechaNacimiento;
            admin.Telefono = request.Telefono;
            admin.Direccion = request.Direccion;
            admin.Localidad = request.Localidad;
            admin.CodigoPostal = request.CodigoPostal;
            admin.Provincia = request.Provincia;
            admin.Pais = request.Pais;
            admin.ContactoEmergencia = request.ContactoEmergencia;
            admin.Email = request.Email;

            await _context.SaveChangesAsync();

            return _mapper.CreateAdministradorResponse(admin);
        }
    }
}
