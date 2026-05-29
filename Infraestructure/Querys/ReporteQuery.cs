using Aplication.Interfaces.IReporte;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infraestructure.Querys
{
    public class ReporteQuery : IReporteQuery
    {
        private readonly AppDbContext _context;

        public ReporteQuery(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Reporte>> GetAllReportes()
        {
            return await _context.Reportes.ToListAsync();
        }
    }
}
