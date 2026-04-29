using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.ICancha
{
    public interface ICanchaQuery
    {
        Task<Cancha> GetCanchaById(int idCancha);
        Task<IEnumerable<Cliente>> GetListCancha();
        Task<Cancha> GetCanchasActivas();
        Task<Cancha> GetCanchasDisponibles(DateTime fecha, TimeSpan hora);
    }
}
