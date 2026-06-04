using Aplication.DTOs.Request.Cancha;
using Aplication.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplication.Interfaces.ICancha
{
    public interface ICanchaService
    {
        Task<CanchaResponse> CreateCancha(CreateCanchaRequest request);
        Task<CanchaResponse> UpdateCancha(int idCancha, UpdateCanchaRequest request);
        Task DeleteCancha(int idCancha);
        Task<CanchaResponse> GetCanchaById(int idCancha);
        Task<List<CanchaResponse>> GetAll();
        Task<List<CanchaResponse>> GetCanchasActivas();
        Task<List<CanchaResponse>> GetCanchasDisponibles(DateTime fecha, TimeSpan hora);
        Task UpdatePreciosGlobal(UpdatePreciosRequest request);
    }
}
