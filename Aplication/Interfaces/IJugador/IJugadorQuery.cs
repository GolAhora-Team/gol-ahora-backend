using Aplication.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IJugador
{
    public interface IJugadorQuery
    {
        Task<Jugador> GetJugadorById(int idJugador);
        Task<IEnumerable<Jugador>> GetListJugadores();
    }
}
