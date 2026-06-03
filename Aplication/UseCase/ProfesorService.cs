using Aplication.DTOs;
using Aplication.DTOs.Request.Profesor;
using Aplication.DTOs.Response.Profesor;
using Aplication.Interfaces.IProfesor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.UseCase
{
    public class ProfesorService : IProfesorService
    {
        private readonly IProfesorQuery _query;
        private readonly IProfesorCommand _command;

        public ProfesorService(IProfesorQuery query, IProfesorCommand command)
        {
            _query = query;
            _command = command;
        }

        // ── Queries ────────────────────────────────
        public async Task<IEnumerable<ProfesorDto>> GetAllAsync()
            => await _query.GetAllAsync();

        public async Task<ProfesorDto?> GetByIdAsync(int id)
            => await _query.GetByIdAsync(id);

        public async Task<ProfesorDto?> GetByDniAsync(int dni)
            => await _query.GetByDniAsync(dni);

        public async Task<IEnumerable<ProfesorDto>> GetByEspecialidadAsync(string especialidad)
            => await _query.GetByEspecialidadAsync(especialidad);

        public async Task<IEnumerable<ProfesorDto>> GetByClaseAsync(int claseId)
            => await _query.GetByClaseAsync(claseId);

        // ── Commands ───────────────────────────────
        public async Task<ProfesorDto> CreateAsync(ProfesorDto dto)
            => await _command.CreateAsync(dto);

        public async Task<ProfesorDto?> UpdateAsync(int id, ProfesorDto dto)
            => await _command.UpdateAsync(id, dto);

        public async Task<ProfesorResponse?> UpdateSimpleAsync(int id, UpdateProfesorSimpleRequest request)
            => await _command.UpdateSimpleAsync(id, request);

        public async Task<bool> DeleteAsync(int id)
            => await _command.DeleteAsync(id);

        public async Task<bool> AsignarClaseAsync(int profesorId, int claseId)
            => await _command.AsignarClaseAsync(profesorId, claseId);

        public async Task<bool> AsignarEntrenamientoAsync(int profesorId, int entrenamientoId)
            => await _command.AsignarEntrenamientoAsync(profesorId, entrenamientoId);

        public async Task<bool> ValidarCertificadoAsync(int profesorId)
            => await _command.ValidarCertificadoAsync(profesorId);

        public async Task<byte[]?> GetCertificado(int profesorId)
            => await _query.GetCertificadoAsync(profesorId);
    }
}
