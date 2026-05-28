using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IFactura
{
    public interface IFacturaCommand
    {
        Task InsertFactura(Factura factura);

        Task UpdateFactura(Factura factura);

        Task RemoveFactura(int id);
    }
}
