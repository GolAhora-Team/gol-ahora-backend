using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IFactura
{
    public interface IFacturaService
    {
        Task<List<Factura>> GetAll();
        Task<Factura> GetById(int id);
        Task<Factura> Create(Factura factura);
    }
}
