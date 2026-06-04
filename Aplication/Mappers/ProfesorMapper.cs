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
                CertificadoFechaInicio = profesor.CertificadoFechaInicio,
                CertificadoFechaVencimiento = profesor.CertificadoFechaVencimiento,
                TieneCertificado = !string.IsNullOrEmpty(profesor.CertificadoUrl),
                CertificadoUrl = profesor.CertificadoUrl,
                CertificadoNombreArchivo = profesor.CertificadoNombreArchivo,
                CertificadoEstado = ObtenerEstadoCertificado(profesor.CertificadoUrl, profesor.CertificadoFechaVencimiento)
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
                CertificadoFechaInicio = dto.CertificadoFechaInicio,
                CertificadoFechaVencimiento = dto.CertificadoFechaVencimiento,
                TieneCertificado = dto.TieneCertificado,
                CertificadoUrl = dto.CertificadoUrl,
                CertificadoNombreArchivo = dto.CertificadoNombreArchivo,
                CertificadoEstado = dto.CertificadoEstado
            };
        }

        public Profesor CreateProfesorToProfesorRequest(CreateProfesorRequest request)
        {
            if (request == null)
            {
                return null;
            }

            byte[]? certificadoBytes = null;
            if (!string.IsNullOrEmpty(request.CertificadoBase64))
            {
                // Extraer el tipo MIME y los datos base64 si están en el formato "data:application/pdf;base64,..."
                var base64Data = request.CertificadoBase64;
                if (base64Data.Contains(","))
                {
                    base64Data = base64Data.Split(',')[1];
                }
                
                int mod4 = base64Data.Length % 4;
                if (mod4 > 0)
                {
                    base64Data += new string('=', 4 - mod4);
                }

                try
                {
                    certificadoBytes = Convert.FromBase64String(base64Data);
                }
                catch (FormatException)
                {
                    throw new Domain.Exceptions.ExceptionBadRequest("El archivo PDF del certificado está corrupto o tiene un formato inválido.");
                }
            }

            return new Profesor
            {
                Certificacion = request.Certificacion,
                CertificadoFechaInicio = request.CertificadoFechaInicio,
                CertificadoFechaVencimiento = request.CertificadoFechaVencimiento,
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
                Email = request.Email
            };
        }
        
        public Profesor CreateProfesorFromFormRequest(Aplication.DTOs.Request.Usuario.CreateUsuarioProfesorFormRequest request)
        {
            if (request == null) return null;
            return new Profesor
            {
                Certificacion = request.Certificacion,
                CertificadoFechaInicio = request.CertificadoFechaInicio,
                CertificadoFechaVencimiento = request.CertificadoFechaVencimiento,
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
                Email = request.Email
            };
        }
    }
}
