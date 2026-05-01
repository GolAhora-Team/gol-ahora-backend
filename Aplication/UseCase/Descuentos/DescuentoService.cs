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

        public DescuentoService(IDescuentoCommand command, IDescuentoQuery query)
        {
            _command = command;
            _query = query;
        }

        // 🟢 CREATE
        public async Task<DescuentoResponse> CreateDescuento(CreateDescuentoRequest request)
        {
            var descuento = new Descuento
            {
                Nombre = request.Nombre,
                Descripcion = request.Descripcion,
                Porcentaje = request.Porcentaje,
                FechaInicio = request.FechaInicio,
                FechaFin = request.FechaFin
            };

            await _command.InsertDescuento(descuento);

            return new DescuentoResponse
            {
                Id = descuento.Id,
                Nombre = descuento.Nombre,
                Descripcion = descuento.Descripcion,
                Porcentaje = descuento.Porcentaje,
                FechaInicio = descuento.FechaInicio,
                FechaFin = descuento.FechaFin
            };
        }

        // 🔵 GET ALL
        public async Task<List<DescuentoResponse>> GetAll()
        {
            var descuentos = await _query.GetListDescuentos();

            var responseList = new List<DescuentoResponse>();

            foreach (var d in descuentos)
            {
                responseList.Add(new DescuentoResponse
                {
                    Id = d.Id,
                    Nombre = d.Nombre,
                    Descripcion = d.Descripcion,
                    Porcentaje = d.Porcentaje,
                    FechaInicio = d.FechaInicio,
                    FechaFin = d.FechaFin
                });
            }

            return responseList;
        }

        // 🔵 GET BY ID
        public async Task<DescuentoResponse> GetById(int id)
        {
            var d = await _query.GetDescuentoById(id);

            if (d == null)
                throw new Exception("El descuento no existe");

            return new DescuentoResponse
            {
                Id = d.Id,
                Nombre = d.Nombre,
                Descripcion = d.Descripcion,
                Porcentaje = d.Porcentaje,
                FechaInicio = d.FechaInicio,
                FechaFin = d.FechaFin
            };
        }

        // 🟡 UPDATE
        public async Task<DescuentoResponse> UpdateDescuento(int id, UpdateDescuentoRequest request)
        {
            var d = await _query.GetDescuentoById(id);

            if (d == null)
                throw new Exception("El descuento no existe");

            d.Nombre = request.Nombre;
            d.Descripcion = request.Descripcion;
            d.Porcentaje = request.Porcentaje;
            d.FechaInicio = request.FechaInicio;
            d.FechaFin = request.FechaFin;

            await _command.UpdateDescuento(d);

            return new DescuentoResponse
            {
                Id = d.Id,
                Nombre = d.Nombre,
                Descripcion = d.Descripcion,
                Porcentaje = d.Porcentaje,
                FechaInicio = d.FechaInicio,
                FechaFin = d.FechaFin
            };
        }

        // 🔴 DELETE
        public async Task DeleteDescuento(int id)
        {
            await _command.RemoveDescuento(id);
        }
    }
}
