using Aplication.DTOs.Request.Jugador;
using Aplication.DTOs.Response;
using Aplication.Interfaces.IJugador;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Mappers
{
    public class JugadorMapper : IJugadorMapper
    {
        public Jugador CreateJugador(JugadorRequest jugador)
        {
            return new Jugador
            {
                Numero = jugador.Numero,
                Posicion = jugador.Posicion,
                ClienteId = jugador.ClienteId,
                EquipoId = jugador.EquipoId,
                EquipoNombre = ""
            };
        }

        public JugadorResponse CreateJugadorResponse(Jugador jugador)
        {
            return new JugadorResponse
            {
                Id = jugador.Id,
                Numero = jugador.Numero,
                EsCapitan = jugador.EsCapitan,
                EsTitular = jugador.EsTitular,
                Posicion = jugador.Posicion,
                Estado = jugador.Estado,
                ClienteId = jugador.ClienteId,
                EquipoId = jugador.EquipoId,
                EquipoNombre = jugador.EquipoNombre,
                Sanciones = jugador.Sanciones?.Select(s => new SancionResponse
                {
                    Id = s.Id,
                    Fecha = s.Fecha,
                    Tarjeta = s.Tarjeta,
                    JugadorId = s.JugadorId
                }).ToList() ?? new List<SancionResponse>()
            };
        }
    }
}

