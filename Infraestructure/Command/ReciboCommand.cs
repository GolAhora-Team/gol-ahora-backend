using Aplication.Interfaces.IRecibo;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Command
{
    public class ReciboCommand : IReciboCommand
    {
        private readonly AppDbContext _context;

        public ReciboCommand(AppDbContext context)
        {
            _context = context;
        }

        public async Task InsertRecibo(Recibo recibo)
        {
            _context.Recibos.Add(recibo);
            await _context.SaveChangesAsync();
        }
    }
}
