using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IReserva
{
    public interface IReservaCommand
    {
        Task InsertReserva(Reserva reserva);
        Task UpdateReserva(Reserva reserva);
        Task RemoveReserva(Reserva idReserva);
    }
}
