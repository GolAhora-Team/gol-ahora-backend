using Aplication.Interfaces.IEntrenamiento;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infraestructure.Querys
{
    public class EntrenamientoQuery : IEntrenamientoQuery
    {
        private readonly AppDbContext _context;

        public EntrenamientoQuery(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Entrenamiento>> GetAllEntrenamientos()
        {
            return await _context.Entrenamientos
                .Include(e => e.Profesor)
                .Include(e => e.Cancha)
                .Include(e => e.Clientes)
                .ToListAsync();
        }

        public async Task<Entrenamiento> GetEntrenamientoById(int id)
        {
            return await _context.Entrenamientos
                .Include(e => e.Profesor)
                .Include(e => e.Cancha)
                .Include(e => e.Clientes)
                .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
