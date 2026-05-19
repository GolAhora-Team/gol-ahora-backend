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
    public class ClientesQuery : IClientesQuery
    {
        private readonly AppDbContext _context;

        public ClientesQuery(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Cliente> GetClienteById(int idCliente)
        {
            var cliente = await _context.Clientes.FindAsync(idCliente);

            return cliente;
        }

        public async Task<IEnumerable<Cliente>> GetListClientes()
        {
            return await _context.Clientes.ToListAsync();
        }
    }
}
