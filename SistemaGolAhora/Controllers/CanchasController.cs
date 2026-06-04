using Aplication.DTOs.Request.Cancha;
using Aplication.Interfaces.ICancha;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace SistemaGolAhora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CanchasController : ControllerBase
    {
        private readonly ICanchaService _services;

        public CanchasController(ICanchaService services)
        {
            _services = services;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _services.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var cancha = await _services.GetCanchaById(id);
                return Ok(cancha);
            }
            catch (Exception ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
        }

        [HttpGet("activas")]
        public async Task<IActionResult> GetCanchasActivas()
        {
            try
            {
                var result = await _services.GetCanchasActivas();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("disponibles")]
        public async Task<IActionResult> GetCanchasDisponibles([FromQuery] DateTime fecha, [FromQuery] TimeSpan hora)
        {
            try
            {
                var result = await _services.GetCanchasDisponibles(fecha, hora);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCancha(CreateCanchaRequest request)
        {
            try
            {
                var result = await _services.CreateCancha(request);
                return StatusCode(201, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCanchaRequest request)
        {
            try
            {
                var response = await _services.UpdateCancha(id, request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _services.DeleteCancha(id);
                return Ok(new { mensaje = "Cancha eliminada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("precios")]
        public async Task<IActionResult> UpdatePrecios([FromBody] UpdatePreciosRequest request)
        {
            try
            {
                await _services.UpdatePreciosGlobal(request);
                return Ok(new { mensaje = "Precios actualizados correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
