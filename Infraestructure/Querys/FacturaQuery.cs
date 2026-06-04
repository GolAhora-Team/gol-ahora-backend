using Domain.Entities;
using Aplication.Interfaces.IFactura;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Querys
{
    public class FacturaQuery : IFacturaQuery
    {
        private readonly AppDbContext _context;

        public FacturaQuery(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Factura>> GetListFacturas()
        {
            return await _context.Facturas.ToListAsync();
        }

        public async Task<Factura?> GetFacturaById(int id)
        {
            return await _context.Facturas
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Factura>> GetFacturasByClienteId(int clienteId)
        {
            return await _context.Facturas
                .Where(f => f.ClienteId == clienteId)
                .OrderByDescending(f => f.FechaEmision)
                .ToListAsync();
        }
    }
}
