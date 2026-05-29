using Domain.Entities;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IReporte
{
    public interface IReporteCommand
    {
        Task SaveReporte(Reporte reporte);
    }
}
