using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IClases
{
    public interface IClaseCommand
    {
        Task InsertClase(Clase clase);
        Task UpdateClase(Clase clase);
        Task RemoveClase(Clase idClase);
    }
}
