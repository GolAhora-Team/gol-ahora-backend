using Aplication.DTOs.Request.Competición;
using Aplication.DTOs.Request.Equipo;
using Aplication.DTOs.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.ICompeticion
{
    public interface ICompeticionMapper
    {
        CompeticionResponse CreateCompeticionResponse(Competicion competicion);
        Competicion CreateCompeticion(CompeticionRequest competicion);
    }
}
