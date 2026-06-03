using Aplication.Interfaces.ICompeticion;
using Aplication.Interfaces.IEquipo;
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
    public class CompeticionQuery : ICompeticionQuery
    {
        private readonly AppDbContext _context;

        public CompeticionQuery(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Competicion> GetCompeticionById(int idCompeticion)
        {
            var competicion = await _context.Competiciones
                .Include(c => c.Equipos)
                .FirstOrDefaultAsync(c => c.Id == idCompeticion);

            return competicion;
        }
                

        public async Task<IEnumerable<Competicion>> GetListCompeticion()
        {
            return await _context.Competiciones
                .Include(c => c.Equipos)
                .ToListAsync();
        }

        public async Task<Competicion?> GetCompeticionConEquipos(int competicionId)
        {
            return await _context.Competiciones
                .Include(c => c.Equipos) 
                .FirstOrDefaultAsync(c => c.Id == competicionId);
        }
    }
}
