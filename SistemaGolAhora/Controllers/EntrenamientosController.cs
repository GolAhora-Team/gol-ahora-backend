using Aplication.DTOs.Request.Entrenamiento;
using Aplication.DTOs.Response.Entrenamiento;
using Aplication.Interfaces.IEntrenamiento;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaGolAhora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntrenamientosController : ControllerBase
    {
        private readonly IEntrenamientoService _entrenamientoService;

        public EntrenamientosController(IEntrenamientoService entrenamientoService)
        {
            _entrenamientoService = entrenamientoService;
        }

        [HttpPost]
        public async Task<ActionResult<EntrenamientoResponse>> CrearEntrenamiento([FromBody] EntrenamientoCreateRequest request)
        {
            try
            {
                var response = await _entrenamientoService.CrearEntrenamiento(request);
                return CreatedAtAction(nameof(GetEntrenamientoById), new { id = response.Id }, response);
            }
            catch (ExceptionBadRequest ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (ExceptionNotFound ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EntrenamientoResponse>> GetEntrenamientoById(int id)
        {
            try
            {
                var response = await _entrenamientoService.GetEntrenamientoById(id);
                return Ok(response);
            }
            catch (ExceptionNotFound ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<EntrenamientoResponse>>> GetAllEntrenamientos()
        {
            var response = await _entrenamientoService.GetAllEntrenamientos();
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EntrenamientoResponse>> UpdateEntrenamiento(int id, [FromBody] EntrenamientoUpdateRequest request)
        {
            try
            {
                var response = await _entrenamientoService.UpdateEntrenamiento(id, request);
                return Ok(response);
            }
            catch (ExceptionBadRequest ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (ExceptionNotFound ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<EntrenamientoDeleteResponse>> DeleteEntrenamiento(int id)
        {
            try
            {
                var response = await _entrenamientoService.DeleteEntrenamiento(id);
                return Ok(response);
            }
            catch (ExceptionNotFound ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPost("{entrenamientoId}/clientes/{clienteId}")]
        public async Task<ActionResult<EntrenamientoShortResponse>> AddCliente(int entrenamientoId, int clienteId)
        {
            try
            {
                var response = await _entrenamientoService.AddCliente(entrenamientoId, clienteId);
                return Ok(response);
            }
            catch (ExceptionBadRequest ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (ExceptionNotFound ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
        [HttpGet("{entrenamientoId}/cliente/{clienteId}/pulsera")]
        public async Task<IActionResult> DownloadPulsera(int entrenamientoId, int clienteId, [FromServices] Aplication.Interfaces.ICodigoBarrasService codigoBarrasService)
        {
            try
            {
                var pdfBytes = await codigoBarrasService.GenerarPulseraPdfParaActividadAsync(entrenamientoId, clienteId, false);
                return File(pdfBytes, "application/pdf", $"Pulsera_Entrenamiento_{entrenamientoId}_Cliente_{clienteId}.pdf");
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
