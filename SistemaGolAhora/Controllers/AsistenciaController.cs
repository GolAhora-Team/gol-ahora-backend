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

        [HttpGet("actividad/{actividadId}")]
        public async Task<IActionResult> GetAsistenciasPorActividadYFecha(int actividadId, [FromQuery] DateTime fecha, [FromQuery] bool esClase)
        {
            try
            {
                var response = await _asistenciaService.GetAsistenciasPorActividadYFecha(actividadId, fecha, esClase);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost("manual")]
        public async Task<IActionResult> RegistrarAsistenciaManual([FromQuery] int actividadId, [FromQuery] int clienteId, [FromQuery] bool esClase)
        {
            try
            {
                var response = await _asistenciaService.RegistrarAsistenciaManual(actividadId, clienteId, esClase);
                return Ok(new { success = response, mensaje = "Asistencia manual registrada correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost("barcode")]
        public async Task<IActionResult> RegistrarAsistenciaCodigoBarras([FromQuery] string codigoBarras, [FromQuery] int actividadId, [FromQuery] bool esClase)
        {
            try
            {
                var response = await _asistenciaService.RegistrarAsistenciaCodigoBarras(codigoBarras, actividadId, esClase);
                return Ok(new { success = response, mensaje = "Asistencia por código de barras registrada correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
