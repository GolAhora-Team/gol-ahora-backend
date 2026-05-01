using Aplication.Interfaces.ICliente;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Command
{
    public class ClientesCommand : IClientesCommand
    {
        private readonly AppDbContext _context;

        public ClientesCommand(AppDbContext context)
        {
            _context = context;
        }

        public async Task InsertCliente(Cliente cliente)
        {
            _context.Add(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCliente(Cliente cliente)
        {
            _context.Update(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveCliente(int idCliente)
        {
            var cliente = await _context.Clientes.FindAsync(idCliente);

            if (cliente != null)
            {
                _context.Remove(cliente); 
                await _context.SaveChangesAsync();
            }
        }       

    }
}
