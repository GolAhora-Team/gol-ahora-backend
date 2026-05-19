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
                CompeticionId = equipo.CompeticionId
            };
        }

        public EquipoResponse CreateEquipoResponse(Equipo equipo)
        {
            return new EquipoResponse
            {
                Nombre = equipo.Nombre,
                CantidadMaxJugadores = equipo.CantidadMaxJugadores,
                Descripcion = equipo.Descripcion,
                CompeticionId = equipo.CompeticionId
            };
        }
    }
}
