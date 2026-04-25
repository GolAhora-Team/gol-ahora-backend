using Aplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IAdmin
{
    public interface IAdminService
    {
        // ── Queries ────────────────────────────────
        Task<IEnumerable<AdministradorDto>> GetAllAsync();
        Task<AdministradorDto?> GetByIdAsync(int id);
        Task<AdministradorDto?> GetByDniAsync(int dni);
        Task<AdministradorDto?> GetByIdentificadorAsync(int identificador);
        Task<IEnumerable<AdministradorDto>> GetFacturadoresAsync();

        // ── Commands ───────────────────────────────
        Task<AdministradorDto> CreateAsync(AdministradorDto dto);
        Task<AdministradorDto?> UpdateAsync(int id, AdministradorDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
