using Aplication.Interfaces.IEntrenamiento;
using Domain.Entities;
using System.Threading.Tasks;

namespace Infraestructure.Command
{
    public class EntrenamientoCommand : IEntrenamientoCommand
    {
        private readonly AppDbContext _context;

        public EntrenamientoCommand(AppDbContext context)
        {
            _context = context;
        }

        public async Task InsertEntrenamiento(Entrenamiento entrenamiento)
        {
            _context.Entrenamientos.Add(entrenamiento);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveEntrenamiento(Entrenamiento entrenamiento)
        {
            _context.Entrenamientos.Remove(entrenamiento);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEntrenamiento(Entrenamiento entrenamiento)
        {
            _context.Entrenamientos.Update(entrenamiento);
            await _context.SaveChangesAsync();
        }
    }
}
