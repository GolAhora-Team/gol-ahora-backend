using Aplication.DTOs.Request.Partido;
using Aplication.Interfaces.IPartido;
using Aplication.UseCase;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SistemaGolAhora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartidoController : ControllerBase
    {
        private readonly IPartidoService _services;

        public PartidoController(IPartidoService services)
        {
            _services = services;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var partido = await _services.GetPartidoById(id);
                return Ok(partido); 
            }
            catch (Exception ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
        }

        [HttpGet("competicion/{competicionId}")]
        public async Task<IActionResult> GetByCompeticion(int competicionId)
        {
            var partidos = await _services.GetPartidosPorCompeticion(competicionId);
            return Ok(partidos);
        }

        [HttpGet("competicion/{competicionId}/fase/{fase}")]
        public async Task<IActionResult> GetByFase(int competicionId, FaseTorneo fase)
        {
            var partidos = await _services.GetPartidosPorFase(competicionId, fase);
            return Ok(partidos);
        }

        [HttpPut("{id}/resultado")]
        public async Task<IActionResult> CargarResultado(int id, [FromBody] CargarResultadoRequest request)
        {
            try
            {
                var partidoActualizado = await _services.CargarResultado(id, request);
                return Ok(partidoActualizado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost("fixture/competicion/{competicionId}")]
        public async Task<IActionResult> GenerarFixture(int competicionId)
        {
            try
            {                
                await _services.GenerarFixture(competicionId);

                return Ok(new { mensaje = "El fixture se generó correctamente." });
            }
            catch (Exception ex)
            {                
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
