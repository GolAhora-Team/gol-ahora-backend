using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.ICancha
{
    public interface ICanchaCommand
    {
        Task InsertCancha(Cancha cancha);
        Task UpdateCancha(Cancha cancha);
        Task RemoveCancha(int idCancha);
    }
}
