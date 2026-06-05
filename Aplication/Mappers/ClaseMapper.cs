using Aplication.DTOs.Request.Clase;
using Aplication.DTOs.Response.Clase;
using Aplication.Interfaces.IClases;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Mappers
{
    public class ClaseMapper: IClaseMapper
    {
        public Clase createClasetoRequest(ClaseCreateRequest request)
        {
            return new Clase
            {
                Nombre = request.Nombre,
                Descripcion = request.Descripcion,
                CapacidadMax = request.CapacidadMax,
                Fecha = request.Fecha,
                HoraInicio = request.HoraInicio,
                HoraFin = request.HoraFin,
                PrecioInscripcion = request.PrecioInscripcion,
                ProfesorId = request.ProfesorId,
                CanchaId = request.CanchaId,
                DiasSemana = request.DiasSemana
            };
        }

        public ClaseCreateResponse ClaseCreateResponse(Clase clase)
        {
            return new ClaseCreateResponse
            {
                Id = clase.Id,
                Nombre = clase.Nombre,
                capacidadMax = clase.CapacidadMax,
                Fecha = clase.Fecha,
                HoraInicio = clase.HoraInicio,
                HoraFin = clase.HoraFin
            };

        }

        public ClaseResponse ClaseToClaseResponse(Clase clase)
        {
            return new ClaseResponse
            {
                Id = clase.Id,
                Nombre = clase.Nombre,
                Descripcion = clase.Descripcion,
                CapacidadMax = clase.CapacidadMax,
                Fecha = clase.Fecha,
                HoraInicio = clase.HoraInicio,
                HoraFin = clase.HoraFin,
                PrecioInscripcion = clase.PrecioInscripcion,
                Profesor = clase.Profesor != null ? new ProfesorShortResponse
                {
                    Id = clase.Profesor.Id,
                    Nombre = clase.Profesor.Nombre,
                    Apellido = clase.Profesor.Apellido,
                    especialidad = clase.Profesor.Especialidad
                } : null,
                Alumnos = clase.Asistencias != null ? clase.Asistencias.Where(i => i.Cliente != null).Select(i => new ClaseAlumnos
                {
                    Id = i.Cliente.Id,
                    Nombre = i.Cliente.Nombre,
                    Apellido = i.Cliente.Apellido
                }).ToList() : new List<ClaseAlumnos>()
            };
        }
    }
}
