using Aplication.DTOs.Request.Entrenamiento;
using Aplication.DTOs.Response.Entrenamiento;
using Aplication.Interfaces.IEntrenamiento;
using Domain.Entities;

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
                HoraInicio = request.HoraInicio,
                HoraFin = request.HoraFin,
                DiasSemana = request.DiasSemana,
                CanchaId = request.CanchaId,
                PrecioInscripcion = request.PrecioInscripcion
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
                HoraInicio = entrenamiento.HoraInicio,
                HoraFin = entrenamiento.HoraFin,
                DiasSemana = entrenamiento.DiasSemana,
                CanchaNombre = entrenamiento.Cancha?.Nombre ?? "Sin Cancha",
                PrecioInscripcion = entrenamiento.PrecioInscripcion
            };
        }
    }
}
