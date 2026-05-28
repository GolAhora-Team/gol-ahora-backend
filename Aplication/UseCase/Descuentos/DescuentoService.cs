using Aplication.Interfaces.IDescuento;
using Aplication.Response;
using Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplication.DTOs.Request.Descuento;

namespace Aplication.UseCase.Descuentos
{
    public class DescuentoService : IDescuentoService
    {
        private readonly IDescuentoCommand _command;
        private readonly IDescuentoQuery _query;
        private readonly IDescuentoMapper _mapper;

        public DescuentoService(IDescuentoCommand command, IDescuentoQuery query, IDescuentoMapper mapper)
        {
            _command = command;
            _query = query;
            _mapper = mapper;
        }

        // 🟢 CREATE
        public async Task<DescuentoResponse> CreateDescuento(CreateDescuentoRequest request)
        {
            var descuento = _mapper.CreateDescuento(request);

            await _command.InsertDescuento(descuento);
            descuento = await _query.GetDescuentoById(descuento.Id);
            return _mapper.CreateDescuentoResponse(descuento);
        }

        // 🔵 GET ALL
        public async Task<List<DescuentoResponse>> GetAll()
        {
            var descuentos = await _query.GetListDescuentos();

            return _mapper.CreateDescuentoResponseList(descuentos);
        }

        // 🔵 GET BY ID
        public async Task<DescuentoResponse> GetById(int id)
        {
            var d = await _query.GetDescuentoById(id);

            if (d == null)
                throw new Exception("El descuento no existe");

            return _mapper.CreateDescuentoResponse(d);
        }

        // 🟡 UPDATE
        public async Task<DescuentoResponse> UpdateDescuento(int id, UpdateDescuentoRequest request)
        {
            var d = await _query.GetDescuentoById(id);

            if (d == null)
                throw new Exception("El descuento no existe");

            _mapper.UpdateDescuento(d, request);

            await _command.UpdateDescuento(d);

            return _mapper.CreateDescuentoResponse(d);
        }

        // 🔴 DELETE
        public async Task DeleteDescuento(int id)
        {
            await _command.RemoveDescuento(id);
        }
    }
}
