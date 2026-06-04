using Aplication.DTOs.Request.Cliente;
using Aplication.DTOs.Request.Equipo;
using Aplication.DTOs.Response;
using Aplication.Interfaces.IEquipo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SistemaGolAhora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipoController : ControllerBase
    {
        private readonly IEquipoService _services;

        public EquipoController(IEquipoService services)
        {
            _services = services;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<EquipoResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _services.GetAll();
            return new JsonResult(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(EquipoResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateEquipo(CreateEquipoRequest request)
        {
            try
            {
                var result = await _services.CreateEquipo(request);
                return StatusCode(201, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EquipoResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var cliente = await _services.GetEquipoById(id);
                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(EquipoResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(int id, [FromBody] CreateEquipoRequest request)
        {
            try
            {
                var response = await _services.UpdateEquipo(id, request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(EquipoResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _services.DeleteEquipo(id);
                return Ok(new { mensaje = "Equipo eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("por-cliente/{clienteId}")]
        [ProducesResponseType(typeof(List<EquipoResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByClienteId(int clienteId)
        {
            try
            {
                var result = await _services.GetEquiposByClienteId(clienteId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost("{equipoId}/invitar")]
        public async Task<IActionResult> InvitarJugador(int equipoId, [FromBody] InvitarJugadorRequest request)
        {
            try
            {
                await _services.InvitarJugador(equipoId, request.Username, request.InvitadoPorUsuarioId);
                return Ok(new { mensaje = "Invitación enviada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }

    public class InvitarJugadorRequest
    {
        public string Username { get; set; }
        public int InvitadoPorUsuarioId { get; set; }
    }
}
