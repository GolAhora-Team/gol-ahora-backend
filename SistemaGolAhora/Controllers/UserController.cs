using Aplication.DTOs.Request.Usuario;
using Aplication.Interfaces.IUsuario;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> CreateUsuarioProfesor([FromBody] CreateUsuarioProfesorFormRequest request)
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

        [HttpGet("validate-reset-token")]
        public async Task<IActionResult> ValidateResetToken([FromQuery] string token)
        {
            try
            {
                var isValid = await _usuarioService.ValidateResetToken(token);
                return Ok(new { isValid });
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

        [HttpGet("seed-julian")]
        public async Task<IActionResult> SeedJulian([FromServices] AppDbContext context)
        {
            try
            {
                var email = "juliannicolasantunes@gmail.com";
                var persona = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(
                    context.Personas, p => p.Email == email);
                
                if (persona == null)
                {
                    return BadRequest(new { message = "No se encontró ninguna Persona con el correo juliannicolasantunes@gmail.com en la base de datos." });
                }

                var usuarioExistente = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(
                    context.Usuarios, u => u.PersonaId == persona.Id);

                if (usuarioExistente != null)
                {
                    if (usuarioExistente.Email != email)
                    {
                        usuarioExistente.Email = email;
                        context.Usuarios.Update(usuarioExistente);
                        await context.SaveChangesAsync();
                        return Ok(new { message = "El usuario ya existía en la tabla Usuarios pero con otro email. Se actualizó al correo correcto.", username = usuarioExistente.Username, email = usuarioExistente.Email, id = usuarioExistente.Id });
                    }
                    return Ok(new { message = "El usuario ya existe en la tabla Usuarios con el email correcto.", username = usuarioExistente.Username, email = usuarioExistente.Email, id = usuarioExistente.Id });
                }

                var nuevoUsuario = new Domain.Entities.Usuario
                {
                    Username = "julianantunes",
                    Email = email,
                    PasswordHash = "1234",
                    PersonaId = persona.Id,
                    TipoUsuario = Domain.Enums.TipoUsuario.Cliente
                };

                context.Usuarios.Add(nuevoUsuario);
                await context.SaveChangesAsync();

                return Ok(new { message = "Usuario creado y enlazado con éxito.", username = nuevoUsuario.Username, email = nuevoUsuario.Email, id = nuevoUsuario.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, detail = ex.ToString() });
            }
        }
        [HttpGet("buscar-username/{username}")]
        public async Task<IActionResult> BuscarPorUsername(string username, [FromServices] AppDbContext context)
        {
            try
            {
                var usuario = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(
                    context.Usuarios.Include(u => u.Persona),
                    u => u.Username.ToLower() == username.Trim().ToLower());

                if (usuario == null)
                {
                    return Ok(new { existe = false });
                }

                // Solo clientes pueden ser invitados
                if (usuario.TipoUsuario != Domain.Enums.TipoUsuario.Cliente)
                {
                    return Ok(new { existe = false, mensaje = "El usuario no es de tipo Cliente" });
                }

                var persona = usuario.Persona;
                return Ok(new
                {
                    existe = true,
                    usuarioId = usuario.Id,
                    clienteId = persona?.Id,
                    nombre = persona != null ? $"{persona.Nombre} {persona.Apellido}" : usuario.Username,
                    username = usuario.Username
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
