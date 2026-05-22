using Aplication.Interfaces.IPago;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Querys
{
    public class PagoQuery : IPagoQuery
    {
        private readonly AppDbContext _context;

        public PagoQuery(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Pago>> GetListPagos()
        {
            return await _context.Pagos.ToListAsync();
        }

        public async Task<Pago?> GetPagoById(int id)
        {
            return await _context.Pagos
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
