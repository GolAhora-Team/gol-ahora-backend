using Aplication.DTOs;
using Aplication.Interfaces.IJugador;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Command
{
    public class JugadoresCommand : IJugadorCommand
    {
        private readonly AppDbContext _context;
        public JugadoresCommand(AppDbContext context)
        {
            _context = context;
        }

        public async Task InsertJugador(Jugador jugador)
        {
            _context.Add(jugador);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveJugador(int idJugador)
        {
            var jugador = await _context.Jugadores.FindAsync(idJugador);
            if(jugador != null)
            {
                _context.Remove(jugador);
                await _context.SaveChangesAsync();
            }            
        }

        public async Task UpdateJugador(Jugador jugador)
        {
            _context.Update(jugador);
            await _context.SaveChangesAsync();
        }
    }
}
