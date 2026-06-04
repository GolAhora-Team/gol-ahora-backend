using Aplication.DTOs.Request.Factura;
using Aplication.DTOs.Response.Factura;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IFactura
{
    public interface IFacturaService
    {
        Task<List<FacturaResponse>> GetAll();

        Task<FacturaResponse> GetById(int id);

        Task<FacturaResponse> CreateFactura(CreateFacturaRequest request);

        Task<FacturaResponse> UpdateFactura(int id, UpdateFacturaRequest request);

        Task DeleteFactura(int id);

        Task<List<FacturaResponse>> GetFacturasByClienteId(int clienteId);
    }
}
