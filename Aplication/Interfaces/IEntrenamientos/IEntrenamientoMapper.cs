using Aplication.DTOs.Request.Entrenamiento;
using Aplication.DTOs.Response.Entrenamiento;
using Domain.Entities;

namespace Aplication.Interfaces.IEntrenamiento
{
    public interface IEntrenamientoMapper
    {
        Entrenamiento CreateRequestToEntrenamiento(EntrenamientoCreateRequest request);
        EntrenamientoResponse EntrenamientoToResponse(Entrenamiento entrenamiento);
    }
}
