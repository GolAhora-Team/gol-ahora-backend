using Aplication.DTOs.Response;
using Aplication.Interfaces.IPartido;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Querys
{
    public class PartidoQuery : IPartidoQuery
    {
        private readonly AppDbContext _context;

        public PartidoQuery(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Partido> GetPartidoById(int idPartido)
        {
            var partido = await _context.Partidos.FindAsync(idPartido);

            return partido;
        }

        public async Task<List<PartidoResponse>> GetPartidosPorCompeticion(int competicionId)
        {
            return await _context.Partidos
            .Where(p => p.CompeticionId == competicionId)
            .Select(p => new PartidoResponse
            {
                Id = p.Id,
                Fecha = p.Fecha,
                Hora = p.Hora,
                Arbitro = p.Arbitro,
                GolesLocal = p.GolesLocal,
                GolesVisitante = p.GolesVisitante,
                Estado = p.Estado,
                Jornada = p.Jornada,
                Fase = p.Fase,

                EquipoLocalId = p.EquipoLocalId,
                EquipoLocalNombre = p.EquipoLocal.Nombre,

                EquipoVisitanteId = p.EquipoVisitanteId,
                EquipoVisitanteNombre = p.EquipoVisitante.Nombre,

                CompeticionId = p.CompeticionId,
                CompeticionNombre = p.Competicion.Nombre,

                GanadorId = p.GanadorId,
                GanadorNombre = p.Ganador != null ? p.Ganador.Nombre : null
            })
            .ToListAsync();
        }

        public async Task<List<PartidoResponse>> GetPartidosPorFase(int competicionId, FaseTorneo fase)
        {
            return await _context.Partidos
            .Where(p => p.CompeticionId == competicionId && p.Fase == fase)
            .Select(p => new PartidoResponse
            {
                Id = p.Id,
                Fecha = p.Fecha,
                Hora = p.Hora,
                Arbitro = p.Arbitro,
                GolesLocal = p.GolesLocal,
                GolesVisitante = p.GolesVisitante,
                Estado = p.Estado,
                Jornada = p.Jornada,
                Fase = p.Fase,

                EquipoLocalId = p.EquipoLocalId,
                EquipoLocalNombre = p.EquipoLocal.Nombre,

                EquipoVisitanteId = p.EquipoVisitanteId,
                EquipoVisitanteNombre = p.EquipoVisitante.Nombre,

                CompeticionId = p.CompeticionId,
                CompeticionNombre = p.Competicion.Nombre,

                GanadorId = p.GanadorId,
                GanadorNombre = p.Ganador != null ? p.Ganador.Nombre : null
            })
            .ToListAsync();
        }
    }
}
