using Aplication.Interfaces.IAsistencia;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Querys
{
    public class AsistenciaQuery: IAsistenciaQuery
    {
        private readonly AppDbContext _context;

        public AsistenciaQuery(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AsistenciaExists(int id)
        {
            return await _context.Asistencias.AnyAsync(a => a.Id == id);
        }

        public async Task<List<Asistencia>> GetAllAsistencias()
        {
            return await _context.Asistencias.ToListAsync();
        }

        public async Task<Asistencia?> GetAsistenciaById(int id)
        {
            return await _context.Asistencias.FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
