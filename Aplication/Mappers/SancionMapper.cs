using Aplication.DTOs.Request.Sancion;
using Aplication.DTOs.Response;
using Aplication.Interfaces.ISancion;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Mappers
{
    public class SancionMapper : ISancionMapper
    {
        public Sancion CreateSancion(SancionRequest sancion)
        {
            return new Sancion
            {
                Fecha = sancion.Fecha,
                Tarjeta = sancion.Tarjeta,
                JugadorId = sancion.JugadorId
            };           
        }

        public SancionResponse CreateSancionResponse(Sancion sancion)
        {
            return new SancionResponse
            {
                Id = sancion.Id,
                Fecha = sancion.Fecha,
                Tarjeta = sancion.Tarjeta,
                JugadorId = sancion.JugadorId
            };
        }
    }
}
