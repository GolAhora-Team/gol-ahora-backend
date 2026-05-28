using Aplication.DTOs;
using Aplication.Interfaces.IProfesor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Querys
{
    public class ProfesorQuery : IProfesorQuery
    {
        private readonly AppDbContext _context;

        public ProfesorQuery(AppDbContext context)
        {
            _context = context;
        }

        private static ProfesorDto MapToDto(Domain.Entities.Profesor p) => new()
        {
            Id = p.Id,
            Certificacion = p.Certificacion,
            Especialidad = p.Especialidad,
            Dni = p.Dni,
            Nombre = p.Nombre,
            Apellido = p.Apellido,
            Genero = p.Genero,
            FechaNacimiento = p.FechaNacimiento,
            Telefono = p.Telefono,
            Direccion = p.Direccion,
            Localidad = p.Localidad,
            CodigoPostal = p.CodigoPostal,
            Provincia = p.Provincia,
            Pais = p.Pais,
            ContactoEmergencia = p.ContactoEmergencia,
            Email = p.Email,
            FechaRegistro = p.FechaRegistro
        };

        public async Task<IEnumerable<ProfesorDto>> GetAllAsync()
            => await _context.Profesores
                .Select(p => MapToDto(p))
                .ToListAsync();

        public async Task<ProfesorDto?> GetByIdAsync(int id)
        {
            var p = await _context.Profesores.FindAsync(id);
            return p is null ? null : MapToDto(p);
        }

        public async Task<ProfesorDto?> GetByDniAsync(int dni)
        {
            var p = await _context.Profesores
                .FirstOrDefaultAsync(x => x.Dni == dni);
            return p is null ? null : MapToDto(p);
        }

        public async Task<IEnumerable<ProfesorDto>> GetByEspecialidadAsync(string especialidad)
            => await _context.Profesores
                .Where(p => p.Especialidad == especialidad)
                .Select(p => MapToDto(p))
                .ToListAsync();

        public async Task<IEnumerable<ProfesorDto>> GetByClaseAsync(int claseId)
            => await _context.Profesores
     //           .Where(p => p.claseId == claseId)
                .Select(p => MapToDto(p))
                .ToListAsync();
    }
}
