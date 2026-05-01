using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.ICliente
{
    public interface IClientesCommand
    {
        Task InsertCliente(Cliente cliente);
        Task UpdateCliente(Cliente cliente);
        Task RemoveCliente(int idCliente);
        
    }
}
