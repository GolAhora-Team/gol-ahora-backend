using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IReporte
{
    public interface IReporteQuery
    {
        Task<List<Reporte>> GetAllReportes();
    }
}
