using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IFactura
{
    public interface IFacturaQuery
    {
        Task<List<Factura>> GetListFacturas();

        Task<Factura?> GetFacturaById(int id);

        Task<List<Factura>> GetFacturasByClienteId(int clienteId);
    }
}
