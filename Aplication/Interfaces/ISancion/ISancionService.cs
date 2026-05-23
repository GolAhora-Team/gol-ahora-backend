using Aplication.DTOs.Request.Jugador;
using Aplication.DTOs.Request.Sancion;
using Aplication.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.ISancion
{
    public interface ISancionService
    {
        Task<SancionResponse> CreateSancion(SancionRequest request);
        Task<SancionResponse> DeleteSancion(int sancionId);
        Task<List<SancionResponse>> GetSancionesPorJugador(int jugadorId);
    }
}
