using Aplication.DTOs.Request.Competición;
using Aplication.DTOs.Request.Equipo;
using Aplication.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.ICompeticion
{
    public interface ICompeticionService
    {
        Task<CompeticionResponse> CreateCompeticion(CompeticionRequest request);
        Task<CompeticionResponse> UpdateCompeticion(int competicionId, CompeticionRequest request);
        Task<CompeticionResponse> DeleteCompeticion(int competicionId);
        Task<List<CompeticionResponse>> GetAll();
        Task<CompeticionResponse> GetCompeticionById(int competicionId);
    }
}
