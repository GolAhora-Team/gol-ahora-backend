using Aplication.DTOs.Request.Descuento;
using Aplication.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IDescuento
{
    public interface IDescuentoMapper
    {
        Descuento CreateDescuento(CreateDescuentoRequest request);
        DescuentoResponse CreateDescuentoResponse(Descuento descuento);
        // UPDATE
        void UpdateDescuento(Descuento descuento, UpdateDescuentoRequest request);

        // LIST
        List<DescuentoResponse> CreateDescuentoResponseList(List<Descuento> descuentos);
    }
}
