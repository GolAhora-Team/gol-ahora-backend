using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.ICliente
{
    public interface IClienteServices
    {
        Task<Cliente> CreateCliente();
        Task<Cliente> DeleteCliente();
        Task<Cliente> UpdateCliente(int clienteId);
        Task<List<Cliente>> GetAll();
    }
}
