using Aplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IJugador
{
    public interface IJugadorService
    {
        Task<IEnumerable<JugadorDto>> GetAllAsync();
        Task<JugadorDto?> GetByIdAsync(int id);
        Task<JugadorDto?> GetByClienteIdAsync(int clienteId);
        Task<JugadorDto> CreateAsync(JugadorDto jugadorDto);
        Task<JugadorDto?> UpdateAsync(int id, JugadorDto jugadorDto);
        Task<bool> DeleteAsync(int id);
    }
}
