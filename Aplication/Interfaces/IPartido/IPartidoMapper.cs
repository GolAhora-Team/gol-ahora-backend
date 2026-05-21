using Aplication.DTOs.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IPartido
{
    public interface IPartidoMapper
    {
        PartidoResponse CreatePartidoResponse(Partido partido);
        List<PartidoResponse> CreatePartidoResponseList(List<Partido> partidos);
    }
}
