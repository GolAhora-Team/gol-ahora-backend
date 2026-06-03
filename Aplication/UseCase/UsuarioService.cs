using Aplication.DTOs.Request.Usuario;
using Aplication.DTOs.Response.Usuario;
using Aplication.Interfaces.IAdmin;
using Aplication.Interfaces.ICliente;
using Aplication.Interfaces.IProfesor;
using Aplication.Interfaces.IUsuario;
using Domain.Exceptions;
using System.Net;
using System.Net.Mail;

namespace Aplication.UseCase
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioCommand _usuariocommand;
        private readonly IUsuarioQuery _usuarioQuery;
        private readonly IUsuarioMapper _usuariomapper;
        private readonly IClienteMapper _clientemapper;
        private readonly IClientesCommand _clienteCommand;
        private readonly IClientesQuery _clientesQuery;
        private readonly IAdminCommand _adminCommand;
        private readonly IAdminMapper _adminMapper;
        private readonly IAdminQuery _adminQuery;
        private readonly IProfesorCommand _profesorCommand;
        private readonly IProfesorMapper _profesorMapper;
        private readonly IProfesorQuery _profesorQuery;

        public UsuarioService(IUsuarioCommand usuariocommand, IUsuarioMapper usuariomapper, IClienteMapper clientemapper, IClientesCommand clienteCommand, IAdminMapper adminMapper, IAdminCommand adminCommand, IProfesorMapper profesorMapper, IProfesorCommand profesorCommand, IUsuarioQuery usuarioQuery, IProfesorQuery profesorQuery, IClientesQuery clientesQuery, IAdminQuery adminQuery)
        {
            _usuariocommand = usuariocommand;
            _usuariomapper = usuariomapper;
            _clientemapper = clientemapper;
            _clienteCommand = clienteCommand;
            _adminMapper = adminMapper;
            _adminCommand = adminCommand;
            _profesorMapper = profesorMapper;
            _profesorCommand = profesorCommand;
            _usuarioQuery = usuarioQuery;
            _profesorQuery = profesorQuery;
            _clientesQuery = clientesQuery;
            _adminQuery = adminQuery;
        }

        public async Task<UsuarioClienteResponse> CreateUsuarioCliente(CreateUsuarioClienteRequest usuario)
        {
            if (usuario.Cliente.Dni < 0)
            {
                throw new ExceptionBadRequest("El DNI no puede ser negativo.");
            }

            var availability = await CheckAvailability(usuario.Cliente.Dni, usuario.Email, usuario.Username);
            if (availability.Any())
            {
                throw new ExceptionBadRequest($"Los siguientes datos ya están en uso: {string.Join(", ", availability)}");
            }

            var clienteEntity = _clientemapper.CreateCliente(usuario.Cliente);
            await _clienteCommand.InsertCliente(clienteEntity);

            var usuarioEntity = _usuariomapper.CreateUsuario(usuario, clienteEntity.Id);
            await _usuariocommand.Create(usuarioEntity);

            return _usuariomapper.CreateUsuarioResponse(usuarioEntity.Id, clienteEntity.Nombre, clienteEntity.Apellido);
        }

        public async Task<UsuarioAdminResponse> CreateUsuarioAdmin(CreateUsuarioAdminRequest usuario)
        {
            if (usuario.Admin.Dni < 0)
            {
                throw new ExceptionBadRequest("El DNI no puede ser negativo.");
            }

            var availability = await CheckAvailability(usuario.Admin.Dni, usuario.Email, usuario.Username);
            if (availability.Any())
            {
                throw new ExceptionBadRequest($"Los siguientes datos ya están en uso: {string.Join(", ", availability)}");
            }

            var adminEntity = _adminMapper.CreateAdminToRequest(usuario.Admin);
            await _adminCommand.CreateAsync(adminEntity);

            var usuarioEntity = _usuariomapper.CreateUsuario(usuario, adminEntity.Id);
            await _usuariocommand.Create(usuarioEntity);

            return _usuariomapper.CreateUsuarioAdminResponse(usuarioEntity.Id, adminEntity.Nombre, adminEntity.Apellido, adminEntity.FechaAlta);
        }

        public async Task<UsuarioProfesorResponse> CreateUsuarioProfesor(CreateUsuarioProfesorRequest usuario)
        {
            if (usuario.request.Dni < 0)
            {
                throw new ExceptionBadRequest("El DNI no puede ser negativo.");
            }

            var availability = await CheckAvailability(usuario.request.Dni, usuario.Email, usuario.Username);
            if (availability.Any())
            {
                throw new ExceptionBadRequest($"Los siguientes datos ya están en uso: {string.Join(", ", availability)}");
            }

            if (!string.IsNullOrEmpty(usuario.request.CertificadoBase64))
            {
                var base64Data = usuario.request.CertificadoBase64;
                if (base64Data.Contains(","))
                {
                    base64Data = base64Data.Split(',')[1];
                }
                // Validar tamaño: máximo 4MB
                // Fórmula aproximada de base64 a bytes: (longitud base64 * 3) / 4
                var sizeInBytes = (base64Data.Length * 3) / 4;
                if (sizeInBytes > 4 * 1024 * 1024)
                {
                    throw new ExceptionBadRequest("El certificado excede el límite máximo de 4 MB.");
                }
            }

            var profesorEntity = _profesorMapper.CreateProfesorToProfesorRequest(usuario.request);
            await _profesorCommand.CreateProfesor(profesorEntity);

            var usuarioEntity = _usuariomapper.CreateUsuario(usuario, profesorEntity.Id);
            await _usuariocommand.Create(usuarioEntity);

            return _usuariomapper.CreateUsuarioProfesorResponse(usuarioEntity.Id, profesorEntity.Nombre, profesorEntity.Apellido, profesorEntity.Especialidad);
        }

        public async Task<UsuarioLogginResponse> LogginUsuario(LoginRequest loginRequest)
        {
            var usuarioEntity = await _usuarioQuery.GetByIdentifierAndPassword(loginRequest.Email, loginRequest.Password);
            if (usuarioEntity == null)
            {
                throw new ExceptionNotFound("Usuario no encontrado.");
            }

            if (usuarioEntity.TipoUsuario == Domain.Enums.TipoUsuario.Cliente)
            {
                var clienteEntity = await _clientesQuery.GetClienteById(usuarioEntity.PersonaId);
                if (clienteEntity == null)
                {
                    throw new ExceptionNotFound("Cliente no encontrado.");
                }
                return new UsuarioLogginResponse
                {
                    IdUsuario = usuarioEntity.Id,
                    TipoUsuario = usuarioEntity.TipoUsuario,
                    IdPersona = clienteEntity.Id,
                    Nombre = clienteEntity.Nombre,
                    Apellido = clienteEntity.Apellido
                };
            }
            else if (usuarioEntity.TipoUsuario == Domain.Enums.TipoUsuario.Administrador)
            {
                var adminEntity = await _adminQuery.GetByIdAsync(usuarioEntity.PersonaId);
                if (adminEntity == null)
                {
                    throw new ExceptionNotFound("Admin no encontrado.");
                }
                return new UsuarioLogginResponse
                {
                    IdUsuario = usuarioEntity.Id,
                    TipoUsuario = usuarioEntity.TipoUsuario,
                    IdPersona = adminEntity.Id,
                    Nombre = adminEntity.Nombre,
                    Apellido = adminEntity.Apellido,
                    Identificador = adminEntity.Identificador
                };
            }
            else if (usuarioEntity.TipoUsuario == Domain.Enums.TipoUsuario.Profesor)
            {
                var profesorEntity = await _profesorQuery.GetByIdAsync(usuarioEntity.PersonaId);
                if (profesorEntity == null)
                {
                    throw new ExceptionNotFound("Profesor no encontrado.");
                }
                return new UsuarioLogginResponse
                {
                    IdUsuario = usuarioEntity.Id,
                    TipoUsuario = usuarioEntity.TipoUsuario,
                    IdPersona = profesorEntity.Id,
                    Nombre = profesorEntity.Nombre,
                    Apellido = profesorEntity.Apellido
                };
            }
            else
            {
                throw new ExceptionBadRequest("Tipo de usuario no válido.");
            }
        }

        public async Task<bool> ChangePassword(ChangePasswordRequest request)
        {
            var usuario = await _usuarioQuery.GetById(request.IdUsuario);
            if (usuario == null)
            {
                throw new ExceptionNotFound("Usuario no encontrado.");
            }

            if (usuario.PasswordHash != request.CurrentPassword)
            {
                throw new ExceptionBadRequest("La contraseña actual es incorrecta.");
            }

            usuario.PasswordHash = request.NewPassword;
            await _usuariocommand.Update(usuario);

            return true;
        }

        public async Task<List<string>> CheckAvailability(int dni, string email, string username)
        {
            return await _usuarioQuery.CheckUniqueness(dni, email, username);
        }


        public async Task<bool> ForgotPassword(string email)
        {
            var usuarioEntity = await _usuarioQuery.GetByEmail(email);
            if (usuarioEntity == null)
            {
                // Por seguridad es mejor no revelar si el email existe o no, pero seguiremos el patrón de la app.
                throw new ExceptionNotFound("Usuario no encontrado con ese correo.");
            }

            // Generar token y expiración
            var token = Guid.NewGuid().ToString();
            usuarioEntity.ResetToken = token;
            usuarioEntity.ResetTokenExpires = DateTime.UtcNow.AddHours(1);

            await _usuariocommand.Update(usuarioEntity);

            // Enviar email
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("complejogolahora@gmail.com", "tgcf xugs czdh ekpx"),
                    EnableSsl = true,
                };

                var frontendUrl = $"https://gol-ahora-git-develop-javifl-proyects.vercel.app/NuevaClave?token={token}";

                var htmlBody = $@"
