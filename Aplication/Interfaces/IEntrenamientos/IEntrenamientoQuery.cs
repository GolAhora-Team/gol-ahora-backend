using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IEntrenamiento
{
    public interface IEntrenamientoQuery
    {
        Task<List<Entrenamiento>> GetAllEntrenamientos();
        Task<Entrenamiento> GetEntrenamientoById(int id);
    }
}
