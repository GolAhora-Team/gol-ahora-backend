using Aplication.DTOs;
using Aplication.DTOs.Request.Cliente;
using Aplication.DTOs.Request.Jugador;
using Aplication.DTOs.Response;
using Aplication.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IJugador
{
    public interface IJugadorService
    {
        Task<JugadorResponse> CreateJugador(JugadorRequest request);
        Task<JugadorResponse> UpdateJugador(int jugadorId, JugadorRequest request);
        Task<JugadorResponse> DeleteJugador(int jugadorId);
        Task<List<JugadorResponse>> GetAll();
        Task<JugadorResponse> GetJugadorById(int jugadorId);
    }
}
