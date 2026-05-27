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
        Task<List<Cancha>> GetListCancha();
        Task<List<Cancha>> GetCanchasActivas();
        Task<List<Cancha>> GetCanchasDisponibles(DateTime fecha, TimeSpan hora);
        Task<bool> CanchaExists(int idCancha);
    }
}
