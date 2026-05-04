using Aplication.DTOs.Request.Admin;
using Aplication.Interfaces.IAdmin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SistemaGolAhora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministradorController : ControllerBase
    {
        private readonly IAdminService _services;
        private readonly IAdminMapper _mapper;

        public AdministradorController(IAdminService services, IAdminMapper mapper)
        {
            _services = services;
            _mapper = mapper;
        }

        [HttpPut("{id}/simple")]
        public async Task<IActionResult> UpdateSimple(int id, [FromBody] UpdateAdministradorSimpleRequest request)
        {
            try
            {
                var response = await _services.UpdateSimpleAsync(id, request);
                if (response == null) return NotFound(new { mensaje = "Administrador no encontrado" });
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
                var deleted = await _services.DeleteAsync(id);
                if (!deleted) return NotFound(new { mensaje = "Administrador no encontrado" });

                return Ok(new { mensaje = "Administrador eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
