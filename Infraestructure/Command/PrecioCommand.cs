using Aplication.Interfaces.IPrecio;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Command
{
    public class PrecioCommand : IPrecioCommand
    {
        private readonly AppDbContext _context;

        public PrecioCommand(AppDbContext context)
        {
            _context = context;
        }

        public async Task InsertPrecio(Precio precio)
        {
            _context.Add(precio);
            await _context.SaveChangesAsync();
        }

        public async Task RemovePrecio(int idPrecio)
        {
            var precio = await _context.Precios.FindAsync(idPrecio);
            if(precio != null)
            {
                _context.Remove(precio);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdatePrecio(Precio precio)
        {
            _context.Update(precio);
            await _context.SaveChangesAsync();
        }
    }
}
