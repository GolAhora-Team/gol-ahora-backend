using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IPago
{
    public interface IPagoQuery
    {
        Task<List<Domain.Entities.Pago>> GetListPagos();

        Task<Domain.Entities.Pago?> GetPagoById(int id);
    }
}
