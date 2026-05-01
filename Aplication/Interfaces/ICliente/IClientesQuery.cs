using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.ICliente
{
    public interface IClientesQuery
    {
        Task<Cliente> GetClienteById(int idCliente);
        Task<IEnumerable<Cliente>> GetListClientes();
    }
}
