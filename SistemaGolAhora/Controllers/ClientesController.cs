using Aplication.DTOs.Request.Cliente;
using Aplication.Interfaces.ICliente;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SistemaGolAhora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteServices _services;

        public ClientesController(IClienteServices services)
        {
            _services = services;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _services.GetAll();
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCliente(CreateClienteRequest request)
        {
            try
            {
                var result = await _services.CreateCliente(request);                
                return StatusCode(201, result);
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
                var cliente = await _services.GetClienteById(id);
                return Ok(cliente);
            }
            catch (Exception ex)
            {                
                return NotFound(new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateClienteRequest request)
        {
            try
            {
                var response = await _services.UpdateCliente(id, request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("{id}/apto-medico/descargar")]
        public async Task<IActionResult> DownloadAptoMedico(int id)
        {
            try
            {
                var bytes = await _services.GetAptoMedico(id);
                if (bytes == null || bytes.Length == 0)
                    return NotFound(new { mensaje = "El cliente no posee un apto médico." });

                // Detectamos pdf por la firma o asumimos image/jpeg por defecto
                string contentType = "application/pdf";
                if (bytes.Length > 4 && bytes[0] != 0x25 && bytes[1] != 0x50 && bytes[2] != 0x44 && bytes[3] != 0x46)
                {
                    contentType = "image/jpeg";
                }

                return File(bytes, contentType);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpDelete("{id}/apto-medico")]
        public async Task<IActionResult> DeleteAptoMedico(int id)
        {
            try
            {
                await _services.DeleteAptoMedico(id);
                return Ok(new { mensaje = "Apto médico eliminado correctamente" });
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
                await _services.DeleteCliente(id);
                return Ok(new { mensaje = "Cliente eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
