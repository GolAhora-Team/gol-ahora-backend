using Aplication.Interfaces.ICompeticion;
using Domain.Entities;
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
            var competicion = await _context.Competiciones.FindAsync(idCompeticion);

            return competicion;
        }

        public async Task<IEnumerable<Competicion>> GetListCompeticion()
        {
            return await _context.Competiciones.ToListAsync();
        }
    }
}
