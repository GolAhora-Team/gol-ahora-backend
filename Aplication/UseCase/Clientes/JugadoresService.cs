using Aplication.DTOs;
using Aplication.Interfaces.IJugador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.UseCase.Clientes
{
    public class JugadoresService : IJugadorService
    {
        private readonly IJugadorQuery _query;
        private readonly IJugadorCommand _command;
        public JugadoresService(IJugadorQuery query, IJugadorCommand command)
        {
            _query = query;
            _command = command;
        }
        public async Task<IEnumerable<JugadorDto>> GetAllAsync()
            => await _query.GetAllAsync();
        public async Task<JugadorDto?> GetByIdAsync(int id)
            => await _query.GetByIdAsync(id);
        public async Task<JugadorDto?> GetByClienteIdAsync(int clienteId)
            => await _query.GetByClienteIdAsync(clienteId);
        public async Task<JugadorDto> CreateAsync(JugadorDto jugadorDto)
            => await _command.CreateAsync(jugadorDto);
        public async Task<JugadorDto?> UpdateAsync(int id, JugadorDto jugadorDto)
            => await _command.UpdateAsync(id, jugadorDto);
        public async Task<bool> DeleteAsync(int id)
            => await _command.DeleteAsync(id);
    }
}
