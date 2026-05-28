using Domain.Entities;
using Aplication.Interfaces.IFactura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Command
{
    public class FacturaCommand : IFacturaCommand
    {
        private readonly AppDbContext _context;

        public FacturaCommand(AppDbContext context)
        {
            _context = context;
        }

        public async Task InsertFactura(Factura factura)
        {
            _context.Facturas.Add(factura);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFactura(Factura factura)
        {
            _context.Facturas.Update(factura);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFactura(int id)
        {
            var factura = await _context.Facturas.FindAsync(id);

            if (factura != null)
            {
                _context.Facturas.Remove(factura);
                await _context.SaveChangesAsync();
            }
        }
    }
}
