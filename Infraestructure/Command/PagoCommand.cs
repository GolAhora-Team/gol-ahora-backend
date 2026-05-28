using Aplication.Interfaces.IPago;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Command
{
    public class PagoCommand : IPagoCommand
    {
        private readonly AppDbContext _context;

        public PagoCommand(AppDbContext context)
        {
            _context = context;
        }

        public async Task InsertPago(Pago pago)
        {
            _context.Pagos.Add(pago);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePago(Pago pago)
        {
            _context.Pagos.Update(pago);
            await _context.SaveChangesAsync();
        }

        public async Task RemovePago(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);

            if (pago != null)
            {
                _context.Pagos.Remove(pago);
                await _context.SaveChangesAsync();
            }
        }
    }
}
