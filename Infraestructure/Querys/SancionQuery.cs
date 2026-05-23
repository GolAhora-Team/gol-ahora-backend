using Aplication.Interfaces.ISancion;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Querys
{
    public class SancionQuery : ISancionQuery
    {
        private readonly AppDbContext _context;

        public SancionQuery(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Sancion> GetSancionById(int sancionId)
        {
            var sancion = await _context.Sanciones.FindAsync(sancionId);
            return sancion;
        }

        public async Task<List<Sancion>> GetSancionesPorJugador(int jugadorId)
        {
            return await _context.Sanciones            
            .Where(s => s.JugadorId == jugadorId)            
            .OrderByDescending(s => s.Fecha)            
            .ToListAsync();
        }
    }
}
