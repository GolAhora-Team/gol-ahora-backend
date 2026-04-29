using Aplication.Interfaces.IDescuento;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Command
{
    public class DescuentoCommand : IDescuentoCommand
    {
        private readonly AppDbContext _context;

        public DescuentoCommand(AppDbContext context)
        {
            _context = context;
        }

        //  INSERT
        public async Task InsertDescuento(Descuento descuento)
        {
            _context.Descuentos.Add(descuento);
            await _context.SaveChangesAsync();
        }

        // UPDATE
        public async Task UpdateDescuento(Descuento descuento)
        {
            _context.Descuentos.Update(descuento);
            await _context.SaveChangesAsync();
        }

        // DELETE
        public async Task RemoveDescuento(int id)
        {
            var descuento = await _context.Descuentos.FindAsync(id);

            if (descuento != null)
            {
                _context.Descuentos.Remove(descuento);
                await _context.SaveChangesAsync();
            }
        }
    }
}
