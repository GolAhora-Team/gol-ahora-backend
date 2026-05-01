using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IDescuento
{
    public interface IDescuentoCommand
    {
        Task InsertDescuento(Descuento descuento);
        Task UpdateDescuento(Descuento descuento);
        Task RemoveDescuento(int id);
    }
}
