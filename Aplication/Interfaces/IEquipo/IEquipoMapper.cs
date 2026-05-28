using Aplication.DTOs.Request.Cliente;
using Aplication.DTOs.Request.Equipo;
using Aplication.DTOs.Response;
using Aplication.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IEquipo
{
    public interface IEquipoMapper
    {
        EquipoResponse CreateEquipoResponse(Equipo equipo);
        Equipo CreateEquipo(CreateEquipoRequest equipo);
    }
}
