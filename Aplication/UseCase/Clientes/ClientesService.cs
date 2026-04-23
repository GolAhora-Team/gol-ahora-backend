using Aplication.Interfaces.ICliente;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.UseCase.Clientes
{
    public class ClientesService : IClienteServices
    {
        private readonly IClientesCommand _command;
        private readonly IClientesCommand _query;

        public ClientesService(IClientesCommand command, IClientesCommand query)
        {
            _command = command;
            _query = query;
        }

        public Task<Cliente> CreateCliente()
        {

            throw new NotImplementedException();
        }

        public Task<Cliente> DeleteCliente()
        {
            throw new NotImplementedException();
        }

        public Task<List<Cliente>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Cliente> UpdateCliente(int clienteId)
        {
            throw new NotImplementedException();
        }
    }
}
