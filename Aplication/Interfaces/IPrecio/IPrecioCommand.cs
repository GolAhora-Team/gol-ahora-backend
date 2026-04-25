using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IPrecio
{
    public interface IPrecioCommand
    {
        Task InsertPrecio(Precio precio);
        Task RemovePrecio(int idPrecio);
        Task UpdatePrecio(Precio precio);

    }
}
