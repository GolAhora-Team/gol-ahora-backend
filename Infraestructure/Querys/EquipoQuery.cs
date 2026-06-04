using Aplication.Interfaces.IEquipo;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Querys
{
    public class EquipoQuery : IEquipoQuery
    {
        private readonly AppDbContext _context;

        public EquipoQuery(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Equipo> GetEquipoById(int idEquipo)
        {
            var equipo = await _context.Equipos
                .Include(e => e.CreadoPorCliente)
                .Include(e => e.Formaciones)
                    .ThenInclude(f => f.JugadoresPosiciones)
                .FirstOrDefaultAsync(e => e.Id == idEquipo);

            return equipo;
        }

        public async Task<IEnumerable<Equipo>> GetListEquipos()
        {
            return await _context.Equipos
                .Include(e => e.CreadoPorCliente)
                .ToListAsync();
        }

        public async Task<IEnumerable<Equipo>> GetEquiposByClienteId(int clienteId)
        {
            return await _context.Equipos
                .Include(e => e.CreadoPorCliente)
                .Where(e => e.CreadoPorClienteId == clienteId)
                .ToListAsync();
        }
    }
}
