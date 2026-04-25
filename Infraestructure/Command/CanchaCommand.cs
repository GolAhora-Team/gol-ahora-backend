using Aplication.Interfaces.ICancha;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Command
{
    public class CanchaCommand : ICanchaCommand
    {
        private readonly AppDbContext _context;

        public CanchaCommand(AppDbContext context)
        {
            _context = context;
        }

        public async Task InsertCancha(Cancha cancha)
        {
            _context.Add(cancha);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveCancha(int idCancha)
        {
            var cancha = await _context.Canchas.FindAsync(idCancha);
            if(cancha != null)
            {
                _context.Remove(cancha);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateCancha(Cancha cancha)
        {
            _context.Update(cancha);
            await _context.SaveChangesAsync();
        }
    }
}
