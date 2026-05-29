using Aplication.Interfaces.IReporte;
using Domain.Entities;
using System.Threading.Tasks;

namespace Infraestructure.Command
{
    public class ReporteCommand : IReporteCommand
    {
        private readonly AppDbContext _context;

        public ReporteCommand(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveReporte(Reporte reporte)
        {
            _context.Reportes.Add(reporte);
            await _context.SaveChangesAsync();
        }
    }
}
