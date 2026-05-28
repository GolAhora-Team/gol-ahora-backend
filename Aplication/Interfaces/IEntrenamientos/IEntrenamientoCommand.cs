using Domain.Entities;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IEntrenamiento
{
    public interface IEntrenamientoCommand
    {
        Task InsertEntrenamiento(Entrenamiento entrenamiento);
        Task UpdateEntrenamiento(Entrenamiento entrenamiento);
        Task RemoveEntrenamiento(Entrenamiento entrenamiento);
    }
}
