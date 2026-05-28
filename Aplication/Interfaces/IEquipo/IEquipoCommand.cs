using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IEquipo
{
    public interface IEquipoCommand
    {
        Task InsertEquipo(Equipo equipo);
        Task UpdateEquipo(Equipo equipo);
        Task RemoveEquipo(int idEquipo);
    }
}
