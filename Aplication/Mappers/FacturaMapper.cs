using Aplication.DTOs.Request.Factura;
using Aplication.DTOs.Response.Factura;
using Aplication.Interfaces.IFactura;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Mappers
{
    public class FacturaMapper : IFacturaMapper
    {
        // CREATE
        public Factura CreateFactura(CreateFacturaRequest request)
        {
            return new Factura
            {
                FechaEmision = request.FechaEmision,
                Total = request.Total,
                ClienteId = request.ClienteId
            };
        }

        // RESPONSE
        public FacturaResponse CreateFacturaResponse(Factura factura)
        {
            return new FacturaResponse
            {
                Id = factura.Id,
                FechaEmision = factura.FechaEmision,
                Total = factura.Total,
                ClienteId = factura.ClienteId
            };
        }

        // UPDATE
        public void UpdateFactura(Factura factura, UpdateFacturaRequest request)
        {
            factura.FechaEmision = request.FechaEmision;
            factura.Total = request.Total;
            factura.ClienteId = request.ClienteId;
        }

        // LIST
        public List<FacturaResponse> CreateFacturaResponseList(List<Factura> facturas)
        {
            return facturas.Select(f => new FacturaResponse
            {
                Id = f.Id,
                FechaEmision = f.FechaEmision,
                Total = f.Total,
                ClienteId = f.ClienteId
            }).ToList();
        }
    }
}
