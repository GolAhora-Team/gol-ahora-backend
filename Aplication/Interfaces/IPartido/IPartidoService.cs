using Aplication.DTOs.Request.Competición;
using Aplication.DTOs.Request.Partido;
using Aplication.DTOs.Response;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IPartido
{
    public interface IPartidoService
    {        
        Task<PartidoResponse> GetPartidoById(int partidoId);
        Task<List<PartidoResponse>> GetPartidosPorCompeticion(int competicionId);
        Task<List<PartidoResponse>> GetPartidosPorFase(int competicionId, FaseTorneo fase);        
        Task<PartidoResponse> CargarResultado(int partidoId, CargarResultadoRequest request);        
        Task GenerarFixture(int competicionId);
        
    }
}
