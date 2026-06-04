using Aplication.DTOs.Request.Equipo;
using Aplication.DTOs.Response;
using Aplication.Interfaces.IEquipo;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Mappers
{
    public class EquipoMapper : IEquipoMapper
    {        
        public Equipo CreateEquipo(CreateEquipoRequest equipo)
        {
            return new Equipo
            {                
                Nombre = equipo.Nombre,
                CantidadMaxJugadores = equipo.CantidadMaxJugadores,
                Descripcion = equipo.Descripcion,
                ColorPrimario = equipo.ColorPrimario ?? "#ffffff",
                ColorSecundario = equipo.ColorSecundario ?? "#ffffff",
                CompeticionId = equipo.CompeticionId,
                CreadoPorClienteId = equipo.CreadoPorClienteId
            };
        }

        public EquipoResponse CreateEquipoResponse(Equipo equipo)
        {
            return new EquipoResponse
            {
                Id = equipo.Id,
                Nombre = equipo.Nombre,
                CantidadMaxJugadores = equipo.CantidadMaxJugadores,
                Descripcion = equipo.Descripcion,
                ColorPrimario = equipo.ColorPrimario ?? "#ffffff",
                ColorSecundario = equipo.ColorSecundario ?? "#ffffff",
                CompeticionId = equipo.CompeticionId,
                CreadoPorClienteId = equipo.CreadoPorClienteId,
                CreadoPorNombre = equipo.CreadoPorCliente != null ? $"{equipo.CreadoPorCliente.Nombre} {equipo.CreadoPorCliente.Apellido}" : null,
                Formaciones = equipo.Formaciones?.Select(f => new EquipoFormacionResponse
                {
                    Id = f.Id,
                    TipoCancha = f.TipoCancha,
                    FormacionDefecto = f.FormacionDefecto,
                    Jugadores = f.JugadoresPosiciones?.Select(jp => new JugadorFormacionResponse
                    {
                        JugadorId = jp.JugadorId,
                        EsTitular = jp.EsTitular,
                        Posicion = jp.Posicion,
                        EsCapitan = jp.EsCapitan
                    }).ToList() ?? new List<JugadorFormacionResponse>()
                }).ToList() ?? new List<EquipoFormacionResponse>()
            };
        }
    }
}
