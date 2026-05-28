using Aplication.Interfaces.ICompeticion;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Command
{
    public class CompeticionCommand : ICompeticionCommand
    {
        private readonly AppDbContext _context;

        public CompeticionCommand(AppDbContext context)
        {
            _context = context;
        }

        public async Task InsertCompeticion(Competicion competicion)
        {
            _context.Add(competicion);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveCompeticion(int idCompeticion)
        {
            var competicion = await _context.Competiciones.FindAsync(idCompeticion);

            if (competicion != null)
            {
                _context.Remove(competicion);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateCompeticion(Competicion competicion)
        {
            _context.Update(competicion);
            await _context.SaveChangesAsync();
        }
    }
}
