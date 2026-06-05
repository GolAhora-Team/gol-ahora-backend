using Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaGolAhora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificacionController : ControllerBase
    {
        private readonly INotificacionService _notificacionService;
        private readonly Aplication.Interfaces.IUsuario.IUsuarioQuery _usuarioQuery;

        public NotificacionController(INotificacionService notificacionService, Aplication.Interfaces.IUsuario.IUsuarioQuery usuarioQuery)
        {
            _notificacionService = notificacionService;
            _usuarioQuery = usuarioQuery;
        }

        private int? GetUsuarioIdFromToken()
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer ")) return null;
            
            var tokenJson = authHeader.Substring("Bearer ".Length).Trim();
            try {
                using (var doc = System.Text.Json.JsonDocument.Parse(tokenJson))
                {
                    if (doc.RootElement.TryGetProperty("idUsuario", out var idProp))
                    {
                        return idProp.GetInt32();
                    }
                }
            } catch { }
            return null;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotificaciones()
        {
            var usuarioId = GetUsuarioIdFromToken();
            if (!usuarioId.HasValue)
            {
                return Unauthorized(new { mensaje = "Token inválido o no proporcionado" });
            }

            var notificaciones = await _notificacionService.ObtenerNotificacionesPorUsuario(usuarioId.Value);
            return Ok(notificaciones);
        }

        [HttpPost("leidas")]
        public async Task<IActionResult> MarcarComoLeidas()
        {
            var usuarioId = GetUsuarioIdFromToken();
            if (!usuarioId.HasValue)
            {
                return Unauthorized(new { mensaje = "Token inválido o no proporcionado" });
            }

            await _notificacionService.MarcarNotificacionesComoVistas(usuarioId.Value);
            return Ok(new { message = "Notificaciones marcadas como leídas" });
        }

        [HttpPost("{id}/aceptar")]
        public async Task<IActionResult> AceptarInvitacion(int id)
        {
            try
            {
                await _notificacionService.AceptarInvitacion(id);
                return Ok(new { mensaje = "Invitación aceptada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost("{id}/rechazar")]
        public async Task<IActionResult> RechazarInvitacion(int id)
        {
            try
            {
                await _notificacionService.RechazarInvitacion(id);
                return Ok(new { mensaje = "Invitación rechazada" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost("general")]
        public async Task<IActionResult> CrearNotificacionGeneral([FromBody] NotificacionGeneralRequest request)
        {
            try
            {
                await _notificacionService.CrearNotificacionGeneral(request.Mensaje, request.RolesDestino, request.Tipo);
                return Ok(new { mensaje = "Notificación enviada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }

    public class NotificacionGeneralRequest
    {
        public string Mensaje { get; set; }
        public string RolesDestino { get; set; }
        public string Tipo { get; set; }
    }
}
