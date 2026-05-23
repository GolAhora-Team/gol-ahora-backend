using Aplication.Interfaces.ISancion;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Command
{
    public class SancionCommand : ISancionCommand
    {
        private readonly AppDbContext _context;

        public SancionCommand(AppDbContext context)
        {
            _context = context;
        }

        public async Task InsertSancion(Sancion sancion)
        {
            _context.Add(sancion);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveSancion(int idSancion)
        {
            var sancion = await _context.Sanciones.FindAsync(idSancion);
            if(sancion != null)
            {
                _context.Remove(sancion);
                await _context.SaveChangesAsync();
            }
        }
    }
}
