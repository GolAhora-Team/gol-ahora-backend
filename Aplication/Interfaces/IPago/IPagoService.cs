using Aplication.DTOs.Request.Pago;
using Aplication.DTOs.Response.Pago;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IPago
{

    public interface IPagoService
    {
        Task<List<PagoResponse>> GetAll();

        Task<PagoResponse> GetById(int id);

        Task<PagoResponse> CreatePago(CreatePagoRequest request);

        Task<PagoResponse> UpdatePago(int id, UpdatePagoRequest request);

        Task DeletePago(int id);
    }


}
