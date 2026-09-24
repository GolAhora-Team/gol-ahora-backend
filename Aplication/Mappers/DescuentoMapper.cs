using Aplication.DTOs.Request.Descuento;
using Aplication.Interfaces.IDescuento;
using Aplication.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Mappers
{
    public class DescuentoMapper : IDescuentoMapper
    {
        public Descuento CreateDescuento(CreateDescuentoRequest request)
        {
            return new Descuento
            {
                Nombre = request.Nombre,
                Descripcion = request.Descripcion,
                Porcentaje = request.Porcentaje,
                FechaInicio = request.FechaInicio,
                FechaFin = request.FechaFin
            };
        }

        public DescuentoResponse CreateDescuentoResponse(Descuento descuento)
        {
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
        public void UpdateDescuento(Descuento descuento, UpdateDescuentoRequest request)
        {
            descuento.Nombre = request.Nombre;
            descuento.Descripcion = request.Descripcion;
            descuento.Porcentaje = request.Porcentaje;
            descuento.FechaInicio = request.FechaInicio;
            descuento.FechaFin = request.FechaFin;
        }

        public List<DescuentoResponse> CreateDescuentoResponseList(List<Descuento> descuentos)
        {
            return descuentos.Select(d => new DescuentoResponse
            {
                Id = d.Id,
                Nombre = d.Nombre,
                Descripcion = d.Descripcion,
                Porcentaje = d.Porcentaje,
                FechaInicio = d.FechaInicio,
                FechaFin = d.FechaFin
            }).ToList();
        }
    }
}
