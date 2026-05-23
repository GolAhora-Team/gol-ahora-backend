using Aplication.DTOs.Request.Cliente;
using Aplication.DTOs.Request.Equipo;
using Aplication.DTOs.Request.Jugador;
using Aplication.Interfaces.IJugador;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace SistemaGolAhora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JugadorController : ControllerBase
    {
        private readonly IJugadorService _services;

        public JugadorController(IJugadorService services)
        {
            _services = services;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var jugador = await _services.GetJugadorById(id);
                return Ok(jugador); 
            }
            catch (Exception ex)
            {
                return NotFound(new { mensaje = ex.Message }); 
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _services.GetAll();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateJugador([FromBody] JugadorRequest request)
        {
            try
            {
                var result = await _services.CreateJugador(request);
                return StatusCode(201, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJugador(int id, [FromBody] JugadorRequest request)
        {
            try
            {
                var response = await _services.UpdateJugador(id, request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJugador(int id)
        {
            try
            {
                await _services.DeleteJugador(id);
                return Ok(new { mensaje = "Jugador eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
