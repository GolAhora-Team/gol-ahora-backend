using Aplication.Interfaces.IReporte;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplication.UseCase.Reportes
{
    public class ReporteService : IReporteService
    {
        private readonly IReporteCommand _command;
        private readonly IReporteQuery _query;

        public ReporteService(IReporteCommand command, IReporteQuery query)
        {
            _command = command;
            _query = query;
        }

        public async Task<List<Reporte>> GetAllReportes()
        {
            return await _query.GetAllReportes();
        }

        public async Task SaveReporte(Reporte reporte)
        {
            await _command.SaveReporte(reporte);
        }
    }
}
