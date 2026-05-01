using Aplication.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplication.DTOs.Request.Descuento;


namespace Aplication.Interfaces.IDescuento
{
    public interface IDescuentoService
    {
        Task<DescuentoResponse> CreateDescuento(CreateDescuentoRequest request);
        Task<DescuentoResponse> UpdateDescuento(int id, UpdateDescuentoRequest request);
        Task DeleteDescuento(int id);
        Task<List<DescuentoResponse>> GetAll();
        Task<DescuentoResponse> GetById(int id);
    }
}
