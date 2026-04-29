using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IPago
{
    public interface IPagoService
    {
        Task<List<Pago>> GetAll();
        Task<Pago> GetById(int id);
        Task<Pago> Create(Pago pago);
        Task<bool> Delete(int id);
    }
}
