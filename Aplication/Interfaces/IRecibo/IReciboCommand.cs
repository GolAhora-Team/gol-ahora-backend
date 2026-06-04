using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IRecibo
{
    public interface IReciboCommand
    {
        Task InsertRecibo(Recibo recibo);
    }
}
