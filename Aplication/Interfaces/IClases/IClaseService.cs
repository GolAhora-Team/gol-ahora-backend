using Aplication.DTOs.Request.Clase;
using Aplication.DTOs.Response.Clase;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IClases
{
    public interface IClaseService
    {
        Task<ClaseCreateResponse> CrearClase(ClaseCreateRequest clase);
        Task<ClaseResponse> getClaseId(int id);
        Task<List<ClaseResponse>> getClases();
        Task<ClaseCreateResponse> UpdateClase(int id, ClaseUpdateRequest clase);
        Task<ClaseDeleteResponse> DeleteClase(int id);
        Task<ClaseShortResponse> addProfesor(int claseId, int profesorId);
        Task<ClaseShortResponse> addCliente(int claseId, int clienteId);
    }
}
