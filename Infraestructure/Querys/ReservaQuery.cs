using Aplication.Interfaces.IReserva;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Querys
{
    public class ReservaQuery : IReservaQuery
    {
        private readonly AppDbContext _context;
        public ReservaQuery(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Domain.Entities.Reserva>> GetAllReservas()
        {
            return await _context.Reservas.ToListAsync();
        }
        public async Task<Domain.Entities.Reserva?> GetReservaById(int id)
        {
            return await _context.Reservas.FirstOrDefaultAsync(r => r.Id == id);
        }
        public async Task<bool> ReservaExists(int id)
        {
            return await _context.Reservas.AnyAsync(r => r.Id == id);
        }
    }
}
