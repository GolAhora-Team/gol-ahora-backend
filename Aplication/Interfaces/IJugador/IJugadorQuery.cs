using Aplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IJugador
{
    public interface IJugadorQuery
    {
        Task<IEnumerable<JugadorDto>> GetAllAsync();
        Task<JugadorDto?> GetByIdAsync(int id);
        Task<JugadorDto?> GetByClienteIdAsync(int clienteId);
    }
}
