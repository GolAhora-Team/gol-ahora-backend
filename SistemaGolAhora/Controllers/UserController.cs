using Aplication.DTOs.Request.Usuario;
using Aplication.Interfaces.IUsuario;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SistemaGolAhora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UserController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("Cliente")]
        public async Task<IActionResult> CreateUsuarioCliente([FromBody] CreateUsuarioClienteRequest request)
        {
            try
            {
                var response = await _usuarioService.CreateUsuarioCliente(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Admin")]
        public async Task<IActionResult> CreateUsuarioAdmin([FromBody] CreateUsuarioAdminRequest request)
        {
            try
            {
                var response = await _usuarioService.CreateUsuarioAdmin(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Profesor")]
        public async Task<IActionResult> CreateUsuarioProfesor([FromBody] CreateUsuarioProfesorRequest request)
        {
            try
            {
                var response = await _usuarioService.CreateUsuarioProfesor(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var response = await _usuarioService.LogginUsuario(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
