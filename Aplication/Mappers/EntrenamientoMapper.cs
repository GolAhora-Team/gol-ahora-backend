using Aplication.DTOs.Request.Entrenamiento;
using Aplication.DTOs.Response.Entrenamiento;
using Aplication.Interfaces.IEntrenamiento;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Aplication.Mappers
{
    public class EntrenamientoMapper : IEntrenamientoMapper
    {
        public Entrenamiento CreateRequestToEntrenamiento(EntrenamientoCreateRequest request)
        {
            return new Entrenamiento
            {
                Nombre = request.Nombre,
                Fecha = request.Fecha,
                CupoMaximo = request.CupoMaximo,
                ProfesorId = request.ProfesorId,
                CanchaId = request.CanchaId
            };
        }

        public EntrenamientoResponse EntrenamientoToResponse(Entrenamiento entrenamiento)
        {
            return new EntrenamientoResponse
            {
                Id = entrenamiento.Id,
                Nombre = entrenamiento.Nombre,
                Fecha = entrenamiento.Fecha,
                CupoMaximo = entrenamiento.CupoMaximo,
                ProfesorId = entrenamiento.ProfesorId,
                CanchaId = entrenamiento.CanchaId,
                Profesor = entrenamiento.Profesor != null ? new EntrenamientoProfesorResponse
                {
                    Id = entrenamiento.Profesor.Id,
                    Nombre = entrenamiento.Profesor.Nombre,
                    Apellido = entrenamiento.Profesor.Apellido
                } : null,
                Clientes = entrenamiento.Clientes != null ? entrenamiento.Clientes
                    .Where(c => c.Cliente != null)
                    .Select(c => new EntrenamientoClienteResponse
                    {
                        Id = c.Cliente.Id,
                        Nombre = c.Cliente.Nombre,
                        Apellido = c.Cliente.Apellido
                    }).ToList() : new List<EntrenamientoClienteResponse>()
            };
        }
    }
}
