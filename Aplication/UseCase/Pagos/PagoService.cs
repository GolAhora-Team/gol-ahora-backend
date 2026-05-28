using Aplication.DTOs.Request.Pago;
using Aplication.DTOs.Response.Pago;
using Aplication.Interfaces.IDescuento;
using Aplication.Interfaces.IPago;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.UseCase.Pagos
{
    public class PagoService : IPagoService
    {
        private readonly IPagoCommand _command;
        private readonly IPagoQuery _query;
        private readonly IPagoMapper _mapper;

        public PagoService(IPagoCommand command, IPagoQuery query, IPagoMapper mapper)
        {
            _command = command;
            _query = query;
            _mapper = mapper;
        }

        //  CREATE
        public async Task<PagoResponse> CreatePago(CreatePagoRequest request)
        {
            var pago = _mapper.CreatePago(request);

            await _command.InsertPago(pago);
            pago = await _query.GetPagoById(pago.Id);
            return _mapper.CreatePagoResponse(pago);
        }

        //  GET ALL
        public async Task<List<PagoResponse>> GetAll()
        {
            var pago = await _query.GetListPagos();

            return _mapper.CreatePagoResponseList(pago);
        }

        //  GET BY ID
        public async Task<PagoResponse> GetById(int id)
        {
            var d = await _query.GetPagoById(id);

            if (d == null)
                throw new Exception("El pago no existe");

            return _mapper.CreatePagoResponse(d);
        }

        //  UPDATE
        public async Task<PagoResponse> UpdatePago(int id, UpdatePagoRequest request)
        {
            var d = await _query.GetPagoById(id);

            if (d == null)
                throw new Exception("ERROR CHE: El pago no existe");

            _mapper.UpdatePago(d, request);

            await _command.UpdatePago(d);

            return _mapper.CreatePagoResponse(d);
        }

        //  DELETE
        public async Task DeletePago(int id)
        {
            await _command.RemovePago(id);
        }
    }
}
