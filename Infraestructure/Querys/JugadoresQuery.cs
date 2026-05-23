using Aplication.DTOs;
using Aplication.Interfaces.IJugador;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Querys
{
    public class JugadoresQuery : IJugadorQuery
    {
        private readonly AppDbContext _context;
        public JugadoresQuery(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Jugador> GetJugadorById(int idJugador)
        {
            var jugador = await _context.Jugadores.FindAsync(idJugador);
            return jugador;
        }

        public async Task<IEnumerable<Jugador>> GetListJugadores()
        {
            return await _context.Jugadores.ToListAsync();
        }
    }
}
