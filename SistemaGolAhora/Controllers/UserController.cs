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
                return BadRequest(new { mensaje = ex.Message });
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
                return BadRequest(new { mensaje = ex.Message });
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
                return BadRequest(new { mensaje = ex.Message });
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
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                var response = await _usuarioService.ChangePassword(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("check-availability")]
        public async Task<IActionResult> CheckAvailability([FromQuery] int dni, [FromQuery] string email, [FromQuery] string username)
        {
            try
            {
                var takenFields = await _usuarioService.CheckAvailability(dni, email, username);
                return Ok(takenFields);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        public class ForgotPasswordRequestDto
        {
            public string Email { get; set; }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto request)
        {
            try
            {
                var success = await _usuarioService.ForgotPassword(request.Email);
                return Ok(new { success, message = "Correo de recuperación enviado exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            try
            {
                await _usuarioService.ResetPassword(request);
                return Ok(new { success = true, message = "Contraseña restablecida exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("apto-medico")]
        public async Task<IActionResult> UploadAptoMedico([FromBody] UploadAptoMedicoRequest request)
        {
            try
            {
                await _usuarioService.UploadAptoMedico(request);
                return Ok(new { success = true, message = "Apto médico subido exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
