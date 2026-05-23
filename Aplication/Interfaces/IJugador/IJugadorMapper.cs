using Aplication.DTOs.Request.Cliente;
using Aplication.DTOs.Request.Jugador;
using Aplication.DTOs.Response;
using Aplication.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IJugador
{
    public interface IJugadorMapper
    {
        JugadorResponse CreateJugadorResponse(Jugador jugador);
        Jugador CreateJugador(JugadorRequest jugador);
    }
}
