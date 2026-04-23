using Aplication.Interfaces.ICliente;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Querys
{
    public class ClienteQuery : IClienteQuery
    {
        public Cliente GetCliente(int id)
        {
            throw new NotImplementedException();
        }

        public List<Cliente> GetListClientes()
        {
            throw new NotImplementedException();
        }
    }
}
