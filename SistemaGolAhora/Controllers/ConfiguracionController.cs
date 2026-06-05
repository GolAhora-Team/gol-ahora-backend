using Aplication.DTOs;
using Aplication.Interfaces.IConfiguracion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SistemaGolAhora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfiguracionController : ControllerBase
    {
        private readonly IConfiguracionService _service;

        public ConfiguracionController(IConfiguracionService service)
        {
            _service = service;
        }

        [HttpGet("cancelaciones")]
        [ProducesResponseType(typeof(ConfiguracionCancelacionesDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetConfiguracion()
        {
            try
            {
                var response = await _service.GetConfiguracion();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("cancelaciones")]
        [ProducesResponseType(typeof(ConfiguracionCancelacionesDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateConfiguracion([FromBody] ConfiguracionCancelacionesDto request)
        {
            try
            {
                var response = await _service.UpdateConfiguracion(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
