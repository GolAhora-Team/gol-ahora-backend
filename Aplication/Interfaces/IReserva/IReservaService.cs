using Aplication.DTOs.Request.Reserva;
using Aplication.DTOs.Response.Reserva;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IReserva
{
    public interface IReservaService
    {
        Task<CreateReservaResponse> CrearReserva(CreateReservaRequest request);

        Task<ReservaResponse> GetReservaById(int id);

        Task<List<ReservaResponse>> GetAllReservas();

        Task<CancelacionInfoResponse> GetCancelacionInfo(int reservaId);

        Task<ReservaResponse> CancelarReserva(int id);

        Task<ReservaResponse> ModificarReserva(int id, UpdateReservaRequest request);
    }
}
