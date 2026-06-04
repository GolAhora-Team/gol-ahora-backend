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
            FechaRegistro = p.FechaRegistro,
            CertificadoFechaInicio = p.CertificadoFechaInicio,
            CertificadoFechaVencimiento = p.CertificadoFechaVencimiento,
            TieneCertificado = !string.IsNullOrEmpty(p.CertificadoUrl),
            CertificadoUrl = p.CertificadoUrl,
            CertificadoNombreArchivo = p.CertificadoNombreArchivo,
            CertificadoEstado = ObtenerEstadoCertificado(p.CertificadoUrl, p.CertificadoFechaVencimiento)
        };

        private static string ObtenerEstadoCertificado(string? url, DateTime? fechaVencimiento)
        {
            if (string.IsNullOrEmpty(url)) return "Sin certificado";
            if (!fechaVencimiento.HasValue) return "Certificado válido";
            if (fechaVencimiento.Value < DateTime.UtcNow) return "Certificado vencido";
            return "Certificado válido";
        }

        public async Task<IEnumerable<ProfesorDto>> GetAllAsync()
        {
            var profesores = await _context.Profesores.ToListAsync();
            return profesores.Select(p => MapToDto(p)).ToList();
        }

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
        {
            var profesores = await _context.Profesores
                .Where(p => p.Especialidad == especialidad)
                .ToListAsync();
            return profesores.Select(p => MapToDto(p)).ToList();
        }

        public async Task<IEnumerable<ProfesorDto>> GetByClaseAsync(int claseId)
        {
            var profesores = await _context.Profesores
     //           .Where(p => p.claseId == claseId)
                .ToListAsync();
            return profesores.Select(p => MapToDto(p)).ToList();
        }

        public async Task<byte[]?> GetCertificadoAsync(int profesorId)
        {
            var p = await _context.Profesores.FindAsync(profesorId);
            if (p == null || string.IsNullOrEmpty(p.CertificadoUrl)) return null;

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var filePath = Path.Combine(uploadsFolder, p.CertificadoUrl.TrimStart('/'));

            if (System.IO.File.Exists(filePath))
            {
                return await System.IO.File.ReadAllBytesAsync(filePath);
            }
            return null;
        }
    }
}
