using Aplication.DTOs.Request.Cliente;
using Aplication.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.ICliente
{
    public interface IClienteMapper
    {
        ClienteResponse CreateClienteResponse(Cliente cliente);
        Cliente CreateCliente(CreateClienteRequest cliente);
    }
}
