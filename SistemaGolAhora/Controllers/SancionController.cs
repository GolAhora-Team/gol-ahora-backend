using Aplication.DTOs.Request.Jugador;
using Aplication.DTOs.Request.Sancion;
using Aplication.DTOs.Response;
using Aplication.Interfaces.ISancion;
using Aplication.UseCase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SistemaGolAhora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SancionController : ControllerBase
    {
        private readonly ISancionService _services;

        public SancionController(ISancionService services)
        {
            _services = services;
        }

        [HttpGet("jugador/{jugadorId}")]
        public async Task<IActionResult> GetSancionesPorJugador(int jugadorId)
        {
            try
            {
                var sanciones = await _services.GetSancionesPorJugador(jugadorId);
                return Ok(sanciones);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSancion([FromBody] SancionRequest request)
        {
            try
            {
                var result = await _services.CreateSancion(request);
                return StatusCode(201, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpDelete("{sancionId}")]
        public async Task<IActionResult> DeleteSancion(int sancionId)
        {
            try
            {
                await _services.DeleteSancion(sancionId);
                return Ok(new { mensaje = "Sanción eliminada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
