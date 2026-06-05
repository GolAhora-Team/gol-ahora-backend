using Aplication.Interfaces.IPartido;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Command
{
    public class PartidoCommand : IPartidoCommand
    {
        private readonly AppDbContext _context;

        public PartidoCommand(AppDbContext context)
        {
            _context = context;
        }

        public async Task InsertPartidos(List<Partido> partidos)
        {
            await _context.Partidos.AddRangeAsync(partidos);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePartido(Partido partido)
        {
            _context.Partidos.Update(partido);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePartidosPorCompeticion(int competicionId)
        {
            var partidos = _context.Partidos.Where(p => p.CompeticionId == competicionId);
            if (partidos.Any())
            {
                _context.Partidos.RemoveRange(partidos);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeletePartido(int id)
        {
            var partido = await _context.Partidos.FindAsync(id);
            if (partido != null)
            {
                _context.Partidos.Remove(partido);
                await _context.SaveChangesAsync();
            }
        }
    }
}
