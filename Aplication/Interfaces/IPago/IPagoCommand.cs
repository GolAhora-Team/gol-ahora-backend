using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IPago
{
    public interface IPagoCommand
    {
        Task InsertPago(Domain.Entities.Pago pago);

        Task UpdatePago(Domain.Entities.Pago pago);

        Task RemovePago(int id);
    }

}
