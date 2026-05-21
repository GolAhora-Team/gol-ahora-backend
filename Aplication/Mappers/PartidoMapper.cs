using Aplication.DTOs.Response;
using Aplication.Interfaces.IPartido;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Mappers
{
    public class PartidoMapper : IPartidoMapper
    {
        public PartidoResponse CreatePartidoResponse(Partido partido)
        {
            if (partido == null) return null;

            return new PartidoResponse
            {
                Id = partido.Id,
                Fecha = partido.Fecha,
                Hora = partido.Hora,
                Arbitro = partido.Arbitro,
                GolesLocal = partido.GolesLocal,
                GolesVisitante = partido.GolesVisitante,
                Estado = partido.Estado,
                Jornada = partido.Jornada,
                Fase = partido.Fase,

                
                EquipoLocalId = partido.EquipoLocalId,
                EquipoLocalNombre = partido.EquipoLocal?.Nombre, 

                EquipoVisitanteId = partido.EquipoVisitanteId,
                EquipoVisitanteNombre = partido.EquipoVisitante?.Nombre,

                CompeticionId = partido.CompeticionId,
                CompeticionNombre = partido.Competicion?.Nombre,
                                
                GanadorId = partido.GanadorId,
                GanadorNombre = partido.Ganador?.Nombre
            };
        }

        public List<PartidoResponse> CreatePartidoResponseList(List<Partido> partidos)
        {
            if (partidos == null) return new List<PartidoResponse>();

            return partidos.Select(partido => CreatePartidoResponse(partido)).ToList();
        }
    }
}
