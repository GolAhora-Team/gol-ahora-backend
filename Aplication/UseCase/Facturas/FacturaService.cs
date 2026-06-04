using Aplication.Interfaces.IFactura;
using Aplication.DTOs.Request.Factura;
using Aplication.DTOs.Response.Factura;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.UseCase.Facturas
{
    public class FacturaService : IFacturaService
    {
        private readonly IFacturaCommand _command;
        private readonly IFacturaQuery _query;
        private readonly IFacturaMapper _mapper;

        public FacturaService(IFacturaCommand command, IFacturaQuery query, IFacturaMapper mapper)
        {
            _command = command;
            _query = query;
            _mapper = mapper;
        }

        // 🟢 CREATE
        public async Task<FacturaResponse> CreateFactura(CreateFacturaRequest request)
        {
            var factura = _mapper.CreateFactura(request);

            await _command.InsertFactura(factura);

            factura = await _query.GetFacturaById(factura.Id);

            return _mapper.CreateFacturaResponse(factura);
        }

        // 🔵 GET ALL
        public async Task<List<FacturaResponse>> GetAll()
        {
            var facturas = await _query.GetListFacturas();

            return _mapper.CreateFacturaResponseList(facturas);
        }

        // 🔵 GET BY ID
        public async Task<FacturaResponse> GetById(int id)
        {
            var factura = await _query.GetFacturaById(id);

            if (factura == null)
                throw new Exception("La factura no existe");

            return _mapper.CreateFacturaResponse(factura);
        }

        // 🟡 UPDATE
        public async Task<FacturaResponse> UpdateFactura(int id, UpdateFacturaRequest request)
        {
            var factura = await _query.GetFacturaById(id);

            if (factura == null)
                throw new Exception("La factura no existe");

            _mapper.UpdateFactura(factura, request);

            await _command.UpdateFactura(factura);

            return _mapper.CreateFacturaResponse(factura);
        }

        // 🔴 DELETE
        public async Task DeleteFactura(int id)
        {
            await _command.RemoveFactura(id);
        }

        // 🟣 GET BY CLIENTE
        public async Task<List<FacturaResponse>> GetFacturasByClienteId(int clienteId)
        {
            var facturas = await _query.GetFacturasByClienteId(clienteId);
            return _mapper.CreateFacturaResponseList(facturas);
        }
    }
}
