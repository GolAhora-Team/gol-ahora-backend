using Aplication.Interfaces.IClases;
using Domain.Exceptions;
using Aplication.DTOs.Request.Clase;
using Microsoft.AspNetCore.Mvc;

namespace SistemaGolAhora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaseController : ControllerBase
    {
        private readonly IClaseService _claseService;

        public ClaseController(IClaseService claseService)
        {
            _claseService = claseService;
        }

        [HttpPost]
        public async Task<IActionResult> CrearClase([FromBody] ClaseCreateRequest clase)
        {
            try
            {
                var result = await _claseService.CrearClase(clase);
                return Ok(result);
            }
            catch (ExceptionBadRequest ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClaseById(int id)
        {
            try
            {
                var result = await _claseService.getClaseId(id);
                return Ok(result);
            }
            catch (ExceptionNotFound ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetClases()
        {
            var result = await _claseService.getClases();
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClase(int id, [FromBody] ClaseUpdateRequest clase)
        {
            try
            {
                var result = await _claseService.UpdateClase(id, clase);
                return Ok(result);
            }
            catch (ExceptionBadRequest ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (ExceptionNotFound ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClase(int id)
        {
            try
            {
                var result = await _claseService.DeleteClase(id);
                return Ok(result);
            }
            catch (ExceptionNotFound ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPatch("{claseId}/profesor/{profesorId}")]
        public async Task<IActionResult> AddProfesor(int claseId, int profesorId)
        {
            try
            {
                var result = await _claseService.addProfesor(claseId, profesorId);
                return Ok(result);
            }
            catch (ExceptionBadRequest ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (ExceptionNotFound ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPatch("{claseId}/cliente/{clienteId}")]
        public async Task<IActionResult> AddCliente(int claseId, int clienteId)
        {
            try
            {
                var result = await _claseService.addCliente(claseId, clienteId);
                return Ok(result);
            }
            catch (ExceptionBadRequest ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (ExceptionNotFound ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
