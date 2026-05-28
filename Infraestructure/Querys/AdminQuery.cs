using Aplication.DTOs;
using Aplication.Interfaces.IAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Querys
{
    public class AdminQuery : IAdminQuery
    {
        private readonly AppDbContext _context;

        public AdminQuery(AppDbContext context)
        {
            _context = context;
        }

        private static AdministradorDto MapToDto(Domain.Entities.Administrador a) => new()
        {
            Id = a.Id,
            Identificador = a.Identificador,
            FechaAlta = a.FechaAlta,
            PuedeFacturar = a.PuedeFacturar,
            Dni = a.Dni,
            Nombre = a.Nombre,
            Apellido = a.Apellido,
            Genero = a.Genero,
            FechaNacimiento = a.FechaNacimiento,
            Telefono = a.Telefono,
            Direccion = a.Direccion,
            Localidad = a.Localidad,
            CodigoPostal = a.CodigoPostal,
            Provincia = a.Provincia,
            Pais = a.Pais,
            ContactoEmergencia = a.ContactoEmergencia,
            Email = a.Email,
            FechaRegistro = a.FechaRegistro
        };

        public async Task<IEnumerable<AdministradorDto>> GetAllAsync()
        {
            return await _context.Administradores
                .Select(a => MapToDto(a))
                .ToListAsync();
        }

        public async Task<AdministradorDto?> GetByDniAsync(int dni)
        {
            var admin = await _context.Administradores.FirstOrDefaultAsync(a => a.Dni == dni);
            return admin is null ? null : MapToDto(admin);
        }

        public async Task<AdministradorDto?> GetByIdAsync(int id)
        {
            var admin = await _context.Administradores.FindAsync(id);
            return admin is null ? null : MapToDto(admin);
        }

        public async Task<AdministradorDto?> GetByIdentificadorAsync(int identificador)
        {
            var admin = await _context.Administradores.FirstOrDefaultAsync(a => a.Identificador == identificador);
            return admin is null ? null : MapToDto(admin);
        }

        public async Task<IEnumerable<AdministradorDto>> GetFacturadoresAsync()
        {
            return await _context.Administradores
                .Where(a => a.PuedeFacturar)
                .Select(a => MapToDto(a))
                .ToListAsync();
        }
    }
}
