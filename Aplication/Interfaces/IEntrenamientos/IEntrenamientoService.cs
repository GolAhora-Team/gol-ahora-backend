using Aplication.DTOs.Request.Entrenamiento;
using Aplication.DTOs.Response.Entrenamiento;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IEntrenamiento
{
    public interface IEntrenamientoService
    {
        Task<EntrenamientoResponse> CrearEntrenamiento(EntrenamientoCreateRequest request);
        Task<EntrenamientoResponse> UpdateEntrenamiento(int id, EntrenamientoUpdateRequest request);
        Task<EntrenamientoResponse> GetEntrenamientoById(int id);
        Task<List<EntrenamientoResponse>> GetAllEntrenamientos();
        Task<EntrenamientoDeleteResponse> DeleteEntrenamiento(int id);
        Task<EntrenamientoShortResponse> AddCliente(int entrenamientoId, int clienteId);
        Task<EntrenamientoShortResponse> RemoveCliente(int entrenamientoId, int clienteId);
    }
}
