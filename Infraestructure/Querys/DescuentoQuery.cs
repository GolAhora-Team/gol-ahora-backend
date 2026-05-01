using Aplication.Interfaces.IDescuento;
using Aplication.Interfaces.ICliente;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Querys
{
    public class DescuentoQuery : IDescuentoQuery
    {
        private readonly AppDbContext _context;

        public DescuentoQuery(AppDbContext context)
        {
            _context = context;
        }

        //  GET ALL
        public async Task<List<Descuento>> GetListDescuentos()
        {
            return await _context.Descuentos.ToListAsync();
        }

        //  GET BY ID
        public async Task<Descuento?> GetDescuentoById(int id)
        {
            return await _context.Descuentos
                .FirstOrDefaultAsync(d => d.Id == id);
        }
    }
}
