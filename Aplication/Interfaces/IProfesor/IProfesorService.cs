using Aplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IProfesor
{
    public interface IProfesorService
    {
        // ── Queries ────────────────────────────────
        Task<IEnumerable<ProfesorDto>> GetAllAsync();
        Task<ProfesorDto?> GetByIdAsync(int id);
        Task<ProfesorDto?> GetByDniAsync(int dni);
        Task<IEnumerable<ProfesorDto>> GetByEspecialidadAsync(string especialidad);
        Task<IEnumerable<ProfesorDto>> GetByClaseAsync(int claseId);

        // ── Commands ───────────────────────────────
        Task<ProfesorDto> CreateAsync(ProfesorDto dto);
        Task<ProfesorDto?> UpdateAsync(int id, ProfesorDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> AsignarClaseAsync(int profesorId, int claseId);
        Task<bool> AsignarEntrenamientoAsync(int profesorId, int entrenamientoId);
        Task<bool> ValidarCertificadoAsync(int profesorId);
    }
}
