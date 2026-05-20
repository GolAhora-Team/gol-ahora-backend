using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.ICompeticion
{
    public interface ICompeticionCommand
    {
        Task InsertCompeticion(Competicion competicion);
        Task UpdateCompeticion(Competicion competicion);
        Task RemoveCompeticion(int idCompeticion);
    }
}
