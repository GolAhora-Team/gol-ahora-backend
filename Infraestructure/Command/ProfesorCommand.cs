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
            var profesor = await _context.Profesores.FindAsync(id);
            if (profesor is null) return null;

            profesor.Nombre = request.Nombre;
            profesor.Apellido = request.Apellido;
            profesor.Dni = request.Dni;
            profesor.Genero = request.Genero;
            profesor.ObraSocial = request.ObraSocial;
            profesor.FechaNacimiento = request.FechaNacimiento;
            profesor.Telefono = request.Telefono;
            profesor.Direccion = request.Direccion;
            profesor.Localidad = request.Localidad;
            profesor.CodigoPostal = request.CodigoPostal;
            profesor.Provincia = request.Provincia;
            profesor.Pais = request.Pais;
            profesor.ContactoEmergencia = request.ContactoEmergencia;
            profesor.Email = request.Email;

            if (!string.IsNullOrEmpty(request.CertificadoBase64))
            {
                var base64Data = request.CertificadoBase64;
                if (base64Data.Contains(","))
                {
                    base64Data = base64Data.Split(',')[1];
                }
                
                var sizeInBytes = (base64Data.Length * 3) / 4;
                if (sizeInBytes > 4 * 1024 * 1024)
                {
                    throw new Domain.Exceptions.ExceptionBadRequest("El certificado excede el límite máximo de 4 MB.");
                }

                profesor.CertificadoArchivo = Convert.FromBase64String(base64Data);
                profesor.CertificadoFechaInicio = request.CertificadoFechaInicio;
                profesor.CertificadoFechaFin = request.CertificadoFechaFin;
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
            var profesor = await _context.Profesores.FindAsync(profesorId);
            if (profesor is null) return false;

            // Lógica de validación según tu dominio
            bool tieneCertificado = profesor.CertificadoArchivo != null && profesor.CertificadoArchivo.Length > 0;
            bool estaVigente = profesor.CertificadoFechaFin.HasValue && profesor.CertificadoFechaFin.Value >= DateTime.UtcNow;

            return tieneCertificado && estaVigente;
        }
    }
}
