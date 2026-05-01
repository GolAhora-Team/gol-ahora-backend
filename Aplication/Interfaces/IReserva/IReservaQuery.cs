using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IReserva
{
    public interface IReservaQuery
    {
            Task<Domain.Entities.Reserva?> GetReservaById(int id);
        
            Task<bool> ReservaExists(int id);
        
            Task<List<Domain.Entities.Reserva>> GetAllReservas();
    }
}
