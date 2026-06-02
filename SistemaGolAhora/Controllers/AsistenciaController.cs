using Aplication.DTOs.Request.Asistencia;
using Aplication.Interfaces.IAsistencia;
using Microsoft.AspNetCore.Mvc;

namespace SistemaGolAhora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsistenciaController : ControllerBase
    {
        private readonly IAsistenciaService _asistenciaService;

        public AsistenciaController(IAsistenciaService asistenciaService)
        {
            _asistenciaService = asistenciaService;
        }

        [HttpGet("clase/{claseId}")]
        public async Task<IActionResult> GetAsistenciasPorClaseYFecha(int claseId, [FromQuery] DateTime fecha)
        {
            try
            {
                var response = await _asistenciaService.GetAsistenciasPorClaseYFecha(claseId, fecha);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost("marcar")]
        public async Task<IActionResult> MarcarAsistencia([FromBody] MarcarAsistenciaRequest request)
        {
            try
            {
                var response = await _asistenciaService.MarcarAsistencia(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
