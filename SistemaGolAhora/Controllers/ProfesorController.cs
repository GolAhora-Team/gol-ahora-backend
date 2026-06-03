using Aplication.DTOs.Request.Profesor;
using Aplication.Interfaces.IProfesor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SistemaGolAhora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfesorController : ControllerBase
    {
        private readonly IProfesorService _services;
        private readonly IProfesorMapper _mapper;

        public ProfesorController(IProfesorService services, IProfesorMapper mapper)
        {
            _services = services;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var profesoresDto = await _services.GetAllAsync();
                var response = profesoresDto.Select(p => _mapper.CreateProfesorResponseFromDto(p)).ToList();
                return Ok(response);
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
                var profesorDto = await _services.GetByIdAsync(id);
                if (profesorDto == null) return NotFound(new { mensaje = "Profesor no encontrado" });
                
                var response = _mapper.CreateProfesorResponseFromDto(profesorDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}/simple")]
        public async Task<IActionResult> UpdateSimple(int id, [FromBody] UpdateProfesorSimpleRequest request)
        {
            try
            {
                var response = await _services.UpdateSimpleAsync(id, request);
                if (response == null) return NotFound(new { mensaje = "Profesor no encontrado" });
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("{id}/certificado/descargar")]
        public async Task<IActionResult> DownloadCertificado(int id)
        {
            try
            {
                var bytes = await _services.GetCertificado(id);
                if (bytes == null || bytes.Length == 0)
                    return NotFound(new { mensaje = "El profesor no posee un certificado." });

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _services.DeleteAsync(id);
                if (!deleted) return NotFound(new { mensaje = "Profesor no encontrado" });

                return Ok(new { mensaje = "Profesor eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
