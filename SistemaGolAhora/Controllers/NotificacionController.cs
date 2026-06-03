using Aplication.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SistemaGolAhora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificacionController : ControllerBase
    {
        private readonly INotificacionService _notificacionService;
        private readonly Aplication.Interfaces.IUsuario.IUsuarioQuery _usuarioQuery;

        public NotificacionController(INotificacionService notificacionService, Aplication.Interfaces.IUsuario.IUsuarioQuery usuarioQuery)
        {
            _notificacionService = notificacionService;
            _usuarioQuery = usuarioQuery;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotificaciones()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int usuarioId))
            {
                return Unauthorized();
            }

            var notificaciones = await _notificacionService.ObtenerNotificacionesPorUsuario(usuarioId);
            return Ok(notificaciones);
        }

        [HttpPost("leidas")]
        public async Task<IActionResult> MarcarComoLeidas()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int usuarioId))
            {
                return Unauthorized();
            }

            await _notificacionService.MarcarNotificacionesComoVistas(usuarioId);
            return Ok(new { message = "Notificaciones marcadas como leídas" });
        }
    }
}
