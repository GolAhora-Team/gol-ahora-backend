using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.ICliente
{
    public interface IClienteQuery
    {
        List<Cliente> GetListClientes();
        Cliente GetCliente(int id);
    }
}
