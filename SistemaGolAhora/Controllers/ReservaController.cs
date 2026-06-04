using Aplication.DTOs.Request.Reserva;
using Aplication.DTOs.Response.Reserva;
using Aplication.Interfaces.IReserva;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SistemaGolAhora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaController : ControllerBase
    {
        private readonly IReservaService _reservaService;

        public ReservaController(IReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateReservaResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CrearReserva([FromBody] CreateReservaRequest request)
        {
            try
            {
                var response = await _reservaService.CrearReserva(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ReservaResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObtenerReservas()
        {
            try
            {
                var response = await _reservaService.GetAllReservas();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReservaResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObtenerReservaPorId(int id)
        {
            try
            {
                var response = await _reservaService.GetReservaById(id);
                if (response == null)
                {
                    return NotFound();
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("{id}/cancelacion-info")]
        [ProducesResponseType(typeof(Aplication.DTOs.Response.Reserva.CancelacionInfoResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObtenerInfoCancelacion(int id)
        {
            try
            {
                var response = await _reservaService.GetCancelacionInfo(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPatch("{id}/cancelar")]
        [ProducesResponseType(typeof(ReservaResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CancelarReserva(int id)
        {
            try
            {
                var response = await _reservaService.CancelarReserva(id);
                if (response == null)
                {
                    return NotFound();
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ReservaResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> ModificarReserva(int id, [FromBody] UpdateReservaRequest request)
        {
            try
            {
                var response = await _reservaService.ModificarReserva(id, request);
                if (response == null)
                {
                    return NotFound();
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
