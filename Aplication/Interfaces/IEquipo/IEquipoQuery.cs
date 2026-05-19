using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IEquipo
{
    public interface IEquipoQuery
    {
        Task<Equipo> GetEquipoById(int idEquipo);
        Task<IEnumerable<Equipo>> GetListEquipos();
    }
}
