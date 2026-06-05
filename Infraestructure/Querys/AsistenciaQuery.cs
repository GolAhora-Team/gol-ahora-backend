using Aplication.Interfaces.IAsistencia;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Querys
{
    public class AsistenciaQuery : IAsistenciaQuery
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

        public async Task<List<Asistencia>> GetAsistenciasPorClaseYFecha(int claseId, DateTime fecha)
        {
            return await _context.Asistencias
                .Include(a => a.Cliente)
                .Where(a => a.ClaseId == claseId && a.FechaHoraRegistro.HasValue && a.FechaHoraRegistro.Value.Date == fecha.Date)
                .ToListAsync();
        }

        public async Task<List<AsistenciaEntrenamiento>> GetAsistenciasEntrenamientoPorFecha(int entrenamientoId, DateTime fecha)
        {
            return await _context.AsistenciaEntrenamientos
                .Include(a => a.Cliente)
                .Where(a => a.EntrenamientoId == entrenamientoId && a.Fecha.Date == fecha.Date)
                .ToListAsync();
        }

        public async Task<Asistencia?> GetInscripcionClaseAsync(int claseId, int clienteId)
        {
            return await _context.Asistencias
                .Include(a => a.Clase)
                .FirstOrDefaultAsync(a => a.ClaseId == claseId && a.ClienteId == clienteId);
        }

        public async Task<Asistencia?> GetInscripcionClaseByBarcodeAsync(int claseId, string barcode)
        {
            return await _context.Asistencias
                .FirstOrDefaultAsync(a => a.ClaseId == claseId && a.CodigoBarras == barcode);
        }

        public async Task<ClienteEntrenamiento?> GetInscripcionEntrenamientoAsync(int entrenamientoId, int clienteId)
        {
            return await _context.ClienteEntrenamientos
                .Include(c => c.Entrenamiento)
                .FirstOrDefaultAsync(c => c.EntrenamientoId == entrenamientoId && c.ClienteId == clienteId);
        }

        public async Task<ClienteEntrenamiento?> GetInscripcionEntrenamientoByBarcodeAsync(int entrenamientoId, string barcode)
        {
            return await _context.ClienteEntrenamientos
                .Include(c => c.Entrenamiento)
                .FirstOrDefaultAsync(c => c.EntrenamientoId == entrenamientoId && c.CodigoBarras == barcode);
        }

        public async Task<bool> YaAsistioEntrenamientoAsync(int entrenamientoId, int clienteId, DateTime fecha)
        {
            return await _context.AsistenciaEntrenamientos
                .AnyAsync(a => a.EntrenamientoId == entrenamientoId && a.ClienteId == clienteId && a.Fecha.Date == fecha.Date);
        }
    }
}
