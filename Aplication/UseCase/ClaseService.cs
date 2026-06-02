using Aplication.DTOs.Request.Clase;
using Aplication.DTOs.Response.Clase;
using Aplication.Interfaces.IClases;
using Aplication.Interfaces.ICliente;
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
        private readonly IProfesorCommand _profesorCommand;
        private readonly IClientesQuery _clienteQuery;

        public ClaseService(IClaseCommand claseCommand, IClaseQuery claseQuery, IClaseMapper claseMapper, IProfesorQuery profesorQuery, IProfesorCommand profesorCommand, IClientesQuery clienteQuery)
        {
            _claseCommand = claseCommand;
            _claseQuery = claseQuery;
            _claseMapper = claseMapper;
            _profesorQuery = profesorQuery;
            _profesorCommand = profesorCommand;
            _clienteQuery = clienteQuery;
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

            var profesorExistente = await _profesorQuery.GetByIdAsync(clase.ProfesorId);
            if (profesorExistente == null)
            {
                throw new ExceptionNotFound("Profesor no encontrado.");
            }

            bool esValido = await _profesorCommand.ValidarCertificadoAsync(clase.ProfesorId);
            if (!esValido)
            {
                throw new ExceptionBadRequest("El profesor no tiene un certificado válido o ha expirado.");
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

            bool esValido = await _profesorCommand.ValidarCertificadoAsync(clase.ProfesorId);
            if (!esValido)
            {
                throw new ExceptionBadRequest("El profesor no tiene un certificado válido o ha expirado.");
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

        public async Task<ClaseShortResponse> addProfesor(int claseId, int profesorId)
        {
            var claseExistente = await _claseQuery.GetClaseById(claseId);
            var profesorExistente = await _profesorQuery.GetByIdAsync(profesorId);
            if (claseExistente == null)
            {
                throw new ExceptionNotFound("Clase no encontrada.");
            }
            if (profesorExistente == null)
            {
                throw new ExceptionNotFound("Profesor no encontrado.");
            }

            // Validar que el profesor tiene certificado vigente
            bool esValido = await _profesorCommand.ValidarCertificadoAsync(profesorId);
            if (!esValido)
            {
                throw new ExceptionBadRequest("El profesor no tiene un certificado válido o ha expirado.");
            }

            claseExistente.ProfesorId = profesorId;
            await _claseCommand.UpdateClase(claseExistente);
            return new ClaseShortResponse
            {
                Id = claseExistente.Id,
                Nombre = claseExistente.Nombre,
                capacidadMax = claseExistente.CapacidadMax,
                IdProfesor = profesorId
            };
        }

        public async Task<ClaseShortResponse> addCliente(int claseId, int clienteId)
        {
            var claseExistente = await _claseQuery.GetClaseById(claseId);
            var clienteExiste = await _clienteQuery.GetClienteById(clienteId);

            if (claseExistente == null)
            {
                throw new ExceptionNotFound("Clase no encontrada.");
            }
            if (clienteExiste == null)
            {
                throw new ExceptionNotFound("Cliente no encontrado");
            }
            if (claseExistente.Asistencias.Count >= claseExistente.CapacidadMax)
            {
                throw new ExceptionBadRequest("La clase ha alcanzado su capacidad máxima.");
            }
            if (claseExistente.Asistencias.Any(a => a.ClienteId == clienteId))
            {
                throw new ExceptionBadRequest("El cliente ya está inscrito en esta clase.");
            }
            var nuevaAsistencia = new Asistencia
            {
                ClaseId = claseId,
                ClienteId = clienteId,
                Fecha = claseExistente.Fecha,
                Presente = false
            };
            claseExistente.Asistencias.Add(nuevaAsistencia);
            await _claseCommand.UpdateClase(claseExistente);
            return new ClaseShortResponse
            {
                Id = claseExistente.Id,
                Nombre = claseExistente.Nombre,
                capacidadMax = claseExistente.CapacidadMax,
                IdProfesor = claseExistente.ProfesorId
            };
        }
    }
}
