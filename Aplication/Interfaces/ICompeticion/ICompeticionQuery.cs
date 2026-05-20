using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.ICompeticion
{
    public interface ICompeticionQuery
    {
        Task<Competicion> GetCompeticionById(int idCompeticion);
        Task<IEnumerable<Competicion>> GetListCompeticion();
    }
}
