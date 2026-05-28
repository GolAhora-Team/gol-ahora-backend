using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.ISancion
{
    public interface ISancionCommand
    {
        Task InsertSancion(Sancion sancion);        
        Task RemoveSancion(int idSancion);
    }
}
