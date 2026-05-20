using Aplication.DTOs.Request.Clase;
using Aplication.DTOs.Response.Clase;
using Aplication.Interfaces.IClases;
using Aplication.Interfaces.IProfesor;
using Domain.Entities;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.UseCase
{
    public class ClaseService : IClaseService
    {
        private readonly IClaseCommand _claseCommand;
        private readonly IClaseQuery _claseQuery;
        private readonly IClaseMapper _claseMapper;
        private readonly IProfesorQuery _profesorQuery;

        public ClaseService(IClaseCommand claseCommand, IClaseQuery claseQuery, IClaseMapper claseMapper, IProfesorQuery profesorQuery)
        {
            _claseCommand = claseCommand;
            _claseQuery = claseQuery;
            _claseMapper = claseMapper;
            _profesorQuery = profesorQuery;
        }
        public async Task<ClaseCreateResponse> CrearClase(ClaseCreateRequest clase)
        {

            if (clase.CapacidadMax < 0)
            {
                throw new ExceptionBadRequest("La capacidad máxima no puede ser negativa.");
            }
            if (clase.PrecioInscripcion < 0)
            {
                throw new ExceptionBadRequest("El precio de inscripción no puede ser negativo.");
            }
            if (clase.Fecha < DateTime.UtcNow)
            {
                throw new ExceptionBadRequest("La fecha de la clase no puede ser en el pasado.");
            }

            var nuevaClase = _claseMapper.createClasetoRequest(clase);
            await _claseCommand.InsertClase(nuevaClase);

            return _claseMapper.ClaseCreateResponse(nuevaClase);
        }

        public async Task<ClaseResponse> getClaseId(int id)
        {
            var clase = await _claseQuery.GetClaseById(id);
            if (clase == null)
            {
                throw new ExceptionNotFound("Clase no encontrada.");
            }
            return _claseMapper.ClaseToClaseResponse(clase);
        }

        public async Task<List<ClaseResponse>> getClases()
        {
            var clases = await _claseQuery.GetAllClases();
            return clases.Select(c => _claseMapper.ClaseToClaseResponse(c)).ToList();
        }

        public async Task<ClaseCreateResponse> UpdateClase(int id, ClaseUpdateRequest clase)
        {
            var claseExistente = await _claseQuery.GetClaseById(id);
            var profesorExistente = await _profesorQuery.GetByIdAsync(clase.ProfesorId);
            if (claseExistente == null)
            {
                throw new ExceptionNotFound("Clase no encontrada.");
            }
            if (profesorExistente == null)
            {
                throw new ExceptionNotFound("Profesor no encontrado.");
            }
            if (clase.CapacidadMax < 0)
            {
                throw new ExceptionBadRequest("La capacidad máxima no puede ser negativa.");
            }
            if (clase.PrecioInscripcion < 0)
            {
                throw new ExceptionBadRequest("El precio de inscripción no puede ser negativo.");
            }
            if (clase.Fecha < DateTime.UtcNow)
            {
                throw new ExceptionBadRequest("La fecha de la clase no puede ser en el pasado.");
            }

            claseExistente.Nombre = clase.Nombre;
            claseExistente.Descripcion = clase.Descripcion;
            claseExistente.CapacidadMax = clase.CapacidadMax;
            claseExistente.Fecha = clase.Fecha;
            claseExistente.HoraInicio = clase.HoraInicio;
            claseExistente.HoraFin = clase.HoraFin;
            claseExistente.PrecioInscripcion = clase.PrecioInscripcion;

            await _claseCommand.UpdateClase(claseExistente);

            return _claseMapper.ClaseCreateResponse(claseExistente);
        }

        public async Task<ClaseDeleteResponse> DeleteClase(int id)
        {
            var claseExistente = await _claseQuery.GetClaseById(id);
            if (claseExistente == null)
            {
                throw new ExceptionNotFound("Clase no encontrada.");
            }
            await _claseCommand.RemoveClase(claseExistente);

            return new ClaseDeleteResponse
            {
                Id = claseExistente.Id,
                Nombre = claseExistente.Nombre
            };
        }
    }
}
