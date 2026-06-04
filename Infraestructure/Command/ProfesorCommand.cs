using Aplication.DTOs;
using Aplication.DTOs.Request.Profesor;
using Aplication.DTOs.Response.Profesor;
using Aplication.Interfaces.IProfesor;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Command
{
    public class ProfesorCommand : IProfesorCommand
    {
        private readonly AppDbContext _context;
        private readonly IProfesorMapper _mapper;

        public ProfesorCommand(AppDbContext context, IProfesorMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProfesorDto> CreateAsync(ProfesorDto dto)
        {
            var profesor = new Profesor
            {
                Certificacion = dto.Certificacion,
                Especialidad = dto.Especialidad,
                //ClaseId = dto.ClaseId,
                //EntrenamientoId = dto.EntrenamientoId,
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

            _context.Profesores.Add(profesor);
            await _context.SaveChangesAsync();

            dto.Id = profesor.Id;
            return dto;
        }

        public async Task CreateProfesor(Profesor profesor)
        {
            _context.Profesores.Add(profesor);
            await _context.SaveChangesAsync();
        }

        public async Task<ProfesorDto?> UpdateAsync(int id, ProfesorDto dto)
        {
            var profesor = await _context.Profesores.FindAsync(id);
            if (profesor is null) return null;

            profesor.Certificacion = dto.Certificacion;
            profesor.Especialidad = dto.Especialidad;
            profesor.Nombre = dto.Nombre;
            profesor.Apellido = dto.Apellido;
            profesor.Telefono = dto.Telefono;
            profesor.Email = dto.Email;

            await _context.SaveChangesAsync();

            dto.Id = profesor.Id;
            return dto;
        }

        public async Task<ProfesorResponse?> UpdateSimpleAsync(int id, UpdateProfesorSimpleRequest request)
        {
            var profesor = await _context.Profesores.Include(p => p.Certificados).FirstOrDefaultAsync(p => p.Id == id);
            if (profesor is null) return null;

            profesor.Nombre = request.Nombre;
            profesor.Apellido = request.Apellido;
            profesor.Dni = request.Dni;
            profesor.Genero = request.Genero;
            if (request.ObraSocial != null) profesor.ObraSocial = request.ObraSocial;
            profesor.FechaNacimiento = request.FechaNacimiento;
            profesor.Telefono = request.Telefono;
            profesor.Direccion = request.Direccion;
            profesor.Localidad = request.Localidad;
            profesor.CodigoPostal = request.CodigoPostal;
            profesor.Provincia = request.Provincia;
            profesor.Pais = request.Pais;
            profesor.ContactoEmergencia = request.ContactoEmergencia;
            profesor.Email = request.Email;
            
            if (request.Especialidad != null) profesor.Especialidad = request.Especialidad;
            if (request.Certificacion != null) profesor.Certificacion = request.Certificacion;

            if (request.Certificados != null)
            {
                var currentCertificados = profesor.Certificados.ToList();

                // Eliminar los que vienen con Eliminar = true
                var toDelete = request.Certificados.Where(c => c.Eliminar && c.Id.HasValue).Select(c => c.Id.Value).ToList();
                foreach (var idToDelete in toDelete)
                {
                    var cert = currentCertificados.FirstOrDefault(c => c.Id == idToDelete);
                    if (cert != null)
                    {
                        _context.CertificadosProfesores.Remove(cert);
                    }
                }

                // Actualizar o agregar nuevos
                foreach (var certReq in request.Certificados.Where(c => !c.Eliminar))
                {
                    if (certReq.Id.HasValue && certReq.Id.Value > 0)
                    {
                        var cert = currentCertificados.FirstOrDefault(c => c.Id == certReq.Id.Value);
                        if (cert != null)
                        {
                            cert.FechaInicio = certReq.CertificadoFechaInicio;
                            cert.FechaVencimiento = certReq.CertificadoFechaVencimiento;
                        }
                    }
                    else if (!string.IsNullOrEmpty(certReq.CertificadoBase64))
                    {
                        var base64Data = certReq.CertificadoBase64;
                        if (base64Data.Contains(","))
                        {
                            base64Data = base64Data.Split(',')[1];
                        }

                        var fileBytes = Convert.FromBase64String(base64Data);
                        if (fileBytes.Length > 4 * 1024 * 1024)
                        {
                            throw new Domain.Exceptions.ExceptionBadRequest("Un certificado excede el límite máximo de 4 MB.");
                        }

                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "certificados", "profesores");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        var uniqueFileName = Guid.NewGuid().ToString() + ".pdf"; 
                        if (fileBytes.Length > 4 && fileBytes[0] == 0xFF && fileBytes[1] == 0xD8)
                        {
                            uniqueFileName = Guid.NewGuid().ToString() + ".jpg";
                        }
                        else if (fileBytes.Length > 4 && fileBytes[0] == 0x89 && fileBytes[1] == 0x50 && fileBytes[2] == 0x4E && fileBytes[3] == 0x47)
                        {
                            uniqueFileName = Guid.NewGuid().ToString() + ".png";
                        }

                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        await File.WriteAllBytesAsync(filePath, fileBytes);

                        profesor.Certificados.Add(new CertificadoProfesor
                        {
                            CertificadoUrl = $"/certificados/profesores/{uniqueFileName}",
                            CertificadoNombreArchivo = "Certificado" + Path.GetExtension(uniqueFileName),
                            FechaInicio = certReq.CertificadoFechaInicio,
                            FechaVencimiento = certReq.CertificadoFechaVencimiento
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();

            return _mapper.CreateProfesorResponse(profesor);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var profesor = await _context.Profesores.FindAsync(id);
            if (profesor is null) return false;

            _context.Profesores.Remove(profesor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AsignarClaseAsync(int profesorId, int claseId)
        {
            var profesor = await _context.Profesores.FindAsync(profesorId);
            if (profesor is null) return false;

            //profesor.ClaseId = claseId;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AsignarEntrenamientoAsync(int profesorId, int entrenamientoId)
        {
            var profesor = await _context.Profesores.FindAsync(profesorId);
            if (profesor is null) return false;

            //profesor.EntrenamientoId = entrenamientoId;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ValidarCertificadoAsync(int profesorId)
        {
            var profesor = await _context.Profesores.Include(p => p.Certificados).FirstOrDefaultAsync(p => p.Id == profesorId);
            if (profesor is null) return false;

            var ultimoCertificado = profesor.Certificados?.OrderByDescending(c => c.FechaVencimiento).FirstOrDefault();
            
            bool tieneCertificado = ultimoCertificado != null && !string.IsNullOrEmpty(ultimoCertificado.CertificadoUrl);
            bool estaVigente = ultimoCertificado != null && ultimoCertificado.FechaVencimiento.HasValue && ultimoCertificado.FechaVencimiento.Value >= DateTime.UtcNow;

            return tieneCertificado && estaVigente;
        }
    }
}
