using Aplication.DTOs.Request.Entrenamiento;
using Aplication.DTOs.Response.Entrenamiento;
using Aplication.Interfaces.IEntrenamiento;
using Aplication.Interfaces.IProfesor;
using Aplication.Interfaces.ICliente;
using Domain.Entities;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplication.UseCase.Entrenamientos
{
    public class EntrenamientoService : IEntrenamientoService
    {
        private readonly IEntrenamientoCommand _entrenamientoCommand;
        private readonly IEntrenamientoQuery _entrenamientoQuery;
        private readonly IEntrenamientoMapper _entrenamientoMapper;
        private readonly IProfesorQuery _profesorQuery;
        private readonly IProfesorCommand _profesorCommand;
        private readonly IClientesQuery _clienteQuery;

        public EntrenamientoService(
            IEntrenamientoCommand entrenamientoCommand,
            IEntrenamientoQuery entrenamientoQuery,
            IEntrenamientoMapper entrenamientoMapper,
            IProfesorQuery profesorQuery,
            IProfesorCommand profesorCommand,
            IClientesQuery clienteQuery)
        {
            _entrenamientoCommand = entrenamientoCommand;
            _entrenamientoQuery = entrenamientoQuery;
            _entrenamientoMapper = entrenamientoMapper;
            _profesorQuery = profesorQuery;
            _profesorCommand = profesorCommand;
            _clienteQuery = clienteQuery;
        }

        public async Task<EntrenamientoResponse> CrearEntrenamiento(EntrenamientoCreateRequest request)
        {
            if (request.CupoMaximo <= 0)
            {
                throw new ExceptionBadRequest("El cupo máximo debe ser mayor a 0.");
            }
            if (request.Fecha < DateTime.UtcNow)
            {
                throw new ExceptionBadRequest("La fecha no puede ser en el pasado.");
            }
            
            var profesorExistente = await _profesorQuery.GetByIdAsync(request.ProfesorId);
            if (profesorExistente == null)
            {
                throw new ExceptionNotFound("Profesor no encontrado.");
            }

            bool esValido = await _profesorCommand.ValidarCertificadoAsync(request.ProfesorId);
            if (!esValido)
            {
                throw new ExceptionBadRequest("El profesor no tiene un certificado válido o ha expirado.");
            }

            var nuevoEntrenamiento = _entrenamientoMapper.CreateRequestToEntrenamiento(request);
            await _entrenamientoCommand.InsertEntrenamiento(nuevoEntrenamiento);

            return _entrenamientoMapper.EntrenamientoToResponse(nuevoEntrenamiento);
        }

        public async Task<EntrenamientoResponse> GetEntrenamientoById(int id)
        {
            var entrenamiento = await _entrenamientoQuery.GetEntrenamientoById(id);
            if (entrenamiento == null)
            {
                throw new ExceptionNotFound("Entrenamiento no encontrado.");
            }
            return _entrenamientoMapper.EntrenamientoToResponse(entrenamiento);
        }

        public async Task<List<EntrenamientoResponse>> GetAllEntrenamientos()
        {
            var entrenamientos = await _entrenamientoQuery.GetAllEntrenamientos();
            return entrenamientos.Select(e => _entrenamientoMapper.EntrenamientoToResponse(e)).ToList();
        }

        public async Task<EntrenamientoResponse> UpdateEntrenamiento(int id, EntrenamientoUpdateRequest request)
        {
            var entrenamientoExistente = await _entrenamientoQuery.GetEntrenamientoById(id);
            if (entrenamientoExistente == null)
            {
                throw new ExceptionNotFound("Entrenamiento no encontrado.");
            }

            var profesorExistente = await _profesorQuery.GetByIdAsync(request.ProfesorId);
            if (profesorExistente == null)
            {
                throw new ExceptionNotFound("Profesor no encontrado.");
            }

            bool esValido = await _profesorCommand.ValidarCertificadoAsync(request.ProfesorId);
            if (!esValido)
            {
                throw new ExceptionBadRequest("El profesor no tiene un certificado válido o ha expirado.");
            }

            if (request.CupoMaximo <= 0)
            {
                throw new ExceptionBadRequest("El cupo máximo debe ser mayor a 0.");
            }
            if (request.Fecha < DateTime.UtcNow)
            {
                throw new ExceptionBadRequest("La fecha no puede ser en el pasado.");
            }

            entrenamientoExistente.Nombre = request.Nombre;
            entrenamientoExistente.Fecha = request.Fecha;
            entrenamientoExistente.CupoMaximo = request.CupoMaximo;
            entrenamientoExistente.ProfesorId = request.ProfesorId;
            entrenamientoExistente.CanchaId = request.CanchaId;

            await _entrenamientoCommand.UpdateEntrenamiento(entrenamientoExistente);

            return _entrenamientoMapper.EntrenamientoToResponse(entrenamientoExistente);
        }

        public async Task<EntrenamientoDeleteResponse> DeleteEntrenamiento(int id)
        {
            var entrenamientoExistente = await _entrenamientoQuery.GetEntrenamientoById(id);
            if (entrenamientoExistente == null)
            {
                throw new ExceptionNotFound("Entrenamiento no encontrado.");
            }

            await _entrenamientoCommand.RemoveEntrenamiento(entrenamientoExistente);

            return new EntrenamientoDeleteResponse
            {
                Id = entrenamientoExistente.Id,
                Nombre = entrenamientoExistente.Nombre
            };
        }

        public async Task<EntrenamientoShortResponse> AddCliente(int entrenamientoId, int clienteId)
        {
            var entrenamientoExistente = await _entrenamientoQuery.GetEntrenamientoById(entrenamientoId);
            var clienteExiste = await _clienteQuery.GetClienteById(clienteId);

            if (entrenamientoExistente == null)
            {
                throw new ExceptionNotFound("Entrenamiento no encontrado.");
            }
            if (clienteExiste == null)
            {
                throw new ExceptionNotFound("Cliente no encontrado.");
            }

            // Validación de Apto Médico
            if (clienteExiste.AptoMedicoArchivo == null || clienteExiste.AptoMedicoArchivo.Length == 0)
            {
                throw new ExceptionBadRequest("El cliente no posee un apto médico cargado.");
            }
            if (!clienteExiste.AptoMedicoFechaFin.HasValue || clienteExiste.AptoMedicoFechaFin.Value < DateTime.UtcNow)
            {
                throw new ExceptionBadRequest("El apto médico del cliente se encuentra vencido.");
            }
            
            // Validar Cupo de Entrenamiento
            int clientesActuales = entrenamientoExistente.Clientes?.Count ?? 0;
            if (clientesActuales >= entrenamientoExistente.CupoMaximo)
            {
                throw new ExceptionBadRequest("El entrenamiento ha alcanzado su cupo máximo.");
            }
            
            if (entrenamientoExistente.Clientes != null && entrenamientoExistente.Clientes.Any(c => c.ClienteId == clienteId))
            {
                throw new ExceptionBadRequest("El cliente ya está inscrito en este entrenamiento.");
            }

            var nuevoClienteEntrenamiento = new ClienteEntrenamiento
            {
                EntrenamientoId = entrenamientoId,
                ClienteId = clienteId
            };
            
            if (entrenamientoExistente.Clientes == null)
            {
                entrenamientoExistente.Clientes = new List<ClienteEntrenamiento>();
            }
            
            entrenamientoExistente.Clientes.Add(nuevoClienteEntrenamiento);
            await _entrenamientoCommand.UpdateEntrenamiento(entrenamientoExistente);

            return new EntrenamientoShortResponse
            {
                Id = entrenamientoExistente.Id,
                Nombre = entrenamientoExistente.Nombre,
                CupoMaximo = entrenamientoExistente.CupoMaximo
            };
        }
    }
}
