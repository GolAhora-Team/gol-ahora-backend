using Aplication.DTOs.Request.Descuento;
using Aplication.DTOs.Request.Pago;
using Aplication.DTOs.Response.Pago;
using Aplication.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IPago
{
    public interface IPagoMapper
    {
        Pago CreatePago(CreatePagoRequest request);
        PagoResponse CreatePagoResponse(Pago pago);

        // UPDATE
        void UpdatePago(Pago pago, UpdatePagoRequest request);

        // LIST
        List<PagoResponse> CreatePagoResponseList(List<Pago> pago);
    }
}
