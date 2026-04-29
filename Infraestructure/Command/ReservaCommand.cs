using Aplication.Interfaces.IReserva;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Command
{
    public class ReservaCommand: IReservaCommand
    {
        private readonly AppDbContext _context;

        public ReservaCommand(AppDbContext context)
        {
            _context = context;
        }

        public async Task InsertReserva(Reserva reserva)
        {
            await _context.Reservas.AddAsync(reserva);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveReserva(Reserva idReserva)
        {
            _context.Reservas.Remove(idReserva);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateReserva(Reserva reserva)
        {
            _context.Reservas.Update(reserva);
            await _context.SaveChangesAsync();
        }
    }
}