<!DOCTYPE html>
<html lang=""es"">
<head>
  <meta charset=""UTF-8"">
  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
</head>
<body style=""margin:0;padding:0;background-color:#004d1a;font-family:Arial,Helvetica,sans-serif;"">
  <table role=""presentation"" width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background-color:#004d1a;padding:40px 0;"">
    <tr>
      <td align=""center"">
        <table role=""presentation"" width=""480"" cellpadding=""0"" cellspacing=""0"" style=""max-width:480px;width:100%;border-radius:30px;overflow:hidden;border:2px solid rgba(255,255,255,0.3);background-color:#006400;position:relative;"">
          <!-- Líneas de cancha decorativas -->
          <tr>
            <td style=""height:0;position:relative;"">
              <div style=""position:absolute;top:0;left:20%;right:20%;height:40px;border-bottom:2px solid rgba(255,255,255,0.2);border-left:2px solid rgba(255,255,255,0.2);border-right:2px solid rgba(255,255,255,0.2);""></div>
            </td>
          </tr>
          <!-- Header -->
          <tr>
            <td align=""center"" style=""padding:50px 30px 20px 30px;"">
              <p style=""color:#ffffff;font-size:14px;font-weight:300;letter-spacing:3px;margin:0 0 5px 0;"">Complejo</p>
              <h1 style=""color:#ffffff;font-size:42px;font-weight:900;letter-spacing:-1px;margin:0;"">GOL AHORA</h1>
              <div style=""display:inline-block;background-color:#ffb300;padding:4px 14px;border-radius:4px;margin-top:8px;"">
                <span style=""color:#000000;font-size:10px;font-weight:900;letter-spacing:1px;"">RECUPERACIÓN DE CONTRASEÑA</span>
              </div>
            </td>
          </tr>
          <!-- Card blanca -->
          <tr>
            <td align=""center"" style=""padding:0 25px 40px 25px;"">
              <table role=""presentation"" width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background-color:#ffffff;border-radius:25px;overflow:hidden;box-shadow:0 10px 30px rgba(0,0,0,0.3);"">
                <tr>
                  <td style=""padding:35px 30px;"">
                    <!-- Icono -->
                    <div style=""text-align:center;margin-bottom:20px;"">
                      <div style=""display:inline-block;width:60px;height:60px;border-radius:50%;background-color:#f0fdf4;line-height:60px;text-align:center;"">
                        <span style=""font-size:30px;"">🔑</span>
                      </div>
                    </div>
                    <p style=""color:#1e293b;font-size:18px;font-weight:700;text-align:center;margin:0 0 10px 0;"">¡Hola!</p>
                    <p style=""color:#475569;font-size:14px;line-height:22px;text-align:center;margin:0 0 25px 0;"">
                      Recibimos una solicitud para restablecer la contraseña de tu cuenta en <strong style=""color:#009b3a;"">Complejo Gol Ahora</strong>. 
                      Hacé clic en el botón de abajo para crear una nueva contraseña.
                    </p>
                    <!-- Botón -->
                    <div style=""text-align:center;margin-bottom:25px;"">
                      <a href=""{frontendUrl}"" target=""_blank"" style=""display:inline-block;background:linear-gradient(135deg,#ffb300,#ff9100);color:#000000;font-size:16px;font-weight:900;text-decoration:none;padding:16px 40px;border-radius:15px;letter-spacing:0.5px;"">
                        RESTABLECER CONTRASEÑA
                      </a>
                    </div>
                    <!-- Info -->
                    <div style=""background-color:#f0fdf4;border:1px solid #bbf7d0;border-radius:10px;padding:12px 15px;margin-bottom:20px;"">
                      <p style=""color:#16a34a;font-size:12px;font-weight:600;margin:0;text-align:center;"">
                        ⏱ Este enlace expira en 1 hora por seguridad.
                      </p>
                    </div>
                    <p style=""color:#94a3b8;font-size:12px;text-align:center;margin:0 0 15px 0;"">
                      Si no solicitaste este cambio, podés ignorar este correo. Tu contraseña no será modificada.
                    </p>
                    <!-- Línea divisoria -->
                    <hr style=""border:none;border-top:1px solid #e2e8f0;margin:20px 0;"" />
                    <p style=""color:#cbd5e1;font-size:11px;text-align:center;margin:0;"">
                      Complejo Gol Ahora · Sistema de Gestión Deportiva<br/>
                      S.A. CUIT: 30-12345678-3
                    </p>
                  </td>
                </tr>
              </table>
            </td>
          </tr>
          <!-- Líneas de cancha decorativas inferiores -->
          <tr>
            <td style=""height:40px;position:relative;"">
              <div style=""position:absolute;bottom:0;left:20%;right:20%;height:40px;border-top:2px solid rgba(255,255,255,0.2);border-left:2px solid rgba(255,255,255,0.2);border-right:2px solid rgba(255,255,255,0.2);""></div>
            </td>
          </tr>
        </table>
      </td>
    </tr>
  </table>
</body>
</html>";

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("complejogolahora@gmail.com", "Complejo Gol Ahora"),
                    Subject = "Recuperación de Contraseña - Complejo Gol Ahora",
                    Body = htmlBody,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(email);

                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error enviando email de recuperacion: " + ex.ToString());
                throw new Exception("Error al enviar el correo de recuperación: " + ex.Message);
            }
        }

        public async Task ResetPassword(ResetPasswordRequest request)
        {
            var usuario = await _usuarioQuery.GetByResetToken(request.Token);
            if (usuario == null)
            {
                throw new ExceptionNotFound("Token inválido o expirado.");
            }

            if (usuario.ResetTokenExpires < DateTime.UtcNow)
            {
                throw new ExceptionBadRequest("El token de recuperación ha expirado.");
            }

            if (usuario.PasswordHash == request.NewPassword)
            {
                throw new ExceptionBadRequest("La nueva contraseña no puede ser igual a la anterior.");
            }

            usuario.PasswordHash = request.NewPassword;
            usuario.ResetToken = null;
            usuario.ResetTokenExpires = null;

            await _usuariocommand.Update(usuario);
        }

        public async Task<bool> ValidateResetToken(string token)
        {
            if (string.IsNullOrEmpty(token)) return false;
            var usuario = await _usuarioQuery.GetByResetToken(token);
            if (usuario == null) return false;
            if (usuario.ResetTokenExpires < DateTime.UtcNow) return false;
            return true;
        }

        public async Task UploadAptoMedico(UploadAptoMedicoRequest request)
        {
            if (string.IsNullOrEmpty(request.ArchivoBase64))
            {
                throw new ExceptionBadRequest("No se ha proporcionado ningún archivo.");
            }

            var base64Data = request.ArchivoBase64;
            if (base64Data.Contains(","))
            {
                base64Data = base64Data.Split(',')[1];
            }
            var sizeInBytes = (base64Data.Length * 3) / 4;

            if (sizeInBytes > 2 * 1024 * 1024)
            {
                throw new ExceptionBadRequest("El tamaño del archivo no puede exceder los 2MB.");
            }

            if (request.FechaFin <= request.FechaInicio)
            {
                throw new ExceptionBadRequest("La fecha de fin debe ser posterior a la fecha de inicio.");
            }

            var cliente = await _clientesQuery.GetClienteById(request.ClienteId);
            if (cliente == null)
            {
                throw new ExceptionNotFound("Cliente no encontrado.");
            }

            cliente.AptoMedicoArchivo = Convert.FromBase64String(base64Data);
            cliente.AptoMedicoFechaInicio = request.FechaInicio;
            cliente.AptoMedicoFechaFin = request.FechaFin;
            cliente.AptoFisico = true;

            await _clienteCommand.UpdateCliente(cliente);
        }
    }
}
