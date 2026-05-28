using Aplication.DTOs.Request.Jugador;
using Aplication.DTOs.Request.Sancion;
using Aplication.DTOs.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.ISancion
{
    public interface ISancionMapper
    {
        SancionResponse CreateSancionResponse(Sancion sancion);
        Sancion CreateSancion(SancionRequest sancion);
    }
}
