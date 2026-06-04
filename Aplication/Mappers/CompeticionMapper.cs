using Aplication.DTOs.Request.Competición;
using Aplication.DTOs.Request.Equipo;
using Aplication.DTOs.Response;
using Aplication.Interfaces.ICompeticion;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Mappers
{
    public class CompeticionMapper : ICompeticionMapper
    {
        public Competicion CreateCompeticion(CompeticionRequest competicion)
        {
            return new Competicion
            {
                Nombre = competicion.Nombre,
                Tipo = competicion.Tipo,
                Descripcion = competicion.Descripcion,                
                CantidadEquipos = competicion.CantidadEquipos,
                FechaInicio = competicion.FechaInicio,
                FechaFin = competicion.FechaFin,
                TipoCancha = competicion.TipoCancha,
                FixtureGenerado = competicion.FixtureGenerado
            };
        }

        public CompeticionResponse CreateCompeticionResponse(Competicion competicion)
        {
            return new CompeticionResponse
            {
                Id = competicion.Id,
                Nombre = competicion.Nombre,
                Tipo = competicion.Tipo,
                Descripcion = competicion.Descripcion,
                CantidadEquipos = competicion.CantidadEquipos,
                Estado = competicion.Estado,
                CantInscriptos = competicion.Equipos?.Count ?? 0,
                FechaInicio = competicion.FechaInicio,
                FechaFin = competicion.FechaFin,
                TipoCancha = competicion.TipoCancha,
                FixtureGenerado = competicion.FixtureGenerado
            };
        }
    }
}
