using Aplication.DTOs;
using Aplication.DTOs.Request.Profesor;
using Aplication.DTOs.Response.Profesor;
using Aplication.Interfaces;
using Aplication.Interfaces.INotificacion;
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
        private readonly INotificacionService _notificacionService;

        public ProfesorService(IProfesorQuery query, IProfesorCommand command, INotificacionService notificacionService)
        {
            _query = query;
            _command = command;
            _notificacionService = notificacionService;
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
        {
            var result = await _command.UpdateSimpleAsync(id, request);
            if (result != null && request.Certificados != null && request.Certificados.Any(c => !string.IsNullOrEmpty(c.CertificadoBase64)))
            {
                await _notificacionService.CrearNotificacionGeneral(
                    $"El profesor {result.Nombre} {result.Apellido} ha cargado su certificado.",
                    "ADMIN,PERSONAL",
                    "Documentacion"
                );
            }
            return result;
        }

        public async Task<(bool Success, List<string> AffectedItems, string ProfesorName)> DeleteAsync(int id)
        {
            var result = await _command.DeleteAsync(id);
            if (result.Success && result.AffectedItems != null && result.AffectedItems.Any())
            {
                string itemList = string.Join(", ", result.AffectedItems);
                string mensaje = $"Se eliminó al profesor {result.ProfesorName}. Las siguientes clases y entrenamientos se quedaron sin profesor: {itemList}.";
                await _notificacionService.CrearNotificacionGeneral(mensaje, "ADMIN,PERSONAL", "DesasignacionProfesor");
            }
            return result;
        }

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
