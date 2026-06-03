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
                CertificadoFechaFin = profesor.CertificadoFechaFin,
                TieneCertificado = profesor.CertificadoArchivo != null && profesor.CertificadoArchivo.Length > 0
            };
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
                CertificadoFechaFin = dto.CertificadoFechaFin,
                TieneCertificado = dto.TieneCertificado
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
                certificadoBytes = Convert.FromBase64String(base64Data);
            }

            return new Profesor
            {
                Certificacion = request.Certificacion,
                CertificadoArchivo = certificadoBytes,
                CertificadoFechaInicio = request.CertificadoFechaInicio,
                CertificadoFechaFin = request.CertificadoFechaFin,
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
