using Aplication.Interfaces.IRecibo;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Querys
{
    public class ReciboQuery : IReciboQuery
    {
        private readonly AppDbContext _context;

        public ReciboQuery(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetLastReciboId()
        {
            return await _context.Recibos
                .OrderByDescending(r => r.Id)
                .Select(r => r.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<Recibo> GetReciboByReservaId(int reservaId)
        {
            return await _context.Recibos
                .Include(r => r.Reserva)
                .ThenInclude(res => res.Cliente)
                .Include(r => r.Reserva.Cancha)
                .FirstOrDefaultAsync(r => r.ReservaId == reservaId);
        }
    }
}
