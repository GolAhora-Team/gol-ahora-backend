using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IDescuento
{
    public interface IDescuentoQuery
    {
        Task<List<Descuento>> GetListDescuentos();
        Task<Descuento?> GetDescuentoById(int id);
    }
}
