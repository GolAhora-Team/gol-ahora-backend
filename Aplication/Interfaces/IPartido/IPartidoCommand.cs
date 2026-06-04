using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IPartido
{
    public interface IPartidoCommand
    {
        Task InsertPartidos(List<Partido> partidos);
        Task UpdatePartido(Partido partido);
        Task DeletePartidosPorCompeticion(int competicionId);
    }
}
