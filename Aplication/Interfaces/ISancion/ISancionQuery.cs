using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.ISancion
{
    public interface ISancionQuery
    {
        Task<Sancion> GetSancionById(int sancionId);
        Task<List<Sancion>> GetSancionesPorJugador(int jugadorId);
    }
}
