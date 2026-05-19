using Aplication.DTOs.Request.Cliente;
using Aplication.DTOs.Request.Equipo;
using Aplication.DTOs.Response;
using Aplication.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IEquipo
{
    public interface IEquipoService
    {
        Task<EquipoResponse> CreateEquipo(CreateEquipoRequest request);
        Task<EquipoResponse> UpdateEquipo(int equipoId, CreateEquipoRequest request);
        Task<EquipoResponse> DeleteEquipo(int equipoId);
        Task<List<EquipoResponse>> GetAll();
        Task<EquipoResponse> GetEquipoById(int equipoId);
    }
}
