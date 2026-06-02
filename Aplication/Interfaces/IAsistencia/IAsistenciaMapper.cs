using Aplication.DTOs.Response.Asistencia;
using Domain.Entities;
using System.Collections.Generic;

namespace Aplication.Interfaces.IAsistencia
{
    public interface IAsistenciaMapper
    {
        AsistenciaResponse MapToResponse(Asistencia asistencia);
        List<AsistenciaResponse> MapToResponseList(List<Asistencia> asistencias);
    }
}
