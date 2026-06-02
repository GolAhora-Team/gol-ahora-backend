using Aplication.DTOs.Response.Asistencia;
using Aplication.Interfaces.IAsistencia;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Aplication.Mappers
{
    public class AsistenciaMapper : IAsistenciaMapper
    {
        public AsistenciaResponse MapToResponse(Asistencia asistencia)
        {
            return new AsistenciaResponse
            {
                Id = asistencia.Id,
                Presente = asistencia.Presente,
                Fecha = asistencia.Fecha,
                ClaseId = asistencia.ClaseId,
                ClienteId = asistencia.ClienteId
            };
        }

        public List<AsistenciaResponse> MapToResponseList(List<Asistencia> asistencias)
        {
            return asistencias.Select(MapToResponse).ToList();
        }
    }
}
