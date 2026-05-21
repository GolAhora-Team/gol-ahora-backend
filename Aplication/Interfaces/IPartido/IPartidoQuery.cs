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
    public interface IPartidoQuery
    {
        Task<Partido> GetPartidoById(int idPartido);       
        Task<List<PartidoResponse>> GetPartidosPorCompeticion(int competicionId);                
        Task<List<PartidoResponse>> GetPartidosPorFase(int competicionId, FaseTorneo fase);
    }
}
