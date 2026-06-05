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

namespace Aplication.Mappers
{
    public class ProfesorMapper : IProfesorMapper
    {
        public ProfesorResponse CreateProfesorResponse(Profesor profesor)
        {
            if (profesor == null)
            {
                return null;
            }

            return new ProfesorResponse
            {
                Id = profesor.Id,
                Certificacion = profesor.Certificacion,
                Especialidad = profesor.Especialidad,
                Dni = profesor.Dni,
                Nombre = profesor.Nombre,
                Apellido = profesor.Apellido,
                Genero = profesor.Genero,
                FechaNacimiento = profesor.FechaNacimiento,
                Telefono = profesor.Telefono,
                Direccion = profesor.Direccion,
                Localidad = profesor.Localidad,
                CodigoPostal = profesor.CodigoPostal,
                Provincia = profesor.Provincia,
                Pais = profesor.Pais,
                ContactoEmergencia = profesor.ContactoEmergencia,
                Email = profesor.Email,
                FechaRegistro = profesor.FechaRegistro,
                Certificados = profesor.Certificados?.Select(c => new CertificadoProfesorDto
                {
                    Id = c.Id,
                    Url = c.CertificadoUrl,
                    NombreArchivo = c.CertificadoNombreArchivo,
                    FechaInicio = c.FechaInicio,
                    FechaVencimiento = c.FechaVencimiento,
                    Estado = ObtenerEstadoCertificado(c.CertificadoUrl, c.FechaVencimiento)
                }).ToList() ?? new List<CertificadoProfesorDto>()
            };
        }
        
        private string ObtenerEstadoCertificado(string? url, DateTime? fechaVencimiento)
        {
            if (string.IsNullOrEmpty(url)) return "Sin certificado";
            if (!fechaVencimiento.HasValue) return "Certificado válido";
            if (fechaVencimiento.Value < DateTime.UtcNow) return "Certificado vencido";
            return "Certificado válido";
        }

        public ProfesorResponse CreateProfesorResponseFromDto(ProfesorDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new ProfesorResponse
            {
                Id = dto.Id,
                Certificacion = dto.Certificacion,
                Especialidad = dto.Especialidad,
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
                FechaRegistro = dto.FechaRegistro,
                Certificados = dto.Certificados ?? new List<CertificadoProfesorDto>()
            };
        }

        public Profesor CreateProfesorToProfesorRequest(CreateProfesorRequest request)
        {
            if (request == null)
            {
                return null;
            }



            return new Profesor
            {
                Certificacion = request.Certificacion,
                Especialidad = request.Especialidad,
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
                ObraSocial = "Ninguna",
                FechaRegistro = DateTime.UtcNow
            };
        }
        
        public Profesor CreateProfesorFromFormRequest(Aplication.DTOs.Request.Usuario.CreateUsuarioProfesorFormRequest request)
        {
            if (request == null) return null;
            return new Profesor
            {
                Certificacion = request.Certificacion,
                Especialidad = request.Especialidad,
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
                ObraSocial = "Ninguna",
                FechaRegistro = DateTime.UtcNow
            };
        }
    }
}
