using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IReporte
{
    public interface IReporteService
    {
        Task<List<Reporte>> GetAllReportes();
        Task SaveReporte(Reporte reporte);
    }
}
