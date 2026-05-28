using Aplication.DTOs.Request.Factura;
using Aplication.DTOs.Request.Pago;
using Aplication.DTOs.Response.Factura;
using Aplication.DTOs.Response.Pago;
using Aplication.Interfaces.IPago;
using Aplication.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Mappers
{
    public class PagoMapper : IPagoMapper
    {
        public Pago CreatePago(CreatePagoRequest request)
        {
            return new Pago
            {
                    FechaPago = request.FechaPago,
                    Monto = request.Monto,
                    Metodo = request.Metodo,
                    Estado = request.Estado,
                    FacturaId = request.FacturaId
                };

         }

        public PagoResponse CreatePagoResponse(Pago pago)
        {
            return new PagoResponse
            {

                Id = pago.Id,
                FechaPago = pago.FechaPago,
                Monto = pago.Monto,
                Metodo = pago.Metodo,
                Estado = pago.Estado,
                FacturaId = pago.FacturaId
            };
        }
        // UPDATE
        public void UpdatePago(Pago pago, UpdatePagoRequest request)
        {
            pago.FechaPago = request.FechaPago;
            pago.Monto = request.Monto;
            pago.Metodo = request.Metodo;
            pago.Estado = request.Estado;
            pago.FacturaId = request.FacturaId;
                }

        // LIST
        public List<PagoResponse> CreatePagoResponseList(List<Pago> pago)
        {
            return pago.Select(pag => new PagoResponse
            {
                Id = pag.Id,
                FechaPago = pag.FechaPago,
                Monto = pag.Monto,
                Metodo = pag.Metodo,
                Estado = pag.Estado,
                FacturaId = pag.FacturaId
            }).ToList();
        }
    }
}
