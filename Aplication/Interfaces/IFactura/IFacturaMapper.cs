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
    public interface IFacturaMapper
    {
        // crear
        Factura CreateFactura(CreateFacturaRequest request);

        // response
        FacturaResponse CreateFacturaResponse(Factura factura);

        // modificar
        void UpdateFactura(Factura factura, UpdateFacturaRequest request);

        // listar
        List<FacturaResponse> CreateFacturaResponseList(List<Factura> facturas);
    }
}
