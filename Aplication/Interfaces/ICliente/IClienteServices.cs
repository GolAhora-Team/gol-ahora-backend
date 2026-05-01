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
    public interface IClienteServices
    {
        Task<ClienteResponse> CreateCliente(CreateClienteRequest request);        
        Task<ClienteResponse> UpdateCliente(int clienteId, UpdateClienteRequest request);
        Task<ClienteResponse> DeleteCliente(int clienteId);
        Task<List<ClienteResponse>> GetAll();
        Task<ClienteResponse> GetClienteById(int clienteId);
    }
}
